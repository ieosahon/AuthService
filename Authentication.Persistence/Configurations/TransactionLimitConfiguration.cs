namespace Authentication.Persistence.Configurations;

public class TransactionLimitConfiguration : IEntityTypeConfiguration<TransactionLimit>
{
    public void Configure(EntityTypeBuilder<TransactionLimit> builder)
    {
        builder.HasKey(t => t.Id); 
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.AccountNumber)
            .IsRequired();

        builder.Property(t => t.DailyLimit)
            .HasColumnType("decimal(18,2)"); 
    }
}
