using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cybtans.Testing.Integration
{
    public class ContainerConfig
    {
        public string Image { get; set; }

        public string NamePrefix { get; set; }

        public int ContainerPort { get; set; }

        public int HostPort { get; set; }

        public List<string> Environment { get; set; } = new List<string>();

        public Func<ContainerInfo, Task> WaitFunction { get; set; } 
    }
}
