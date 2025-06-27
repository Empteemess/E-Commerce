using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Web.Api.Configurations;

public static class DbConfigurations
{
    public static IServiceCollection AddDbConfigurations(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(configuration["SQL_CONNECTION_STRING"]));

        return services;
    }
}