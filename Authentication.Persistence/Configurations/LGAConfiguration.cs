namespace Authentication.Persistence.Configurations;

public class LGAConfiguration : IEntityTypeConfiguration<LGA>
{
    public void Configure(EntityTypeBuilder<LGA> builder)
    {
        builder.HasKey(l => l.Id); 

        builder.Property(l => l.Id)
            .ValueGeneratedOnAdd();

        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(l => l.State)
            .WithMany(s => s.Lga)
            .HasForeignKey(l => l.StateId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}
