// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Authentication.Application.DTOs.ResponseDto;

public class LoginResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string NIN { get; set; }
    public string BVN { get; set; }
    public string AccountType { get; set; }
    public string Gender { get; set; }
    public string Title { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string LgaName { get; set; }
    public string StateName { get; set; }
    public string LandMark { get; set; }
    public string Token { get; set; }
    public string AccountNumber { get; set; }
    public string Status { get; set; }
    public DateTime LastLogin { get; set; }
}