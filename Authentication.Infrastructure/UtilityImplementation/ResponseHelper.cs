namespace Authentication.Infrastructure.UtilityImplementation;

public class ResponseHelper : IResponseHelper
{
    private readonly ILogger _logger;

    public ResponseHelper(ILogger logger)
    {
        _logger = logger;
    }

    public Result<T> ResponseT<T>(T details, string msg, string code, string uniqueIdentifier, string logMessage)
    {
        var result = new Result<T>
        {
            ResponseMsg = msg,
            ResponseCode = code,
            ResponseDetails = details
        };
        _logger.Information($"ResponseDetails for user {uniqueIdentifier} {logMessage} ==> {JsonConvert.SerializeObject(result)}");
        return result;
    }
    
    public Result<T> Response<T>(T details, string msg, string code, string uniqueIdentifier, string logMessage)
    {
        var result = new Result<T>
        {
            ResponseMsg = msg,
            ResponseCode = code,
            ResponseDetails = details
        };
        _logger.Information($"ResponseDetails for user {uniqueIdentifier} {logMessage}.");
        return result;
    }
}