using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using NBomber.CSharp;
using NBomber.SignalR;

new PingPongExample().Run();

public record ChatMessage(string User, string Message);

public class PingPongExample
{
    public void Run()
    {
        var scenario = Scenario.Create("signalr_scenario", async ctx =>
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5077/chathub")
                //.AddMessagePackProtocol()
                .Build();

            var signalRConnection = new SignalRConnection(connection, HubProtocolFormat.Json); // Ensure the client uses the same protocol as the server

            // Register handler on Connection object explicitly to receive server-to-client messages
            signalRConnection.Connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var responseSize = signalRConnection.CalculateMessageSize("ReceiveMessage", user, message);
                var response = Response.Ok<object>(new ChatMessage(user, message), sizeBytes: responseSize);
                signalRConnection.Write(response);
            });

            var connect = await Step.Run("start", ctx, async () => await signalRConnection.Start());

            var send = await Step.Run("send_message", ctx, async () =>
            {
                var response = await signalRConnection.Invoke<string>("SendMessage", "test_username", "test_message");
                return response;
            });

            var receive = await Step.Run("receive_message", ctx, async () => await signalRConnection.Receive(ctx.ScenarioCancellationToken));

            var disconnect = await Step.Run("stop", ctx, async () => await signalRConnection.Stop());

            return Response.Ok();
        })
        .WithoutWarmUp()
        .WithLoadSimulations(
            Simulation.KeepConstant(1, TimeSpan.FromSeconds(30))
        );

        NBomberRunner
            .RegisterScenarios(scenario)
            .Run();
    }
}
