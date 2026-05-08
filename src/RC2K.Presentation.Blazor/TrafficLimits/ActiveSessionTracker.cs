using System.Collections.Concurrent;

namespace RC2K.Presentation.Blazor.TrafficLimits;

public class ActiveSessionTracker
{
    private readonly ConcurrentDictionary<string, string> _circuits = new();

    public void RegisterCircuit(string circuitId, string msg)
    {
        _circuits.TryAdd(circuitId, msg);
    }

    public void UnregisterCircuit(string circuitId)
    {
        _circuits.TryRemove(circuitId, out _);
    }

    public int GetActiveCount() => _circuits.Count;

    public IReadOnlyCollection<string> GetActiveCircuitIds()
    {
        return _circuits.Select(kv => $"{kv.Key}{{{kv.Value}}}").ToList();
    }
}
