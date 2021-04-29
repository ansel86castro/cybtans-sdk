using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Formatters;
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
            _logger.LogInformation("called SayHellow ({Request})", request);

            var response = new HelloReply
            {
                Message = "Hello " + request.Name,
                Info = new HellowInfo
                {
                    Id = 1,
                    InnerA = new HellowInfo.Types.InnerA
                    {
                        B = new HellowInfo.Types.InnerA.Types.InnerB
                        {
                            Type = HellowInfo.Types.InnerA.Types.InnerB.Types.TypeB.B
                        }
                    },
                    Name = "Say Hellow",
                    Type = HellowInfo.Types.TypeInfo.B
                },
                InfoArray = { new HellowInfo[] { new HellowInfo() } },
                Keywords = { new[] { "", "" } },
                Date = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow),
                Time = Google.Protobuf.WellKnownTypes.Duration.FromTimeSpan(TimeSpan.Parse("01:05:00")),                 
            };
            
            return Task.FromResult(response);
        }
    }
}
