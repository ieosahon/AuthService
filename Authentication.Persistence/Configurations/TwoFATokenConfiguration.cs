namespace Authentication.Persistence.Configurations;
public class TwoFATokenConfiguration : IEntityTypeConfiguration<TwoFAToken>
{
    public void Configure(EntityTypeBuilder<TwoFAToken> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Token)
            .IsRequired()
            .HasMaxLength(5);

        builder.Property(t => t.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(t => t.TwoFATokenType) 
            .IsRequired()
            .HasMaxLength(50);

        // Ensure uniqueness of combination of Token, Email, PhoneNumber, and TwoFATokenType
        builder.HasIndex(t => new { t.Token, t.Email, t.PhoneNumber, t.TwoFATokenType })
            .IsUnique(); // Composite unique index
    }
}
