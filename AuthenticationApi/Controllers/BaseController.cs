namespace AuthenticationApi.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected readonly IMediator Mediator;
    public BaseController( IMediator mediator)
    {
        Mediator = mediator;
    }

    internal IActionResult HandleResponse<T>(Result<T> result)
    {
        return result.ResponseCode switch
        {
            "200" => Ok(result),
            "400" => BadRequest(result),
            "401" => Unauthorized(result),
            "404" => NotFound(result),
            "409" => Conflict(result),
            _ => StatusCode(500, result)
        };
    }
}