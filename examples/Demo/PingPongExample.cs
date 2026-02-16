using Microsoft.AspNetCore.SignalR.Client;
using NBomber.CSharp;
using NBomber.SignalR;

new PingPongExample().Run();

public class PingPongExample
{
    public void Run()
    {
        var scenario = Scenario.Create("signalr_scenario", async ctx =>
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5077/chathub")
                .Build();

            var signalRConnection = new SignalRConnection(connection);

            signalRConnection.Connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var responseSize = SignalRConnection.CalculateServerInvocationSize("ReceiveMessage", user, message);
                var response = Response.Ok<object>(new { user, message }, sizeBytes: responseSize);
                signalRConnection.Write(response);
            });

            var connect = await Step.Run("connect", ctx, async () =>
            {
                await signalRConnection.Connection.StartAsync();
                return Response.Ok();
            });

            var send = await Step.Run("send_message", ctx, async () =>
            {
                // Invoke directly on signalRConnection, so that NBomber can track the execution time and handle any exceptions
                await signalRConnection.InvokeAsync("SendMessage", "test_username", "test_message");
                return Response.Ok();
            });

            var receive = await Step.Run("receive_message", ctx, async () => await signalRConnection.Receive());

            var disconnect = await Step.Run("disconnect", ctx, async () => {
                await signalRConnection.Connection.StopAsync();
                return Response.Ok();
            });

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
