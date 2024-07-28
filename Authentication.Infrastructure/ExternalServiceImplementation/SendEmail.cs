namespace Authentication.Infrastructure.ExternalServiceImplementation;

public class SendEmail : ISendEmail
{
    private readonly IConfiguration _config;
    private readonly ILogger _logger;
    private readonly IRestHelper _restHelper;

    public SendEmail(ILogger logger, IConfiguration config, IRestHelper restHelper)
    {
        _logger = logger;
        _config = config;
        _restHelper = restHelper;
    }

    public async Task<Result<EmailResponse>> SendEmailAsync(EmailMessage request)
    {
         var result = new Result<EmailResponse>();

        try
        {
            var url = _config["SendEmailUrl"];
            var response = await _restHelper.ConsumeApi<EmailResponse>("AuthService", url, request, "NotificationService" ,ApiType.Post);
            result.ResponseCode = response.ResponseCode;
            result.ResponseMsg = response.ResponseMsg;
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"err from send email method ==> {ex.Message}; st ==> {ex.StackTrace}");
            result.ResponseCode = "500";
            result.ResponseMsg = "Error";
            return result;
        }
    }
}