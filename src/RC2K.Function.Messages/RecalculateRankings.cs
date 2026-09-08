using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RC2K.DomainModel.Exceptions;

namespace RC2K.Function.Messages;

public class Rc2kHubRankingsHttpClient(
    HttpClient httpClient,
    ILogger<Rc2kHubRankingsHttpClient> logger)
{
    public async Task SendPostRankingAutoSnapshotMessage(string credentials)
    {
        using HttpRequestMessage request = new(HttpMethod.Post, "ranking/auto-snapshot");
        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {credentials}");
        request.Content = null;

        try
        {
            using var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }
    }
}

public class RecalculateRankings(
    IConfiguration configuration,
    Rc2kHubRankingsHttpClient rc2khubRankingsHttpClient)
{
    /// <summary>
    /// Triggered every Monday at 6:00 UTC (8:00 CEST, 7:00 CET - pl time)
    /// </summary>
    [Function(nameof(RecalculateRankings_Job))]
    public async Task RecalculateRankings_Job(
        [TimerTrigger("0 0 6 * * 1")] TimerInfo myTimer)
    {
        var appClientSection = configuration.GetSection("AppClient");
        string credentials = appClientSection["AzureFunctionsBotCredentials"]
            ?? throw new MissingConfigurationKeyException("AppClient:AzureFunctionsBotCredentials");

        await rc2khubRankingsHttpClient.SendPostRankingAutoSnapshotMessage(credentials);
    }
}