using NBomber.CSharp;

new IndependentActors.IndependentActors().Run();

namespace IndependentActors
{
    public class SignalRCustomSettings
    {
        public string SignalRServerUrl { get; set; }
    }

    public record Message<T>(T Payload, Dictionary<string, string> Headers);

    public class IndependentActors
    {
        public void Run()
        {
            NBomberRunner.RegisterScenarios(
                new SignalRPublishScenario().Create(),
                new SignalRConsumeScenario().Create()
            )
            .LoadConfig("config.json")
            .Run();
        }
    }
}
