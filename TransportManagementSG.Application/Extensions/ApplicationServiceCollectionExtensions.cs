using Microsoft.Extensions.DependencyInjection;
using TransportManagementSG.Application.Database;
using TransportManagementSG.Application.Interfaces.Repository;
using TransportManagementSG.Application.Repository;
using TransportManagementSG.Application.Services;

namespace TransportManagementSG.Application.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
        services.AddSingleton<DBInitializer>();
        return services;
    }
    
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRoleService, RoleService>();
        return services;
    }
}