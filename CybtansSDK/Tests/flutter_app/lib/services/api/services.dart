
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   Powered By Cybtans    
// </auto-generated>
//******************************************************

import 'dart:typed_data';
import 'dart:convert';
import 'package:http/http.dart' as http;
import './models.dart';


String _getQueryString(Map<String, dynamic> values){
  var list = <String>[];
  for(var entry in values.entries){
    if(entry.value == null) {
      continue;
    }
    if(entry.value is List){
      for(var item in entry.value){
        list.add('${entry.key}=${ Uri.encodeQueryComponent(item is DateTime? item.toIso8601String(): item.toString())}');
      }
    }else if (entry.value is DateTime){
      list.add('${entry.key}=${ Uri.encodeQueryComponent(entry.value.toIso8601String())}');
    }
    else{
      list.add('${entry.key}=${ Uri.encodeQueryComponent(entry.value.toString())}');
    }
  }

  return list.isNotEmpty ? '?'+list.join('&'):'';
}

class CustomerService {
    final http.Client _client;
    final String _baseUrl;

    CustomerService(this._client, this._baseUrl);
    
    /// Returns a collection of CustomerDto
    Future<GetAllCustomerResponse> getAll(GetAllRequest request) async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/Customer'+ _getQueryString(request.toJson())));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Authorization'] = 'Bearer';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return GetAllCustomerResponse.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Returns one CustomerDto by Id
    Future<CustomerDto> get(GetCustomerRequest request) async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/Customer/${request.id}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Authorization'] = 'Bearer';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return CustomerDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Creates one CustomerDto
    Future<CustomerDto> create(CreateCustomerRequest request) async {
    	var httpRequest = http.Request('POST', Uri.parse('$_baseUrl/api/Customer'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Authorization'] = 'Bearer';
    	httpRequest.headers['Content-Type'] = 'application/json';
    	httpRequest.bodyBytes = httpRequest.encoding.encode(jsonEncode(request.toJson()));
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return CustomerDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Updates one CustomerDto by Id
    Future<CustomerDto> update(UpdateCustomerRequest request) async {
    	var httpRequest = http.Request('PUT', Uri.parse('$_baseUrl/api/Customer/${request.id}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Authorization'] = 'Bearer';
    	httpRequest.headers['Content-Type'] = 'application/json';
    	httpRequest.bodyBytes = httpRequest.encoding.encode(jsonEncode(request.toJson()));
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return CustomerDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Deletes one CustomerDto by Id
    Future<void> delete(DeleteCustomerRequest request) async {
    	var httpRequest = http.Request('DELETE', Uri.parse('$_baseUrl/api/Customer/${request.id}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Authorization'] = 'Bearer';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    }

}

class CustomerEventService {
    final http.Client _client;
    final String _baseUrl;

    CustomerEventService(this._client, this._baseUrl);
    
    /// Returns a collection of CustomerEventDto
    Future<GetAllCustomerEventResponse> getAll(GetAllRequest request) async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/CustomerEvent'+ _getQueryString(request.toJson())));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Authorization'] = 'Bearer';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return GetAllCustomerEventResponse.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Returns one CustomerEventDto by Id
    Future<CustomerEventDto> get(GetCustomerEventRequest request) async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/CustomerEvent/${request.id}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Authorization'] = 'Bearer';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return CustomerEventDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Creates one CustomerEventDto
    Future<CustomerEventDto> create(CreateCustomerEventRequest request) async {
    	var httpRequest = http.Request('POST', Uri.parse('$_baseUrl/api/CustomerEvent'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Authorization'] = 'Bearer';
    	httpRequest.headers['Content-Type'] = 'application/json';
    	httpRequest.bodyBytes = httpRequest.encoding.encode(jsonEncode(request.toJson()));
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return CustomerEventDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Updates one CustomerEventDto by Id
    Future<CustomerEventDto> update(UpdateCustomerEventRequest request) async {
    	var httpRequest = http.Request('PUT', Uri.parse('$_baseUrl/api/CustomerEvent/${request.id}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Authorization'] = 'Bearer';
    	httpRequest.headers['Content-Type'] = 'application/json';
    	httpRequest.bodyBytes = httpRequest.encoding.encode(jsonEncode(request.toJson()));
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return CustomerEventDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Deletes one CustomerEventDto by Id
    Future<void> delete(DeleteCustomerEventRequest request) async {
    	var httpRequest = http.Request('DELETE', Uri.parse('$_baseUrl/api/CustomerEvent/${request.id}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Authorization'] = 'Bearer';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    }

}

/// Order's Service
class OrderService {
    final http.Client _client;
    final String _baseUrl;

    OrderService(this._client, this._baseUrl);
    
    /// Hellow; "Func"
    Future<void> foo() async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/Order/foo'));
    	httpRequest.headers['Accept'] = 'application/json';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    }
    
    Future<void> baar() async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/Order/baar'));
    	httpRequest.headers['Accept'] = 'application/json';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    }
    
    Future<void> test() async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/Order/test'));
    	httpRequest.headers['Accept'] = 'application/json';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    }
    
    Future<void> argument() async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/Order/arg'));
    	httpRequest.headers['Accept'] = 'application/json';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    }
    
    /// Upload an image to the server
    Future<UploadImageResponse> uploadImage(UploadImageRequest request) async {
    	var httpRequest = http.MultipartRequest('POST', Uri.parse('$_baseUrl/api/Order/upload'));
    	httpRequest.headers['Accept'] = 'application/json';
    	if(request.name != null) httpRequest.fields['name'] = request.name!;
    	if(request.size != null) httpRequest.fields['size'] = request.size!.toString();
    	if(request.image != null) httpRequest.files.add(http.MultipartFile.fromBytes('image', request.image!));	
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return UploadImageResponse.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    Future<UploadStreamResponse> uploadStreamById(UploadStreamByIdRequest request) async {
    	var httpRequest = http.MultipartRequest('POST', Uri.parse('$_baseUrl/api/Order/${request.id}/upload'));
    	httpRequest.headers['Accept'] = 'application/json';
    	if(request.id != null) httpRequest.fields['id'] = request.id!;
    	if(request.data != null) httpRequest.files.add(http.MultipartFile.fromBytes('data', request.data!));	
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return UploadStreamResponse.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    Future<UploadStreamResponse> uploadStream(Uint8List request) async {
    	var httpRequest = http.MultipartRequest('POST', Uri.parse('$_baseUrl/api/Order/ByteStream'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.files.add(http.MultipartFile.fromBytes('blob', request));	
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return UploadStreamResponse.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    Future<http.StreamedResponse> downloadImage(DownloadImageRequest request) async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/Order/download'+ _getQueryString(request.toJson())));
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return response;
    }
    
    Future<void> getMultiPath(MultiPathRequest request) async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/Order/${request.param1}multipath/${request.param2}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    }
    
    Future<void> sendNotification(OrderNotification request) async {
    	var httpRequest = http.Request('POST', Uri.parse('$_baseUrl/api/Order/${request.orderId}/notify/${request.userId}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Content-Type'] = 'application/json';
    	httpRequest.bodyBytes = httpRequest.encoding.encode(jsonEncode(request.toJson()));
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    }
    
    Future<GetAllNamesResponse> getAllNames() async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/Order/names'));
    	httpRequest.headers['Accept'] = 'application/json';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return GetAllNamesResponse.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    Future<OrderNamesDto> getOrderName(GetOrderNameRequest request) async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/Order/names/${request.id}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return OrderNamesDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    Future<OrderNamesDto> createOrderName(CreateOrderNameRequest request) async {
    	var httpRequest = http.Request('POST', Uri.parse('$_baseUrl/api/Order/names'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Content-Type'] = 'application/json';
    	httpRequest.bodyBytes = httpRequest.encoding.encode(jsonEncode(request.toJson()));
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return OrderNamesDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Returns a collection of OrderDto
    Future<GetAllOrderResponse> getAll(GetAllRequest request) async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/Order'+ _getQueryString(request.toJson())));
    	httpRequest.headers['Accept'] = 'application/json';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return GetAllOrderResponse.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Returns one OrderDto by Id
    Future<OrderDto> get(GetOrderRequest request) async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/Order/${request.id}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return OrderDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Creates one OrderDto
    Future<OrderDto> create(CreateOrderRequest request) async {
    	var httpRequest = http.Request('POST', Uri.parse('$_baseUrl/api/Order'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Content-Type'] = 'application/json';
    	httpRequest.bodyBytes = httpRequest.encoding.encode(jsonEncode(request.toJson()));
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return OrderDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Updates one OrderDto by Id
    Future<OrderDto> update(UpdateOrderRequest request) async {
    	var httpRequest = http.Request('PUT', Uri.parse('$_baseUrl/api/Order/${request.id}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Content-Type'] = 'application/json';
    	httpRequest.bodyBytes = httpRequest.encoding.encode(jsonEncode(request.toJson()));
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return OrderDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Deletes one OrderDto by Id
    Future<void> delete(DeleteOrderRequest request) async {
    	var httpRequest = http.Request('DELETE', Uri.parse('$_baseUrl/api/Order/${request.id}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    }

}

class OrderStateService {
    final http.Client _client;
    final String _baseUrl;

    OrderStateService(this._client, this._baseUrl);
    
    /// Returns a collection of OrderStateDto
    Future<GetAllOrderStateResponse> getAll(GetAllRequest request) async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/OrderState'+ _getQueryString(request.toJson())));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Authorization'] = 'Bearer';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return GetAllOrderStateResponse.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Returns one OrderStateDto by Id
    Future<OrderStateDto> get(GetOrderStateRequest request) async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/OrderState/${request.id}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Authorization'] = 'Bearer';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return OrderStateDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Creates one OrderStateDto
    Future<OrderStateDto> create(CreateOrderStateRequest request) async {
    	var httpRequest = http.Request('POST', Uri.parse('$_baseUrl/api/OrderState'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Authorization'] = 'Bearer';
    	httpRequest.headers['Content-Type'] = 'application/json';
    	httpRequest.bodyBytes = httpRequest.encoding.encode(jsonEncode(request.toJson()));
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return OrderStateDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Updates one OrderStateDto by Id
    Future<OrderStateDto> update(UpdateOrderStateRequest request) async {
    	var httpRequest = http.Request('PUT', Uri.parse('$_baseUrl/api/OrderState/${request.id}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Authorization'] = 'Bearer';
    	httpRequest.headers['Content-Type'] = 'application/json';
    	httpRequest.bodyBytes = httpRequest.encoding.encode(jsonEncode(request.toJson()));
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return OrderStateDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Deletes one OrderStateDto by Id
    Future<void> delete(DeleteOrderStateRequest request) async {
    	var httpRequest = http.Request('DELETE', Uri.parse('$_baseUrl/api/OrderState/${request.id}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Authorization'] = 'Bearer';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    }

}

class ReadOnlyEntityService {
    final http.Client _client;
    final String _baseUrl;

    ReadOnlyEntityService(this._client, this._baseUrl);
    
    /// Returns a collection of ReadOnlyEntityDto
    Future<GetAllReadOnlyEntityResponse> getAll(GetAllRequest request) async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/ReadOnlyEntity'+ _getQueryString(request.toJson())));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Authorization'] = 'Bearer';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return GetAllReadOnlyEntityResponse.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Returns one ReadOnlyEntityDto by Id
    Future<ReadOnlyEntityDto> get(GetReadOnlyEntityRequest request) async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/ReadOnlyEntity/${request.id}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Authorization'] = 'Bearer';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return ReadOnlyEntityDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }

}

class SoftDeleteOrderService {
    final http.Client _client;
    final String _baseUrl;

    SoftDeleteOrderService(this._client, this._baseUrl);
    
    /// Returns a collection of SoftDeleteOrderDto
    Future<GetAllSoftDeleteOrderResponse> getAll(GetAllRequest request) async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/SoftDeleteOrder'+ _getQueryString(request.toJson())));
    	httpRequest.headers['Accept'] = 'application/json';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return GetAllSoftDeleteOrderResponse.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Returns one SoftDeleteOrderDto by Id
    Future<SoftDeleteOrderDto> get(GetSoftDeleteOrderRequest request) async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/SoftDeleteOrder/${request.id}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return SoftDeleteOrderDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Creates one SoftDeleteOrderDto
    Future<SoftDeleteOrderDto> create(CreateSoftDeleteOrderRequest request) async {
    	var httpRequest = http.Request('POST', Uri.parse('$_baseUrl/api/SoftDeleteOrder'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Content-Type'] = 'application/json';
    	httpRequest.bodyBytes = httpRequest.encoding.encode(jsonEncode(request.toJson()));
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return SoftDeleteOrderDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Updates one SoftDeleteOrderDto by Id
    Future<SoftDeleteOrderDto> update(UpdateSoftDeleteOrderRequest request) async {
    	var httpRequest = http.Request('PUT', Uri.parse('$_baseUrl/api/SoftDeleteOrder/${request.id}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Content-Type'] = 'application/json';
    	httpRequest.bodyBytes = httpRequest.encoding.encode(jsonEncode(request.toJson()));
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return SoftDeleteOrderDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    /// Deletes one SoftDeleteOrderDto by Id
    Future<void> delete(DeleteSoftDeleteOrderRequest request) async {
    	var httpRequest = http.Request('DELETE', Uri.parse('$_baseUrl/api/SoftDeleteOrder/${request.id}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    }

}

/// Jwt Authentication Service
class AuthenticationService {
    final http.Client _client;
    final String _baseUrl;

    AuthenticationService(this._client, this._baseUrl);
    
    /// Generates an access token
    Future<LoginResponse> login(LoginRequest request) async {
    	var httpRequest = http.Request('POST', Uri.parse('$_baseUrl/api/auth/login'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Content-Type'] = 'application/json';
    	httpRequest.bodyBytes = httpRequest.encoding.encode(jsonEncode(request.toJson()));
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return LoginResponse.fromJson(jsonDecode(await response.stream.bytesToString()));
    }

}

class ClientService {
    final http.Client _client;
    final String _baseUrl;

    ClientService(this._client, this._baseUrl);
    
    Future<ClientDto> getClient(ClientRequest request) async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/clients/${request.id}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Authorization'] = 'Bearer';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return ClientDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    Future<ClientDto> getClient2(ClientRequest request) async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/clients/client2/${request.id}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Authorization'] = 'Bearer';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return ClientDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }
    
    Future<ClientDto> getClient3(ClientRequest request) async {
    	var httpRequest = http.Request('GET', Uri.parse('$_baseUrl/api/clients/client3/${request.id}'));
    	httpRequest.headers['Accept'] = 'application/json';
    	httpRequest.headers['Authorization'] = 'Bearer';
    	var response = await _client.send(httpRequest);
    	
    	if (response.statusCode < 200 || response.statusCode >= 400) {
    	      throw AssertionError ({
    	        'status': response.statusCode,
    	        'statusText': response.reasonPhrase,
    	        'msg': await response.stream.bytesToString()
    	      });
    	}
    	
    	return ClientDto.fromJson(jsonDecode(await response.stream.bytesToString()));
    }

}
