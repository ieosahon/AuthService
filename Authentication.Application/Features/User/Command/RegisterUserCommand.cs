// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
namespace Authentication.Application.Features.User.Command;

public class RegisterUserCommand : IRequest<Result<RegistrationResponse>>
{
    public RegistrationRequest Request { get; set; }
    public RegisterUserCommand(RegistrationRequest request)
    {
        Request = request;
    }
}

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<RegistrationResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IResponseHelper _responseHelper;
    private readonly ISendEmail _sendEmail;
    private readonly IConfiguration _config;

    public RegisterUserHandler(IUnitOfWork unitOfWork, IResponseHelper responseHelper, ISendEmail sendEmail, IConfiguration config)
    {
        _unitOfWork = unitOfWork;
        _responseHelper = responseHelper;
        _sendEmail = sendEmail;
        _config = config;
    }

    public async Task<Result<RegistrationResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingUser = await _unitOfWork.Repository<Domain.Models.User>().GetSingle(x => x.Email == request.Request.Email);
            var lga = await _unitOfWork.Repository<LGA>().GetSingle(x => x.Id == request.Request.LGAId);
            if (existingUser is not null)
            {
                return _responseHelper.ResponseT<RegistrationResponse>(null, $"User with email {request.Request.Email} already exist.",
                    "409", request.Request.Email, 
                    $"Response for user user registration ==> User with email {request.Request.Email} already exist.");
            }
            
            if (lga is null)
            {
                return _responseHelper.ResponseT<RegistrationResponse>(null, $"Lga entered is not correct.",
                    "404", request.Request.Email, 
                    $"Response for user user registration ==> Lga with id {request.Request.LGAId}.");
            }

            if (request.Request.HasBVN is false)
            {
               request.Request.BVN = Helper.GenerateRandom11DigitNumber();
               request.Request.HasBVN = true;
            }
            
            if (string.IsNullOrWhiteSpace(request.Request.NIN))
            {
               request.Request.BVN = Helper.GenerateRandom11DigitNumber();
            }
            var accountNumber = Helper.GenerateRandom10DigitNumber();
            var token = DateTime.Now.Ticks.ToString()[..5];
            var user = UserObject(request.Request, accountNumber);
            var verifyEmail = new TwoFAToken
            {
                Token = token,
                Email = request.Request.Email,
                PhoneNumber = request.Request.PhoneNumber,
                TwoFATokenType = TwoFATokenTypeEnum.EmailVerification.ToString()
            };
            var transactionLimit = new TransactionLimit
            {
                AccountNumber = accountNumber,
                DailyLimit = 5000000,
            };
            _unitOfWork.Repository<Domain.Models.User>().Add(user);
            _unitOfWork.Repository<TwoFAToken>().Add(verifyEmail);
            _unitOfWork.Repository<TransactionLimit>().Add(transactionLimit);
            
            var emailMessage = new EmailMessage
            {
                To = new List<string>{request.Request.Email},
                Subject = "New account email verification",
                Body = NewAccountEmailTemplate.ConfirmEmail(request.Request.FirstName, accountNumber, token, request.Request.AccountType),
                Purpose = "Account Opening"
            };
            var sendEmail = await _sendEmail.SendEmailAsync(emailMessage);
            if (!sendEmail.ResponseCode.Equals("200"))
            {
                return _responseHelper.ResponseT<RegistrationResponse>(null, $"An error occurred while creating account. Please check your internet and try again.",
                    "400", request.Request.Email, 
                    $"Response for user user registration ==> confirmation email not sent.");
            }

            var save = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (save < 1)
            {
                return _responseHelper.ResponseT<RegistrationResponse>(null, $"An error occurred while creating account. Please check your internet and try again.",
                    "400", request.Request.Email, 
                    $"Response for user user registration ==> database issue.");
            }

            var jwtToken = Helper.GenerateJwtToken(request.Request.Email, request.Request.LastName, request.Request.FirstName, _config);
            var response = UserRegistrationResponse(user, accountNumber, jwtToken, request.Request.LGAId);
            return _responseHelper.Response(response, $"Account successfully created. Please confirm your email to enjoy banking like never before. Account number is {accountNumber}",
                "200", request.Request.Email, 
                $"Account successfully created. Please confirm your email to enjoy banking like never before.");


        }
        catch (Exception ex)
        {
            return _responseHelper.ResponseT<RegistrationResponse>(null, $"An error occurred, please check your internet and try again.",
                "500", request.Request.Email, 
                $"Response for user registration ==> err ==> {ex.Message}; st ==> {ex.StackTrace}.");
        }
    }

    private Domain.Models.User UserObject(RegistrationRequest user, string accountNumber)
    {
        
        return new Domain.Models.User
        {
            PasswordHash = Helper.HashString(user.Password)[..10],
            FirstName = user.FirstName,
            LastName = user.LastName,
            MiddleName = user.MiddleName,
            NIN = user.NIN,
            BVN = user.BVN,
            Email = user.Email,
            AccountType = user.AccountType,
            Gender = user.Gender,
            HasBVN = user.HasBVN,
            Title = user.Title,
            Age = Helper.Age(user.DOB),
            DOB = user.DOB,
            Address = user.Address,
            //LGAId = user.LGAId,
            LandMark = user.LandMark,
            AccountNumber = accountNumber,
            Status = AccountStatusEnum.NotActive.ToString(),
            FailedLoginCount = 0,
            CreatedAt = DateTime.UtcNow.AddHours(1),
            UpdatedAt = DateTime.UtcNow.AddHours(1),
            EmailConfirmed = false,
            PhoneNumber = user.PhoneNumber
            //LastLogin = DateTime.UtcNow
        };
    }

    private static RegistrationResponse UserRegistrationResponse(Domain.Models.User user, string accountNumber, string token, uint lgaId)
    {

        return new RegistrationResponse
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            MiddleName = user.MiddleName,
            NIN = user.NIN,
            BVN = user.BVN,
            AccountType = user.AccountType,
            Gender = user.Gender,
            HasBVN = user.HasBVN,
            Title = user.Title,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            LGAId = lgaId,
            LandMark = user.LandMark,
            AccountNumber = accountNumber,
            Status = user.Status,
            Token = token,
            Email = user.Email
        };
    }
}