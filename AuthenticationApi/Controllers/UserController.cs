namespace AuthenticationApi.Controllers;

[Route("user/")]
public class UserController : BaseController
{
    public UserController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("registration")]
    [GenericRequestValidator(typeof(RegistrationRequestValidation), "Authentication.Application.DTOs.RequestDto.RegistrationRequest")]
    public async Task<IActionResult> Register([FromBody] [Bind] RegistrationRequest request)
        => HandleResponse(await Mediator.Send(new RegisterUserCommand(request)));

    
    [HttpPost("login")]
    [GenericRequestValidator(typeof(LoginRequestValidation), "Authentication.Application.DTOs.RequestDto.LoginDto")]
    public async Task<IActionResult> Login([FromBody][Bind] LoginDto  request)
        => HandleResponse(await Mediator.Send(new LoginCommand(request)));
}