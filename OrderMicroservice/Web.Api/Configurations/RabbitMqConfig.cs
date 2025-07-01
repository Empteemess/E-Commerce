using Domain.Entities;
using Infrastructure.Data;
using MassTransit;
using Web.Api.Saga;

namespace Web.Api.Configurations;

public static class RabbitMqConfig
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(opt =>
        {
            opt.AddSagaStateMachine<UserSaga, UserSagaData>()
                .EntityFrameworkRepository(r =>
                {
                    r.UseSqlServer();

                    r.ExistingDbContext<AppDbContext>();
                });

            opt.AddConsumers(typeof(Program).Assembly);

            opt.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(
                    new Uri(configuration["RABBITMQ_HOST"]!), "/",
                    host =>
                    {
                        host.Username(configuration["RABBITMQ_DEFAULT_USER"]!);
                        host.Password(configuration["RABBITMQ_DEFAULT_PASS"]!);
                    });

                cfg.UseInMemoryOutbox(context);

                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}