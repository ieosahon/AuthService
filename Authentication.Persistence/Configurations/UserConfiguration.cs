namespace Authentication.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();

        builder.Property(u => u.MiddleName)
            .IsRequired(false);
        builder.Property(u => u.FirstName)
            .IsRequired();
        builder.Property(u => u.LastName)
            .IsRequired();
        
        builder.Property(u => u.NIN)
            .IsRequired()
            .HasMaxLength(11);
        
        builder.Property(u => u.BVN)
            .IsRequired()
            .HasMaxLength(11);
        
        builder.Property(u => u.HasBVN)
            .IsRequired();
        
        builder.Property(u => u.AccountType)
            .IsRequired();
        
        builder.Property(u => u.Gender)
            .IsRequired(); 
        
        builder.Property(u => u.Title)
            .IsRequired()
            .HasMaxLength(50); 
        
        builder.Property(u => u.Age)
            .IsRequired()
            .HasMaxLength(3); 
        
        builder.Property(u => u.DOB)
            .IsRequired(); 
        
        builder.Property(u => u.Gender)
            .IsRequired()
            .HasMaxLength(20); 
        
        builder.Property(u => u.Address)
            .IsRequired()
            .HasMaxLength(20); 
        
        builder.Property(u => u.AccountNumber)
            .IsRequired()
            .HasMaxLength(30);
        
        builder.Property(u => u.LandMark)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(10);
        builder.Property(u => u.Status)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(u => u.FailedLoginCount)
            .IsRequired()
            .HasMaxLength(2);
        
        builder.Property(u => u.LastLogin);

        builder.Property(u => u.CreatedAt);
        builder.Property(u => u.UpdatedAt);
        builder.Property(u => u.LgaId)
            .IsRequired();

    }
}
