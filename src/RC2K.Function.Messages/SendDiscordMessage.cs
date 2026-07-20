using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;
namespace RC2K.Function.Messages;

public class SendDiscordMessage(ILogger<SendDiscordMessage> logger)
{
    [Function(nameof(SendDiscordMessage_CheckAndStart))]
    public async Task SendDiscordMessage_CheckAndStart(
        [TimerTrigger("0,10,20,30,40,50 * * * * *")] TimerInfo myTimer,
        [DurableClient] DurableTaskClient client)
    {
        string todayId = $"{nameof(SendDiscordMessage_CheckAndStart)}_{DateTime.UtcNow:yyyyMMdd}";
        var existingInstance = await client.GetInstanceAsync(todayId);

        if (existingInstance == null ||
            existingInstance.RuntimeStatus == OrchestrationRuntimeStatus.Completed ||
            existingInstance.RuntimeStatus == OrchestrationRuntimeStatus.Failed)
        {
            await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(SendDiscordMessage_ScheduleSendTime),
                new StartOrchestrationOptions { InstanceId = todayId });
        }
        else
        {
            logger.LogWarning("Function with id {id} already scheduled", todayId);
        }
    }

    [Function(nameof(SendDiscordMessage_ScheduleSendTime))]
    public async Task SendDiscordMessage_ScheduleSendTime(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        logger.LogInformation("Scheduling send time...");
        DateTime now = context.CurrentUtcDateTime;
        DateTime scheduledTime = now + TimeSpan.FromSeconds(30);

        if (now < scheduledTime)
        {
            await context.CreateTimer(scheduledTime, CancellationToken.None);
        }

        await context.CallActivityAsync(nameof(SendDiscordMessageActivity));
    }


    [Function(nameof(SendDiscordMessageActivity))]
    public void SendDiscordMessageActivity([ActivityTrigger] object input)
    {
        logger.LogInformation(">>> SENDING TO DISCORD <<<");
    }

}