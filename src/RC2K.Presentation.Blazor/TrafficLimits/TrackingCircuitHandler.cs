namespace RC2K.Presentation.Blazor.TrafficLimits;

using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Http;

public class TrackingCircuitHandler : CircuitHandler
{
    private readonly ActiveSessionTracker _tracker;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TrackingCircuitHandler(
        ActiveSessionTracker tracker,
        IHttpContextAccessor httpContextAccessor)
    {
        _tracker = tracker;
        _httpContextAccessor = httpContextAccessor;
    }

    public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        string msg = "";
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            var remoteIp = httpContext.Connection.RemoteIpAddress?.ToString();
            if (httpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            {
                remoteIp = forwardedFor.ToString().Split(',')[0].Trim();
            }

            string method = httpContext.Request.Method;
            string protocol = httpContext.Request.Protocol;
            string path = httpContext.Request.Path;
            string host = httpContext.Request.Host.Value ?? "";
            string user = "";
            if (httpContext.Request.Headers.TryGetValue("User-Agent", out var userAgent))
            {
                user = userAgent.ToString();
            }

            msg = $"M={method},P={protocol},R={host}{path},U={{{user}}},IP={remoteIp}";
        }

        _tracker.RegisterCircuit(circuit.Id, msg);
        return base.OnCircuitOpenedAsync(circuit, cancellationToken);
    }

    public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken) =>
        UnregisterCircuit(circuit);

    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken) =>
        UnregisterCircuit(circuit);

    private Task UnregisterCircuit(Circuit circuit)
    {
        _tracker.UnregisterCircuit(circuit.Id);
        return Task.CompletedTask;
    }
}
