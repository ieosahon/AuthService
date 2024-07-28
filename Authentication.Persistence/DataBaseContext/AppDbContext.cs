namespace Authentication.Persistence.DataBaseContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {
        
    }
    
    public DbSet<LGA> LGAs { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<TransactionLimit> TransactionLimits { get; set; }
    public DbSet<TwoFAToken> TwoFATokens { get; set; }
    public DbSet<User> Users { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new LGAConfiguration());
        modelBuilder.ApplyConfiguration(new StateConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionLimitConfiguration());
        modelBuilder.ApplyConfiguration(new TwoFATokenConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}