
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   Powered By Cybtans    
// </auto-generated>
//******************************************************

using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Cybtans.Test.Gateway.Repository.Definition;
using mds = global::Cybtans.Tests.Gateway.Models;
using Cybtans.Services;

namespace Cybtans.Test.Gateway.Repository.Implementation
{
	[RegisterDependency(typeof(IGreeter))]
	public class Greeter : IGreeter
	{
		private readonly global::Cybtans.Tests.Grpc.Greeter.GreeterClient  _client;
		private readonly ILogger<Greeter> _logger;
		
		public Greeter(global::Cybtans.Tests.Grpc.Greeter.GreeterClient client, ILogger<Greeter> logger)
		{
			_client = client;
			_logger = logger;
		}
		
		public async Task<mds::HelloReply> SayHello(mds::HelloRequest request)
		{
			try
			{
				var response = await _client.SayHelloAsync(request.ToProtobufModel());
				return response.ToPocoModel();
			}
			catch(RpcException ex)
			{
				_logger.LogError(ex, "Failed grpc call Cybtans.Tests.Grpc.Greeter.GreeterClient.SayHello");
				throw;
			}
		}
	}

}
