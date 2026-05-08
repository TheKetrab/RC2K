namespace RC2K.Presentation.Blazor.TrafficLimits;

public class SessionLoggingService : BackgroundService
{
    private string _prev = string.Empty;
    private readonly ILogger<SessionLoggingService> _logger;
    private readonly ActiveSessionTracker _tracker;
    private readonly ITelemetryClientWrapper _telemetryClient;

    public SessionLoggingService(ILogger<SessionLoggingService> logger, ActiveSessionTracker tracker, ITelemetryClientWrapper telemetryClient)
    {
        _logger = logger;
        _tracker = tracker;
        _telemetryClient = telemetryClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var count = _tracker.GetActiveCount();
            var ids = _tracker.GetActiveCircuitIds();

            string msg = $"Active Blazor Interactive Server sessions: {count} | Circuit: {string.Join(", ", ids)}";

            if (_prev != msg)
            {
                _logger.LogInformation(msg);
                _prev = msg;
            }

            _telemetryClient.TrackMetric("ActiveCircuits", count);
            await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
        }
    }
}

