using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces;
using System.Text;
using System.Text.Json;

namespace RC2K.Function.Messages;

public class RecalculateRankings(
    ILogger<SendDiscordMessage> logger)
{
    /// <summary>
    /// Triggered every Monday at 6:00 UTC (8:00 CEST, 7:00 CET - pl time)
    /// </summary>
    [Function(nameof(RecalculateRankings_Job))]
    public Task RecalculateRankings_Job(
        [TimerTrigger("0 0 6 * * 1")] TimerInfo myTimer)
    {
        // TODO
        return Task.CompletedTask;
    }
}