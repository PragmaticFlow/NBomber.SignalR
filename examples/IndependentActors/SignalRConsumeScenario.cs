using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.SignalR;

namespace IndependentActors;

public class SignalRConsumeScenario
{
    public ScenarioProps Create()
    {
        SignalRConnection signalRConnection = null;

        return Scenario.Create("consume_scenario", async ctx =>
        {
            return await signalRConnection.Receive(ctx.ScenarioCancellationToken);
        })
        .WithoutWarmUp()
        .WithLoadSimulations(
            Simulation.KeepConstant(1, TimeSpan.FromSeconds(30))
        )
        .WithInit(async ctx =>
        {
            var config = ctx.GlobalCustomSettings.Get<SignalRCustomSettings>();

            var connection = new HubConnectionBuilder()
                .WithUrl(config.SignalRServerUrl)
                //.AddMessagePackProtocol()
                .Build();

            signalRConnection = new SignalRConnection(connection, HubProtocolFormat.Json); // Ensure the client uses the same protocol as the server

            signalRConnection.Connection.On("ServerPush", (Message<string> message) =>
            {
                var responseSize = signalRConnection.CalculateMessageSize("ServerPush", message);

                var timestampMs = long.Parse(message.Headers["timestamp"]);
                var latency = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - timestampMs;

                var response = Response.Ok<object>(message.Payload, customLatencyMs: latency, sizeBytes: responseSize);
                signalRConnection.Write(response);
            });

            await signalRConnection.Start();
        })
        .WithClean(async ctx =>
        {
            await signalRConnection.Stop();
        });
    }
}
