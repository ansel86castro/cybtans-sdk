using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cybtans.Tests.Grpc
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;           
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            var response = new HelloReply
            {
                Message = "Hello " + request.Name,
                Info = new HellowInfo 
                {
                   
                },
                InfoArray = { new HellowInfo[] { new HellowInfo() } },
                Keywords = { new[] { "", "" } },
                Date = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.Now),
                Time = Google.Protobuf.WellKnownTypes.Duration.FromTimeSpan(TimeSpan.FromSeconds(1))
            };

            DateTime d = response.Date.ToDateTime();
            TimeSpan t = response.Time.ToTimeSpan();

            var list = response.Keywords.ToList();
            return Task.FromResult(response);
        }
    }
}
