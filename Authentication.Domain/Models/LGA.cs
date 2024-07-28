// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
namespace Authentication.Domain.Models;

public class LGA
{
    public uint Id { get; set; }
    public string Name { get; set; }
    public uint StateId { get; set; }
    [ForeignKey("StateId")]
    public virtual State State { get; set; } // Each LGA belongs to one State
    
    // Navigation property for the one-to-many relationship with User
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}