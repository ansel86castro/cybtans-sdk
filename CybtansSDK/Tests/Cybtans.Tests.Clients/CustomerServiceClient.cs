
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
using models = global::Cybtans.Tests.Models;

namespace Cybtans.Tests.Clients
{
	[ApiClient]
	public class CustomerServiceClient : global::Cybtans.Tests.Services.ICustomerService
	{
		private readonly HttpClient _client;
		private readonly IHttpContentSerializer _serializer;
		private readonly Lazy<JsonSerializerOptions> _jsonOptions = new Lazy<JsonSerializerOptions>(() => new() { PropertyNameCaseInsensitive = true }, true);
		
		public CustomerServiceClient(HttpClient client, IHttpContentSerializer serializer = null)
		{
			_client = client;
			_serializer = serializer;
		}
	
		#region Public
		
		/// <summary>
		/// Returns a collection of CustomerDto
		/// </summary>
		public async Task<models::GetAllCustomerResponse> GetAll(models::GetAllRequest request = null)
		{
			using var httpReq = new HttpRequestMessage(HttpMethod.Get, $"/api/Customer?{_GetQueryString(request)}");
			httpReq.Headers.Add("Authorization", "Bearer");
			httpReq.Headers.Add("Accept", _serializer?.ContentType ?? "application/json");
			
			HttpResponseMessage response = null;
			try
			{
			
				response = await _client.SendAsync(httpReq).ConfigureAwait(false);
				if (!response.IsSuccessStatusCode) throw await ApiException.Create(httpReq, response);
				var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
				return _serializer != null ?
				await _serializer.FromStreamAsync<models::GetAllCustomerResponse>(responseStream).ConfigureAwait(false) :
				await System.Text.Json.JsonSerializer.DeserializeAsync<models::GetAllCustomerResponse>(responseStream, _jsonOptions.Value).ConfigureAwait(false);
			
			}
			finally
			{
				response?.Dispose();
			}
		
		}
		
		/// <summary>
		/// Returns one CustomerDto by Id
		/// </summary>
		public async Task<models::CustomerDto> Get(models::GetCustomerRequest request)
		{
			using var httpReq = new HttpRequestMessage(HttpMethod.Get, $"/api/Customer/{request.Id}");
			httpReq.Headers.Add("Authorization", "Bearer");
			httpReq.Headers.Add("Accept", _serializer?.ContentType ?? "application/json");
			
			HttpResponseMessage response = null;
			try
			{
			
				response = await _client.SendAsync(httpReq).ConfigureAwait(false);
				if (!response.IsSuccessStatusCode) throw await ApiException.Create(httpReq, response);
				var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
				return _serializer != null ?
				await _serializer.FromStreamAsync<models::CustomerDto>(responseStream).ConfigureAwait(false) :
				await System.Text.Json.JsonSerializer.DeserializeAsync<models::CustomerDto>(responseStream, _jsonOptions.Value).ConfigureAwait(false);
			
			}
			finally
			{
				response?.Dispose();
			}
		
		}
		
		/// <summary>
		/// Creates one CustomerDto
		/// </summary>
		public async Task<models::CustomerDto> Create(models::CreateCustomerRequest request)
		{
			using var httpReq = new HttpRequestMessage(HttpMethod.Post, $"/api/Customer");
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
				await _serializer.FromStreamAsync<models::CustomerDto>(responseStream).ConfigureAwait(false) :
				await System.Text.Json.JsonSerializer.DeserializeAsync<models::CustomerDto>(responseStream, _jsonOptions.Value).ConfigureAwait(false);
			
			}
			finally
			{
				response?.Dispose();
				memoryOwner?.Dispose();
			}
		
		}
		
		/// <summary>
		/// Updates one CustomerDto by Id
		/// </summary>
		public async Task<models::CustomerDto> Update(models::UpdateCustomerRequest request)
		{
			using var httpReq = new HttpRequestMessage(HttpMethod.Put, $"/api/Customer/{request.Id}");
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
				await _serializer.FromStreamAsync<models::CustomerDto>(responseStream).ConfigureAwait(false) :
				await System.Text.Json.JsonSerializer.DeserializeAsync<models::CustomerDto>(responseStream, _jsonOptions.Value).ConfigureAwait(false);
			
			}
			finally
			{
				response?.Dispose();
				memoryOwner?.Dispose();
			}
		
		}
		
		/// <summary>
		/// Deletes one CustomerDto by Id
		/// </summary>
		public async Task Delete(models::DeleteCustomerRequest request)
		{
			using var httpReq = new HttpRequestMessage(HttpMethod.Delete, $"/api/Customer/{request.Id}");
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
		
		private string _GetQueryString(models::GetAllRequest request)
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
