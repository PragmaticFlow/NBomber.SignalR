using Microsoft.AspNetCore.SignalR;

namespace SignalR.Server;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.Caller.SendAsync("ReceiveMessage", user, "Message sent!");
        await Clients.Others.SendAsync("ReceiveMessage", user, message);
    }
}