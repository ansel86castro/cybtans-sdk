
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
	public class OrderStateServiceClient : IOrderStateServiceClient
	{
		private readonly HttpClient _client;
		private readonly IHttpContentSerializer _serializer;
		private readonly Lazy<JsonSerializerOptions> _jsonOptions = new Lazy<JsonSerializerOptions>(() => new() { PropertyNameCaseInsensitive = true }, true);
		
		public OrderStateServiceClient(HttpClient client, IHttpContentSerializer serializer = null)
		{
			_client = client;
			_serializer = serializer;
		}
	
		#region Public
		
		/// <summary>
		/// Returns a collection of OrderStateDto
		/// </summary>
		public async Task<mds::GetAllOrderStateResponse> GetAll(mds::GetAllRequest request = null)
		{
			using var httpReq = new HttpRequestMessage(HttpMethod.Get, $"/api/OrderState?{_GetQueryString(request)}");
			httpReq.Headers.Add("Authorization", "Bearer");
			httpReq.Headers.Add("Accept", _serializer?.ContentType ?? "application/json");
			HttpResponseMessage response = null;
			try
			{
			
			response = await _client.SendAsync(httpReq).ConfigureAwait(false);
			if (!response.IsSuccessStatusCode) throw await ApiException.Create(httpReq, response);
			var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
			return _serializer != null ?
				await _serializer.FromStreamAsync<mds::GetAllOrderStateResponse>(responseStream).ConfigureAwait(false) :
				await System.Text.Json.JsonSerializer.DeserializeAsync<mds::GetAllOrderStateResponse>(responseStream, _jsonOptions.Value).ConfigureAwait(false);
			
			}
			finally
			{
				response?.Dispose();
			}
		
		}
		
		/// <summary>
		/// Returns one OrderStateDto by Id
		/// </summary>
		public async Task<mds::OrderStateDto> Get(mds::GetOrderStateRequest request)
		{
			using var httpReq = new HttpRequestMessage(HttpMethod.Get, $"/api/OrderState/{request.Id}");
			httpReq.Headers.Add("Authorization", "Bearer");
			httpReq.Headers.Add("Accept", _serializer?.ContentType ?? "application/json");
			HttpResponseMessage response = null;
			try
			{
			
			response = await _client.SendAsync(httpReq).ConfigureAwait(false);
			if (!response.IsSuccessStatusCode) throw await ApiException.Create(httpReq, response);
			var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
			return _serializer != null ?
				await _serializer.FromStreamAsync<mds::OrderStateDto>(responseStream).ConfigureAwait(false) :
				await System.Text.Json.JsonSerializer.DeserializeAsync<mds::OrderStateDto>(responseStream, _jsonOptions.Value).ConfigureAwait(false);
			
			}
			finally
			{
				response?.Dispose();
			}
		
		}
		
		/// <summary>
		/// Creates one OrderStateDto
		/// </summary>
		public async Task<mds::OrderStateDto> Create(mds::CreateOrderStateRequest request)
		{
			using var httpReq = new HttpRequestMessage(HttpMethod.Post, $"/api/OrderState");
			httpReq.Headers.Add("Authorization", "Bearer");
			httpReq.Headers.Add("Accept", _serializer?.ContentType ?? "application/json");
			
			System.Buffers.IMemoryOwner<byte> memoryOwner = null;
			if (_serializer != null)
			{
				memoryOwner = _serializer.ToMemory(request);
				httpReq.Content = new ReadOnlyMemoryContent(memoryOwner.Memory);
				httpReq.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(_serializer.ContentType);
			}
			else
			{
				httpReq.Content = System.Net.Http.Json.JsonContent.Create(request, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
			}
			HttpResponseMessage response = null;
			try
			{
			
			response = await _client.SendAsync(httpReq).ConfigureAwait(false);
			if (!response.IsSuccessStatusCode) throw await ApiException.Create(httpReq, response);
			var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
			return _serializer != null ?
				await _serializer.FromStreamAsync<mds::OrderStateDto>(responseStream).ConfigureAwait(false) :
				await System.Text.Json.JsonSerializer.DeserializeAsync<mds::OrderStateDto>(responseStream, _jsonOptions.Value).ConfigureAwait(false);
			
			}
			finally
			{
				response?.Dispose();
				memoryOwner?.Dispose();
			}
		
		}
		
		/// <summary>
		/// Updates one OrderStateDto by Id
		/// </summary>
		public async Task<mds::OrderStateDto> Update(mds::UpdateOrderStateRequest request)
		{
			using var httpReq = new HttpRequestMessage(HttpMethod.Put, $"/api/OrderState/{request.Id}");
			httpReq.Headers.Add("Authorization", "Bearer");
			httpReq.Headers.Add("Accept", _serializer?.ContentType ?? "application/json");
			
			System.Buffers.IMemoryOwner<byte> memoryOwner = null;
			if (_serializer != null)
			{
				memoryOwner = _serializer.ToMemory(request);
				httpReq.Content = new ReadOnlyMemoryContent(memoryOwner.Memory);
				httpReq.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(_serializer.ContentType);
			}
			else
			{
				httpReq.Content = System.Net.Http.Json.JsonContent.Create(request, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
			}
			HttpResponseMessage response = null;
			try
			{
			
			response = await _client.SendAsync(httpReq).ConfigureAwait(false);
			if (!response.IsSuccessStatusCode) throw await ApiException.Create(httpReq, response);
			var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
			return _serializer != null ?
				await _serializer.FromStreamAsync<mds::OrderStateDto>(responseStream).ConfigureAwait(false) :
				await System.Text.Json.JsonSerializer.DeserializeAsync<mds::OrderStateDto>(responseStream, _jsonOptions.Value).ConfigureAwait(false);
			
			}
			finally
			{
				response?.Dispose();
				memoryOwner?.Dispose();
			}
		
		}
		
		/// <summary>
		/// Deletes one OrderStateDto by Id
		/// </summary>
		public async Task Delete(mds::DeleteOrderStateRequest request)
		{
			using var httpReq = new HttpRequestMessage(HttpMethod.Delete, $"/api/OrderState/{request.Id}");
			httpReq.Headers.Add("Authorization", "Bearer");
			httpReq.Headers.Add("Accept", _serializer?.ContentType ?? "application/json");
			HttpResponseMessage response = null;
			try
			{
			
			response = await _client.SendAsync(httpReq).ConfigureAwait(false);
			if (!response.IsSuccessStatusCode) throw await ApiException.Create(httpReq, response);
			
			}
			finally
			{
				response?.Dispose();
			}
		
		}
		
		#endregion Public
		
		#region Private
		
		private string _GetQueryString(mds::GetAllRequest request)
		{
			if(request == null) return "";
		
			var sb = new StringBuilder();
			if(request.Filter != null)
			{
				if(sb.Length > 0) sb.Append("&"); sb.Append(nameof(request.Filter)).Append("=").Append(Uri.EscapeDataString(request.Filter));
			}
			if(request.Sort != null)
			{
				if(sb.Length > 0) sb.Append("&"); sb.Append(nameof(request.Sort)).Append("=").Append(Uri.EscapeDataString(request.Sort));
			}
			if(request.Skip != null)
			{
				if(sb.Length > 0) sb.Append("&"); sb.Append(nameof(request.Skip)).Append("=").Append(Uri.EscapeDataString(request.Skip.ToString()));
			}
			if(request.Take != null)
			{
				if(sb.Length > 0) sb.Append("&"); sb.Append(nameof(request.Take)).Append("=").Append(Uri.EscapeDataString(request.Take.ToString()));
			}
			return sb.ToString();
		
		}
		
		#endregion Private
		
	
	}

}
