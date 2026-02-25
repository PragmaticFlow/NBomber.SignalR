using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.SignalR;

namespace IndependentActors;

public class SignalRPublishScenario
{
    public ScenarioProps Create()
    {
        SignalRConnection signalRConnection = null;

        return Scenario.Create("publish_scenario", async ctx =>
        {
            var publish = await Step.Run("publish", ctx, async () =>
            {
                // We include the current timestamp so that the consumer can calculate the final latency.
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                // Invoke directly on signalRConnection, so that NBomber can track the execution time and handle any exceptions
                var message = new Message<string>("UserName", new Dictionary<string, string> { ["timestamp"] = timestamp.ToString() });
                return await signalRConnection.Invoke("SendIndependentActorsMessage", message);
            });

            return Response.Ok();
        })
        .WithInit(async ctx =>
        {
            var config = ctx.GlobalCustomSettings.Get<SignalRCustomSettings>();

            var connection = new HubConnectionBuilder()
                .WithUrl(config.SignalRServerUrl)
                //.AddMessagePackProtocol()
                .Build();

            signalRConnection = new SignalRConnection(connection, HubProtocolFormat.Json); // Ensure the client uses the same protocol as the server

            await signalRConnection.Start();
        })
        .WithClean(async ctx =>
        {
            await signalRConnection.Stop();
        });
    }
}
