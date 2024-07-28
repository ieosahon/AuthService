// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Authentication.Application.DTOs.RequestDto;

public class LoginDto
{
    public string AccountNumber { get; set; }
    public string Password { get; set; }
}