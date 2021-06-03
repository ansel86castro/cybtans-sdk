using Cybtans.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cybtans.Tests.Grpc.Models
{
    [GenerateMessage]
    public class HelloModel
    {
        public Guid Id { get; set; }

        public string Message { get; set; }
    }
}
