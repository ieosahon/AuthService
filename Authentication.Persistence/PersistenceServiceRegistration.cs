// ReSharper disable AssignNullToNotNullAttribute

#pragma warning disable CS8604 // Possible null reference argument.

namespace Authentication.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration config)
    {
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DbConnection")));
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDbQuery, DbQuery>();
        return services;
    }
}