using DotNetEnv;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Web.Api.Configurations;
using Web.Api.Entities;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddEnvironmentVariables();

builder.Services
    .AddRabbitMq(builder.Configuration)
    .AddDbConfigurations(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();