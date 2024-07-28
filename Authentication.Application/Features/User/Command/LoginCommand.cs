// ReSharper disable ClassNeverInstantiated.Global

namespace Authentication.Application.Features.User.Command;

public class LoginCommand : IRequest<Result<LoginResponse>>
{
    public LoginDto Request { get; }

    public LoginCommand(LoginDto request)
    {
        Request = request;
    }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IResponseHelper _responseHelper;
    private readonly ISendEmail _sendEmail;
    private readonly IConfiguration _config;
    private readonly IDbQuery _dbQuery;

    public LoginCommandHandler(IUnitOfWork unitOfWork, IResponseHelper responseHelper, ISendEmail sendEmail, IConfiguration config, IDbQuery dbQuery)
    {
        _unitOfWork = unitOfWork;
        _responseHelper = responseHelper;
        _sendEmail = sendEmail;
        _config = config;
        _dbQuery = dbQuery;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _unitOfWork.Repository<Domain.Models.User>().GetSingle(x => x.AccountNumber.Equals(request.Request.AccountNumber));
            if (user is null)
            {
                return _responseHelper.ResponseT<LoginResponse>(null, $"Incorrect user details.",
                    "404", request.Request.AccountNumber,
                    $"Response for user login ==> User with account {request.Request.AccountNumber} does not exist.");
            }

            if (user.FailedLoginCount == nuint.Parse(_config["FailedLoginCount"]))
            {
                return _responseHelper.ResponseT<LoginResponse>(null, $"Account has been locked. Please reset your password to unlock your account.",
                    "404", request.Request.AccountNumber,
                    $"Response for user login ==> User account with account {request.Request.AccountNumber} has been locked.");
            }

            var password = Helper.HashString(request.Request.Password)[..10];
            if (password != user.PasswordHash)
            {
                user.FailedLoginCount++;
                if (user.FailedLoginCount == nuint.Parse(_config["FailedLoginCount"]))
                {
                    user.Status = AccountStatusEnum.Lock.ToString();
                }

                var loginAttemptLeft = nuint.Parse(_config["FailedLoginCount"]) - user.FailedLoginCount;
                _unitOfWork.Repository<Domain.Models.User>().Update(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                var message = nuint.Parse(_config["FailedLoginCount"]) - user.FailedLoginCount == 0
                    ? "Incorrect user details, account is now lock. Please reset your password to unlock your account."
                    : $"Incorrect user details. Login attempt left is {loginAttemptLeft}";
                return _responseHelper.ResponseT<LoginResponse>(null, message,
                    "404", request.Request.AccountNumber,
                    $"Response for user login ==> User with account {request.Request.AccountNumber} did not enter the correct password.");
            }

            if (!user.EmailConfirmed)
            {
                var token = DateTime.Now.Ticks.ToString()[..5];
                var verifyEmail = new TwoFAToken
                {
                    Token = token,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    TwoFATokenType = TwoFATokenTypeEnum.EmailVerification.ToString()
                };
                var emailMessage = new EmailMessage
                {
                    To = new List<string> { user.Email },
                    Subject = "New account email verification",
                    Body = NewAccountEmailTemplate.ConfirmEmail(user.FirstName, user.AccountNumber, token, user.AccountType),
                    Purpose = "Account Verification"
                };
                _unitOfWork.Repository<TwoFAToken>().Add(verifyEmail);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _sendEmail.SendEmailAsync(emailMessage);
                return _responseHelper.ResponseT<LoginResponse>(null, $"Account is yet to be confirm. Please use the pass code sent to your email to confirm your account.",
                    "404", request.Request.AccountNumber,
                    $"Response for user login ==> User account with account {request.Request.AccountNumber} is not verified.");
            }

            if (user.Status != AccountStatusEnum.Active.ToString())
            {
                return _responseHelper.ResponseT<LoginResponse>(null, $"Account is not active. Please reset  your  password.",
                    "404", request.Request.AccountNumber,
                    $"Response for user login ==> User account with account {request.Request.AccountNumber} is {user.Status}.");
            }

            var lgaAndStateName = await _dbQuery.GetLgaAndStateByLgaId(user.LgaId);
            var lastLogin = DateTime.UtcNow.AddHours(1);
            user.LastLogin = lastLogin;
            _unitOfWork.Repository<Domain.Models.User>().Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var userLoginResponse = UserLoginResponseResponse(user, lgaAndStateName, lastLogin, _config);
            return _responseHelper.Response(userLoginResponse, $"Login successful.",
                "200", request.Request.AccountNumber,
                $"Response for user login ==> User account with account {request.Request.AccountNumber} successfully login.");
        }
        catch (Exception ex)
        {
            return _responseHelper.ResponseT<LoginResponse>(null, $"An error occurred, please check your internet and try again.",
                "500", request.Request.AccountNumber,
                $"Response for user Login ==> err ==> {ex.Message}; st ==> {ex.StackTrace}.");
        }
    }

    private static LoginResponse UserLoginResponseResponse(Domain.Models.User user, LgaAndState lgaAndState, DateTime lastLogin, IConfiguration config)
    {
        return new LoginResponse
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            MiddleName = user.MiddleName,
            NIN = user.NIN,
            BVN = user.BVN,
            AccountType = user.AccountType,
            Gender = user.Gender,
            Title = user.Title,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            StateName = lgaAndState.StateName ?? "",
            LgaName = lgaAndState.LgaName ?? "",
            LandMark = user.LandMark,
            AccountNumber = user.AccountNumber,
            Status = user.Status,
            Token = Helper.GenerateJwtToken(user.Email, user.LastName, user.FirstName, config),
            Email = user.Email,
            LastLogin = lastLogin
        };
    }
}