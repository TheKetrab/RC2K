using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.DurableTask.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RC2K.DataAccess.Dynamic;
using RC2K.DataAccess.Dynamic.EnvironmentProviders;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Repositories;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel.Exceptions;
using RC2K.Function.Messages;
using RC2K.Logic;
using RC2K.Logic.Interfaces;

Console.WriteLine("Azure Functions for Messages starting...");

var builder = FunctionsApplication.CreateBuilder(args);
builder.Services.AddDurableTaskWorker();


builder.Services.RegisterCosmos(builder.Configuration,
    builder.Configuration["AZURE_FUNCTIONS_ENVIRONMENT"] switch
    {
        "Development" => typeof(DevEnvironmentProvider),
        "Production" => typeof(ProdEnvironmentProvider),
        _ => throw new ArgumentException(
            $"Unknown environment: {builder.Configuration["AZURE_FUNCTIONS_ENVIRONMENT"]}"
            + " (Set up proper env var AZURE_FUNCTIONS_ENVIRONMENT")
    });
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<DateTimeMessageMapper>();

builder.Services.AddScoped<DiscordWebhooksConfiguration>(sp =>
{
    var discordSection = builder.Configuration.GetSection("Discord");
    string webhookId = discordSection["WebhookId"]
        ?? throw new MissingConfigurationKeyException("Discord:WebhookId");
    string webhookToken = discordSection["WebhookToken"]
        ?? throw new MissingConfigurationKeyException("Discord:WebhookToken");

    return new DiscordWebhooksConfiguration(webhookId, webhookToken);
});

var host = builder.Build();
Console.WriteLine("Azure Functions host for Messages runs.");

await host.RunAsync();
