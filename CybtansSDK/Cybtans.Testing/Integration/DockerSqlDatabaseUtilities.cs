using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Cybtans.Testing.Integration
{
    public static class DockerSqlDatabaseUtilities
    {
        public const string SQLSERVER_SA_PASSWORD = "fH9X3DWt9MwgAnqZ";
        public const string SQLSERVER_IMAGE = "mcr.microsoft.com/mssql/server";
        public const string SQLSERVER_IMAGE_TAG = "2019-latest";

        public static string SQLSERVER_CONTAINER_NAME_PREFIX = "SqlServerIntegrationTest-";

        public static async Task<(string containerId, string port)> StartSqlServerContainer()
        {
            await CleanupRunningContainers();
            var dockerClient = GetDockerClient();
            var freePort = GetFreePort();

            // This call ensures that the latest SQL Server Docker image is pulled
            await dockerClient.Images.CreateImageAsync(new ImagesCreateParameters
            {
                FromImage = $"{SQLSERVER_IMAGE}:{SQLSERVER_IMAGE_TAG}"
            }, null, new Progress<JSONMessage>());

            var sqlContainer = await dockerClient
                .Containers
                .CreateContainerAsync(new CreateContainerParameters
                {
                    Name = SQLSERVER_CONTAINER_NAME_PREFIX + Guid.NewGuid(),
                    Image = $"{SQLSERVER_IMAGE}:{SQLSERVER_IMAGE_TAG}",
                    Env = new List<string>
                    {
                        "ACCEPT_EULA=Y",
                        $"SA_PASSWORD={SQLSERVER_SA_PASSWORD}"
                    },
                    HostConfig = new HostConfig
                    {
                        PortBindings = new Dictionary<string, IList<PortBinding>>
                        {
                            {
                                "1433/tcp",
                                new PortBinding[]
                                {
                                    new PortBinding
                                    {
                                        HostPort = freePort
                                    }
                                }
                            }
                        }
                    }
                });

            await dockerClient
                .Containers
                .StartContainerAsync(sqlContainer.ID, new ContainerStartParameters());

            await WaitUntilDatabaseAvailableAsync(freePort);
            return (sqlContainer.ID, freePort);
        }

        public static async Task RemoveContainer(string dockerContainerId)
        {
            var dockerClient = GetDockerClient();
            await dockerClient.Containers
                .StopContainerAsync(dockerContainerId, new ContainerStopParameters());
            await dockerClient.Containers
                .RemoveContainerAsync(dockerContainerId, new ContainerRemoveParameters());
        }

        private static DockerClient GetDockerClient()
        {
            var dockerUri = IsRunningOnWindows()
                ? "npipe://./pipe/docker_engine"
                : "unix:///var/run/docker.sock";
            return new DockerClientConfiguration(new Uri(dockerUri))
                .CreateClient();
        }

        private static async Task CleanupRunningContainers()
        {
            var dockerClient = GetDockerClient();

            var runningContainers = await dockerClient.Containers
                .ListContainersAsync(new ContainersListParameters());

            foreach (var runningContainer in runningContainers.Where(cont => cont.Names.Any(n => n.Contains(SQLSERVER_CONTAINER_NAME_PREFIX))))
            {
                // Stopping all test containers that are older than one hour, they likely failed to cleanup
                if (runningContainer.Created < DateTime.UtcNow.AddHours(-1))
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

        private static async Task WaitUntilDatabaseAvailableAsync(string databasePort)
        {
            var end = DateTime.UtcNow.AddSeconds(60);
            var connectionEstablised = false;
            while (!connectionEstablised && end > DateTime.UtcNow)
            {
                try
                {
                    var sqlConnectionString = $"Data Source=localhost,{databasePort};Integrated Security=False;User ID=SA;Password={SQLSERVER_SA_PASSWORD}";
                    using var sqlConnection = new SqlConnection(sqlConnectionString);
                    await sqlConnection.OpenAsync();
                    connectionEstablised = true;
                }
                catch
                {
                    // If opening the SQL connection fails, SQL Server is not ready yet
                    await Task.Delay(500);
                }
            }

            if (!connectionEstablised)
            {
                throw new Exception("Connection to the SQL docker database could not be established within 60 seconds.");
            }

            return;
        }

        public static SqlConnection OpenConnection(string port)
        {
            var sqlConnectionString = $"Data Source=localhost,{port};Integrated Security=False;User ID=SA;Password={SQLSERVER_SA_PASSWORD}";
            var conn = new SqlConnection(sqlConnectionString);
            conn.Open();
            return conn;
        }

        private static string GetFreePort()
        {
            var tcpListener = new TcpListener(IPAddress.Loopback, 0);
            tcpListener.Start();
            try
            {
                var port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
                return port.ToString();
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
