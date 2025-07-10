using Application.BackgroundServices;
using Domain.IRepository;
using Infrastructure.Repository;

namespace Web.Api.Configurations;

public static class ServiceConfigs
{
    public static IServiceCollection AddServiceConfigs(this IServiceCollection service)
    {
        service.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();

        service.AddScoped<IUnitOfWork,UnitOfWork>();

        service.AddHostedService<OutboxMessageBackgroundService>();

        return service;
    }
}