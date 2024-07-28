// ReSharper disable ClassNeverInstantiated.Global
namespace Authentication.Domain.Models;

public class State
{
    public uint Id { get; set; }
    public string Name { get; set; }
    // Navigation property for one-to-many relationship
    public virtual ICollection<LGA> Lga { get; set; } // One State can have many LGAs
}