
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
namespace Authentication.Domain.Models;

public sealed class User : IdentityUser<string>
{
    public override string  Id { get; set; } = DateTime.Now.Ticks.ToString();
    public override string PasswordHash { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string NIN { get; set; }
    public string BVN { get; set; }
    public string AccountType { get; set; } = AccountTypeEnum.Savings.ToString();
    public string Gender { get; set; }
    public bool HasBVN { get; set; }
    public string Title { get; set; }
    public int Age { get; set; }
    public DateTime DOB { get; set; }
    public string Address { get; set; }
    public string LandMark { get; set; }
    public string AccountNumber { get; set; }
    public string Status { get; set; }
    public uint FailedLoginCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(1);
    public DateTime LastLogin { get; set; }
    public uint LgaId { get; set; }
    
}
