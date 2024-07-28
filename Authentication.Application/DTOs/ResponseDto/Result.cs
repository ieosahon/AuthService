// ReSharper disable ClassNeverInstantiated.Global
namespace Authentication.Application.DTOs.ResponseDto;

public class Result
{
    public string ResponseCode { get; set; }
    public string ResponseMsg { get; set; }
    public PaginationDetails PaginationDetails { get; set; }
}

public class PaginationDetails
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
}


public class Result<T> : Result
{
    public T ResponseDetails { get; set; }

    public static Result<T> Success(T instance, string message = "successful")
    {
        return new Result<T>
        {
            ResponseCode = "200",
            ResponseDetails = instance,
            ResponseMsg = message,
        };
    }

    public static Result<T> Failed(T instance, string message = "BadRequest")
    {
        return new Result<T>
        {
            ResponseCode = "400",
            ResponseDetails = instance,
            ResponseMsg = message,
        };
    }
    
    public static Result<T> NotFound(T instance, string message = "BadRequest")
    {
        return new Result<T>
        {
            ResponseCode = "404",
            ResponseDetails = instance,
            ResponseMsg = message,
        };
    }
    public static Result<T> Unauthorized(T instance, string message = "Unauthorized")
    {
        return new Result<T>
        {
            ResponseCode = "401",
            ResponseDetails = instance,
            ResponseMsg = message,
        };
    }

    public static Result<T> Error(T instance, string message = "An error occured while processing your request")
    {
        return new Result<T>
        {
            ResponseCode = "500",
            ResponseDetails = instance,
            ResponseMsg = message,
        };
    }
    public static Result<T> Duplicate(T instance, string message = "Duplicate request")
    {
        return new Result<T>
        {
            ResponseCode = "409",
            ResponseDetails = instance,
            ResponseMsg = message,
        };
    }
}