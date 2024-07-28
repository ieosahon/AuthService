namespace Authentication.Persistence.Configurations;

public class StateConfiguration : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        builder.HasKey(s => s.Id); 

        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();

        builder.HasMany(s => s.Lga)
            .WithOne(l => l.State)
            .HasForeignKey(l => l.StateId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
