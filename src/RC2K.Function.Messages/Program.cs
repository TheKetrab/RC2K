using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.DurableTask.Worker;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Azure Functions for Messages starting...");

var builder = FunctionsApplication.CreateBuilder(args);
builder.Services.AddDurableTaskWorker();

var host = builder.Build();
Console.WriteLine("Azure Functions host for Messages runs.");

await host.RunAsync();
