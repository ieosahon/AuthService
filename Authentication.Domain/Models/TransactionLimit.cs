// ReSharper disable PropertyCanBeMadeInitOnly.Global
namespace Authentication.Domain.Models;

public class TransactionLimit
{
    public string Id { get; set; } = DateTime.Now.Ticks.ToString();
    public string AccountNumber { get; set; }
    public decimal DailyLimit { get; set; }
}