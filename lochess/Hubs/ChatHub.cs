using Microsoft.AspNetCore.SignalR;

namespace lochess.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMove(string fenString)
        {
            await Clients.All.SendAsync("ReceiveMove", fenString);
        }
    }
}