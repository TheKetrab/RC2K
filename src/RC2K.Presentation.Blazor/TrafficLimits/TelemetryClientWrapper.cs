using Microsoft.ApplicationInsights;

namespace RC2K.Presentation.Blazor.TrafficLimits;

public interface ITelemetryClientWrapper
{
    void TrackMetric(string name, double value);
}

public class TelemetryClientWrapper(TelemetryClient telemetryClient) : ITelemetryClientWrapper
{
    public void TrackMetric(string name, double value) => telemetryClient.TrackMetric(name, value);
}

public class TelemetryClientNullObjectWrapper() : ITelemetryClientWrapper
{
    public void TrackMetric(string name, double value) { }
}