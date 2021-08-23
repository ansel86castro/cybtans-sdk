using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Cybtans.Testing.Integration
{

    public class ContainerInfo :IAsyncDisposable
    {
        private DockerClient _dockerClient;
        public ContainerInfo(DockerClient dockerClient, string id, int port, string name)
        {
            Id = id;
            HostPort = port;
            _dockerClient = dockerClient;
        }

        public string Id { get; }

        public string Name { get; }

        public int HostPort { get; }

        public int ContainerPort { get; internal set; }

        public string IPAddress { get; internal set; }


        public async ValueTask DisposeAsync()
        {
            await _dockerClient.Containers.StopContainerAsync(Id, new ContainerStopParameters());
            await _dockerClient.Containers.RemoveContainerAsync(Id, new ContainerRemoveParameters());
        }
    }

    public class DockerManager
    {
        DockerClient _dockerClient;

        public DockerManager()
        {
            _dockerClient = GetDockerClient();
        }

        public async Task<ContainerInfo> RunContainerAsync(ContainerConfig config)
        {
            await CleanupRunningContainers(config);
          
           var port = config.HostPort == 0 ? GetFreePort() : config.HostPort;                                    

            await _dockerClient.Images.CreateImageAsync(new ImagesCreateParameters
            {
                FromImage = config.Image,               
            }, null, new Progress<JSONMessage>(log=>
            {
                if (log.Status != null)
                {
                    Trace.WriteLine(log.Status);
                }
                Trace.Write(log.ProgressMessage);
            }));

            var name = config.NamePrefix + Guid.NewGuid().ToString("N");
            var container = await _dockerClient
              .Containers
              .CreateContainerAsync(new CreateContainerParameters
              {
                  Name = name,
                  Image = config.Image,
                  Env = config.Environment,                                
                  HostConfig = new HostConfig
                  {
                      PortBindings = new Dictionary<string, IList<PortBinding>>
                      {
                                {
                                    $"{config.ContainerPort}/tcp",
                                    new PortBinding[]
                                    {
                                        new PortBinding
                                        {
                                            HostPort = port.ToString()
                                        }
                                    }
                                }
                      }
                  },                  
              });

            var info = new ContainerInfo(_dockerClient, container.ID, port, name) { ContainerPort = config.ContainerPort };

            if (!await _dockerClient.Containers.StartContainerAsync(info.Id, new ContainerStartParameters()))
            {
                throw new InvalidOperationException($"Unable to start container for image {config.Image}");
            }

            await Task.Delay(500);

            var inspectResponse = await _dockerClient.Containers.InspectContainerAsync(container.ID);
            info.IPAddress = inspectResponse.NetworkSettings.IPAddress;

            if(config.WaitFunction != null)
            {
                try
                {
                    await config.WaitFunction(info);
                }
                catch
                {
                    await RemoveContainer(info.Id);
                    throw;
                }
            }

            return info;
        }

        public async Task RemoveContainer(string dockerContainerId)
        {
            try
            {
                if (await _dockerClient.Containers.StopContainerAsync(dockerContainerId, new ContainerStopParameters()))
                {
                    await _dockerClient.Containers.RemoveContainerAsync(dockerContainerId, new ContainerRemoveParameters());
                }
            }
            catch (Docker.DotNet.DockerContainerNotFoundException)
            {

            }
        }

        private static DockerClient GetDockerClient()
        {
            var dockerUri = IsRunningOnWindows()
                ? "npipe://./pipe/docker_engine"
                : "unix:///var/run/docker.sock";
            return new DockerClientConfiguration(new Uri(dockerUri))
                .CreateClient();
        }

        private async Task CleanupRunningContainers(ContainerConfig config)
        {            
            var runningContainers = await _dockerClient.Containers
                .ListContainersAsync(new ContainersListParameters());
            
            foreach (var runningContainer in runningContainers.Where(cont => cont.Names.Any(n => n.StartsWith(config.NamePrefix))))
            {
                // Stopping all test containers that are older than one hour, they likely failed to cleanup
                if (config.HostPort > 0 || (runningContainer.Created < DateTime.UtcNow.AddHours(-1)))
                {
                    try
                    {
                        await RemoveContainer(runningContainer.ID);
                    }
                    catch
                    {
                        // Ignoring failures to stop running containers
                    }
                }
            }
        }


        private static int GetFreePort()
        {
            var tcpListener = new TcpListener(IPAddress.Loopback, 0);
            tcpListener.Start();
            try
            {
                var port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
                return port;
            }
            finally
            {
                tcpListener.Stop();                
            }
        }

        private static bool IsRunningOnWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }
    }

}
