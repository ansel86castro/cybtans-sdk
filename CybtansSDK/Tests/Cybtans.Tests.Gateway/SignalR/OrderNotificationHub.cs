using Cybtans.Tests.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public interface IOrderNotificationClient
    {
        Task ReceiveMessage(string msg);
    }

    public class OrderNotificationHub : Hub<IOrderNotificationClient>
    {
        public async Task SendMessage(OrderNotification msg)
        {                        
            await Clients.All.ReceiveMessage(msg.Msg);
        }
    }
}