using MassTransit;

namespace Web.Api.Configurations;

public static class RabbitMqConfig
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(opt =>
        {

            opt.AddConsumers(typeof(RabbitMqConfig).Assembly);

            opt.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(
                    new Uri(configuration["RABBITMQ_HOST"]!), "/",
                    host =>
                    {
                        host.Username(configuration["RABBITMQ_DEFAULT_USER"]!);
                        host.Password(configuration["RABBITMQ_DEFAULT_PASS"]!);
                    });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}