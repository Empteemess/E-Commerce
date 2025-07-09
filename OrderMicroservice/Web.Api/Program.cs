using DotNetEnv;
using Web.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services
    .AddRabbitMq(builder.Configuration)
    .AddDbConfigurations(builder.Configuration)
    .AddServiceConfigs();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();