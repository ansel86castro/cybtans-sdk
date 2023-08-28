
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   Powered By Cybtans    
// </auto-generated>
//******************************************************

using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Cybtans.Common;
using mds = global::Cybtans.Tests.Models;

namespace Cybtans.Tests.Clients
{
	[ApiClient]
	public class ClientServiceClient : IClientServiceClient
	{
		private readonly HttpClient _client;
		private readonly IHttpContentSerializer _serializer;
		private readonly Lazy<JsonSerializerOptions> _jsonOptions = new Lazy<JsonSerializerOptions>(() => new() { PropertyNameCaseInsensitive = true }, true);
		
		public ClientServiceClient(HttpClient client, IHttpContentSerializer serializer = null)
		{
			_client = client;
			_serializer = serializer;
		}
	
		#region Public
		
		public async Task<mds::ClientDto> GetClient(mds::ClientRequest request)
		{
			using var httpReq = new HttpRequestMessage(HttpMethod.Get, $"/api/clients/{request.Id}");
			httpReq.Headers.Add("Authorization", "Bearer");
			httpReq.Headers.Add("Accept", _serializer?.ContentType ?? "application/json");
			HttpResponseMessage response = null;
			try
			{
			
			response = await _client.SendAsync(httpReq).ConfigureAwait(false);
			if (!response.IsSuccessStatusCode) throw await ApiException.Create(httpReq, response);
			var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
			return _serializer != null ?
				await _serializer.FromStreamAsync<mds::ClientDto>(responseStream).ConfigureAwait(false) :
				await System.Text.Json.JsonSerializer.DeserializeAsync<mds::ClientDto>(responseStream, _jsonOptions.Value).ConfigureAwait(false);
			
			}
			finally
			{
				response?.Dispose();
			}
		
		}
		
		public async Task<mds::ClientDto> GetClient2(mds::ClientRequest request)
		{
			using var httpReq = new HttpRequestMessage(HttpMethod.Get, $"/api/clients/client2/{request.Id}");
			httpReq.Headers.Add("Authorization", "Bearer");
			httpReq.Headers.Add("Accept", _serializer?.ContentType ?? "application/json");
			HttpResponseMessage response = null;
			try
			{
			
			response = await _client.SendAsync(httpReq).ConfigureAwait(false);
			if (!response.IsSuccessStatusCode) throw await ApiException.Create(httpReq, response);
			var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
			return _serializer != null ?
				await _serializer.FromStreamAsync<mds::ClientDto>(responseStream).ConfigureAwait(false) :
				await System.Text.Json.JsonSerializer.DeserializeAsync<mds::ClientDto>(responseStream, _jsonOptions.Value).ConfigureAwait(false);
			
			}
			finally
			{
				response?.Dispose();
			}
		
		}
		
		public async Task<mds::ClientDto> GetClient3(mds::ClientRequest request)
		{
			using var httpReq = new HttpRequestMessage(HttpMethod.Get, $"/api/clients/client3/{request.Id}");
			httpReq.Headers.Add("Authorization", "Bearer");
			httpReq.Headers.Add("Accept", _serializer?.ContentType ?? "application/json");
			HttpResponseMessage response = null;
			try
			{
			
			response = await _client.SendAsync(httpReq).ConfigureAwait(false);
			if (!response.IsSuccessStatusCode) throw await ApiException.Create(httpReq, response);
			var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
			return _serializer != null ?
				await _serializer.FromStreamAsync<mds::ClientDto>(responseStream).ConfigureAwait(false) :
				await System.Text.Json.JsonSerializer.DeserializeAsync<mds::ClientDto>(responseStream, _jsonOptions.Value).ConfigureAwait(false);
			
			}
			finally
			{
				response?.Dispose();
			}
		
		}
		
		#endregion Public
		
		#region Private
		
		#endregion Private
		
	
	}

}