using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces;
using System.Text;
using System.Text.Json;

namespace RC2K.Function.Messages;

public record DiscordWebhooksConfiguration(string WebhookId, string WebhookToken);

public class SendDiscordMessage(
    ILogger<SendDiscordMessage> logger,
    IMessageService messageService,
    DiscordWebhooksConfiguration webhooksConfiguration)
{
    private readonly string _discordWebhookUrl = 
        $"https://discord.com/api/webhooks/{webhooksConfiguration.WebhookId}/{webhooksConfiguration.WebhookToken}";

    [Function(nameof(SendDiscordMessage_CheckAndStart))]
    public async Task SendDiscordMessage_CheckAndStart(
        [TimerTrigger("0,10,20,30,40,50 * * * * *")] TimerInfo myTimer,
        [DurableClient] DurableTaskClient client)
    {
        string todayIdBase = $"{nameof(SendDiscordMessage_CheckAndStart)}_{DateTime.UtcNow:yyyyMMdd}_0";
        var existingInstance = await client.GetInstanceAsync(todayIdBase);

        if (existingInstance == null ||
            existingInstance.RuntimeStatus == OrchestrationRuntimeStatus.Failed)
        {
            List<DateTimeMessage> todayMessages = 
                (await messageService.GetForToday())
                .Where(x => !x.Published)
                .ToList();

            for (int i=0; i < todayMessages.Count; i++)
            {
                string id = $"{nameof(SendDiscordMessage_CheckAndStart)}_{DateTime.UtcNow:yyyyMMdd}_{i}";
                await client.ScheduleNewOrchestrationInstanceAsync(
                    nameof(SendDiscordMessage_ScheduleSendTime),
                    todayMessages[i],
                    new StartOrchestrationOptions { InstanceId = id });
            }
        }
        else
        {
            logger.LogWarning("Function with id {id} already scheduled", todayIdBase);
        }
    }

    [Function(nameof(SendDiscordMessage_ScheduleSendTime))]
    public async Task SendDiscordMessage_ScheduleSendTime(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        logger.LogInformation("Scheduling send time...");

        var input = context.GetInput<DateTimeMessage>();
        if (input is null)
        {
            logger.LogError("Failed to parse input to DateTimeMessage");
            return;
        }

        DateTime now = context.CurrentUtcDateTime;
        DateTime scheduledTime = input.DateTime;

        if (now < scheduledTime)
        {
            await context.CreateTimer(scheduledTime, CancellationToken.None);
        }

        await context.CallActivityAsync(nameof(SendDiscordMessageActivity), input);
    }

    [Function(nameof(SendDiscordMessageActivity))]
    public async Task SendDiscordMessageActivity([ActivityTrigger] object input)
    {
        logger.LogInformation("Sending to Discord given object {input}", input);

        if (!TryParseInput(input, out DateTimeMessage? msg))
        {
            return;
        }

        if (!TryParseDateTimeMessage(msg!, out List<DiscordWebhookPayload>? payloads))
        {
            return;
        }

        HttpClient httpClient = new();
        foreach (var payload in payloads!)
        {
            var jsonPayload = JsonSerializer.Serialize(payload);
            using var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClient.PostAsync(_discordWebhookUrl, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send discord message {message}", jsonPayload);
                continue;
            }

            // Small delay to ensure correct sequential delivery in Discord
            await Task.Delay(500);
        }

        msg!.Published = true;
        await messageService.Update(msg);
    }

    private bool TryParseInput(object input, out DateTimeMessage? msg)
    {
        try
        {
            msg = ((JsonElement)input).Deserialize<DateTimeMessage>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while deserializing message");
            msg = null;
            return false;
        }
        if (msg is null)
        {
            logger.LogError("Failed to deserialize message");
            return false;
        }

        return true;
    }

    private bool TryParseDateTimeMessage(DateTimeMessage msg, out List<DiscordWebhookPayload>? payloads)
    {
        byte[] jsonBytes = Convert.FromBase64String(msg!.Value);
        string jsonString = Encoding.UTF8.GetString(jsonBytes);

        try
        {
            payloads = JsonSerializer.Deserialize<List<DiscordWebhookPayload>>(jsonString);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to deserialize json: {jsonString}", jsonString);
            payloads = null;
            return false;
        }
        if (payloads is null)
        {
            logger.LogError("Payload is null after deserialization json: {jsonString}", jsonString);
            return false;
        }

        return true;
    }
}