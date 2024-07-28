// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
namespace Authentication.Domain.Models;

public class TwoFAToken
{
    public string Id { get; set; } = DateTime.Now.Ticks.ToString();
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string TwoFATokenType { get; set; }
    public string Token { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow.AddHours(1);
}