// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Authentication.Application.DTOs.RequestDto;

public class RegistrationRequest
{
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string NIN { get; set; }
    public string BVN { get; set; }
    public string AccountType { get; set; } = AccountTypeEnum.Savings.ToString();
    public string Gender { get; set; }
    public bool HasBVN { get; set; }
    public string Title { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public DateTime DOB { get; set; }
    public string Address { get; set; }
    public uint LGAId { get; set; }
    public string LandMark { get; set; }
}
