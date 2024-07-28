namespace Authentication.Application.Contracts.UtilityContracts;

public interface IResponseHelper
{
    Result<T> ResponseT<T>(T details, string msg, string code, string uniqueIdentifier, string logMessage);
    Result<T> Response<T>(T details, string msg, string code, string uniqueIdentifier, string logMessage);
}