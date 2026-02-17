using Microsoft.AspNetCore.SignalR;

namespace SignalR.Server;

public record Message<T>(T Payload, Dictionary<string, string> Headers);

public class ChatHub : Hub
{
    public async Task<string> SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
        return "Message sent!";
    }

    public async Task SendIndependentActorsMessage(Message<string> message)
    {
        await Clients.Others.SendAsync("ServerPush", message);
    }
}