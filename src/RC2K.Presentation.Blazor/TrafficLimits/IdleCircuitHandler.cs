namespace RC2K.Presentation.Blazor.TrafficLimits;

using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.Options;
using Timer = System.Timers.Timer;

public sealed class IdleCircuitHandler : CircuitHandler, IDisposable
{
    private Circuit? _currentCircuit;
    private readonly ILogger _logger;
    private readonly Timer _timerIdleInfo;
    private readonly Timer _timerForceInvalidate;

    private readonly ActiveSessionTracker _tracker;

    public IdleCircuitHandler(
        ILogger<IdleCircuitHandler> logger,
        IOptions<IdleCircuitOptions> options,
        ActiveSessionTracker tracker)
    {
        _timerIdleInfo = new Timer
        {
            Interval = options.Value.IdleTimeout.TotalMilliseconds,
            AutoReset = false
        };

        _timerForceInvalidate = new Timer
        {
            Interval = options.Value.ForceInvalidateCircuitTimeout.TotalMilliseconds,
            AutoReset = false
        };

        _timerIdleInfo.Elapsed += CircuitIdle;
        _timerForceInvalidate.Elapsed += ForceInvalidateCircuit;
        _logger = logger;
        _tracker = tracker;
    }

    private void CircuitIdle(object? sender, System.Timers.ElapsedEventArgs e) =>
        _logger.LogWarning("Circuit {CircuitId} is idle", _currentCircuit?.Id);

    private void ForceInvalidateCircuit(object? sender, System.Timers.ElapsedEventArgs e)
    {
        _logger.LogWarning("Circuit {CircuitId} was hanging for too long time. Forcing invalidation of this circuit forever.", _currentCircuit?.Id);
        if (_currentCircuit is not null)
        {
            _tracker.UnregisterCircuit(_currentCircuit.Id);
        }
    }

    public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        _currentCircuit = circuit;
        return Task.CompletedTask;
    }

    public override Func<CircuitInboundActivityContext, Task> CreateInboundActivityHandler(
        Func<CircuitInboundActivityContext, Task> next)
    {
        return context =>
        {
            // on circuit action: refresh timers

            _timerIdleInfo.Stop();
            _timerIdleInfo.Start();

            _timerForceInvalidate.Stop();
            _timerForceInvalidate.Start();

            return next(context);
        };
    }

    public void Dispose()
    {
        _timerIdleInfo.Dispose();
        _timerForceInvalidate.Dispose();
    }
}

public class IdleCircuitOptions
{
    public TimeSpan IdleTimeout { get; set; } = TimeSpan.FromMinutes(5);
    public TimeSpan ForceInvalidateCircuitTimeout { get; set; } = TimeSpan.FromMinutes(15);
}

public static class IdleCircuitHandlerServiceCollectionExtensions
{
    public static IServiceCollection AddIdleCircuitHandler(
        this IServiceCollection services,
        Action<IdleCircuitOptions> configureOptions)
    {
        services.Configure(configureOptions);
        services.AddIdleCircuitHandler();

        return services;
    }

    public static IServiceCollection AddIdleCircuitHandler(
        this IServiceCollection services)
    {
        services.AddScoped<CircuitHandler, IdleCircuitHandler>();

        return services;
    }
}