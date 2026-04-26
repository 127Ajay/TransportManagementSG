using Microsoft.Extensions.DependencyInjection;
using TransportManagementSG.Application.Database;

namespace TransportManagementSG.Application.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
        services.AddSingleton<DBInitializer>();
        return services;
    }
}