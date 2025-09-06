using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmiTo.Domain.Repositories;
using SmiTo.Infrastructure.Persistence;
using SmiTo.Infrastructure.Persistence.Context;
using SmiTo.Infrastructure.Persistence.Repositories;

namespace SmiTo.Infrastructure.DependancyInjection;

public static class InfrastructureDependancyInjectionSetUp
{
    public static void AddInfrastructure(this IServiceCollection services,
    IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<SmiToDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        services.AddScoped<IVisitRepository, VisitRepository>();
        services.AddScoped<IURLRepository, URLRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
