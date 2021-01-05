using Cybtans.Messaging;
using Cybtans.Tests.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRChat.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cybtans.Tests.Gateway
{
    public class OrderNotificationHandler : IMessageHandler<OrderNotification>
    {
        private readonly ILogger<OrderNotificationHandler> _logger;
        private readonly IHubContext<OrderNotificationHub, IOrderNotificationClient> _hubContext;

        public OrderNotificationHandler(ILogger<OrderNotificationHandler> logger,
            IHubContext<OrderNotificationHub, IOrderNotificationClient> hubContext)
        {
            this._logger = logger;
            this._hubContext = hubContext;
        }

        public async Task HandleMessage(OrderNotification message)
        {
            _logger.LogInformation("Message Received Order:{Order}, User:{User}, Msg:{Msg}", message.OrderId, message.UserId, message.Msg);

            await _hubContext.Clients.All.ReceiveMessage(message.Msg);
        }
    }
}
