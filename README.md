# ![Logo](CybtansSDK/cybtan.png) Cybtans SDK
[![Build Status](https://dev.azure.com/cybtans/CybtansSDK/_apis/build/status/ansel86castro.cybtans-sdk?branchName=master)](https://dev.azure.com/cybtans/CybtansSDK/_build/latest?definitionId=1&branchName=master)

## Cybtans CLI
The Cybtans command-line interface (CLI) is a cross-platform code generation tool for developing microservice based applications. Use the tool for generating microservice's projects structure, app service contracts, data transfer objects, controllers, client libraries for .NET and Typescript among other utilities. 

### Getting Started

First [download](https://github.com/ansel86castro/cybtans-sdk/releases/download/v3.1.0/cybtans-cli_x64.zip) the **cybtans-cli** tool from the latest release. Extract it and additionally add the directory where you extract the executable to your PATH.

Let's create a solution file for this example
```bash
 dotnet new sln -n MySolution
```

Next let's create a microservice for managing Product's data
```bash
cybtans-cli service -n Products -o ./Products -sln .\MySolution.sln
``` 

The tool generates the following project structures under the **Products** folder
- **Products.Client**:
The microservice client library for C#
- **Products.Models**
The microservice data transfer objects ,request and response messages
- **Products.RestApi**
The microservice Rest API interface. Defines endpoints for calling the microservice operations
- **Products.Services**
The microservice business logic
- **Products.Services.Tests**
The microservice business logic tests
- **Proto**
The microservice protobuff definitions

The cli leverage the [Google Protocol Buffer](https://developers.google.com/protocol-buffers) syntax for defining services ,request and response message. From those definitions client libraries, business services ,dtos and controllers are generated. 

Next let's add some definitions to the `Product.proto` file inside the generated **Proto** directory

```proto
syntax = "proto3";

package Products;

message Product{
	int32 id = 1;
	string name = 2 [required = true];
	string description = 3;
	float price = 4 [optional = true];
	string pictureFileName = 5;
	string pictureUrl = 6;
	int32 brandId = 7;
	int32 catalogId = 8;
	int32 avalaibleStock = 9;
	int32 RestockThreshold = 10;
	datetime createDate = 11;
	datetime updateDate = 12 [optional = true];

	Catalog catalog = 13;
	Brand brand = 14;

	repeated Comment comments = 15;
}

message Catalog {
	int32 id = 1;
	string name = 2;
}

message Brand {
	int32 id = 1;
	string name = 2;
}

message Comment {
	int32 id = 1;
	string text = 2;
	string username = 3;
	int32 userId = 4;
	datetime date = 5;
	int8 rating = 6;
}

message GetProductListRequest {
	string filter = 1;
	string sort = 2;
	int32 skip = 3;
	int32 take = 4 [default = 50];
}

message GetProductRequest {
	int32 id = 1;
}

message UpdateProductRequest {
	int32 id = 1;
	Product Product = 2 [optional = true];
}

message DeleteRequest{
	int32 id = 1;
}

message GetProductListResponse {
	repeated Product items = 1;
	int32 page = 2;
	int32 totalPages = 3;
}


service ProductsService {
	option (prefix) ="api/products";

	rpc GetProducts(GetProductListRequest) returns (GetProductListResponse){		
		option method = "GET";
	};

	rpc GetProduct(GetProductRequest) returns (Product){	
		option template = "{id}"; 
		option method = "GET";
	};

	rpc CreateProduct(Product) returns (Product){			
		option method = "POST";
	};

	rpc UpdateProduct(UpdateProductRequest) returns (Product){			
		option template = "{id}"; 
		option method = "PUT";
	};

	rpc DeleteProduct(DeleteRequest) returns (void){		
		option template = "{id}"; 
		option method = "DELETE";
	};
}
```
There is a file `cybtans.json` in the microservice root directory that provides configurations settings for the cybtans-cli in order to generate code from the proto file. To generate code from the proto just open a terminal in the solution's root directory and run

```bash
cybtans-cli .
```
The cybtans-cli search recursively into all the subdirectories for the `cybtans.json` files and run the proto generator from those settings

Now run the **Products.RestApi** project and navigate in a browser to https://localhost:5001/swagger ,you will see the swagger page containing the descriptions for endpoints of your service's operations/rpcs.

The endpoints are not implemented so it's up to you to implement the service's interface and register the implementation with `ServiceCollection` for dependency injection.


## Cybtans.Serialization

Use the `BinarySerializer` to serialize/deserialize objects to and from bytes arrays or stream. The binary serializer is very fast and when used with objects that implements `IReflectorMetadataProvider` is super fast. It also generates less than a half of the bytes that JSON generates ,thus improving the bandwidth for sending and receiving messages.

### Usage. 
Nuget Console.  
``` 
Install-Package Cybtans.Serialization -Version 1.0.6
```

DotNet Core CLI.  
```
dotnet add package Cybtans.Serialization --version 1.0.6
```

### Example

```csharp
    byte[] buffer = BinaryConvert.Serialize(modelA);
    Model result = BinaryConvert.Deserialize<Model>(buffer);

    //Using Unicode encoding
    var serializer = new BinarySerializer(Encoding.Unicode);
    buffer = serializer.Serialize(model);
    result = serializer.Deserialize<Model>(buffer);
```

#### Test Results

| Library       | Operation | Ticks | Miliseconds|
|---------------|----------:|------:|-----------:|
|Cybtans Binary |Serialize  |97247 ticks| 9 ms   |
|Cybtans Binary |Deserialize| 236388 ticks| 23 ms|
|System.Text.Json |Serialize| 775147 ticks| 77 ms|
|System.Text.Json |Deserialize| 1063610 ticks| 106 ms|


|   Library      | Size      |
|----------------|-----------|
|System.Text.Json|16277 bytes |
|Cytans Binary|10605 bytes    |

#### Benchmark
``` ini
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.836 (1909/November2018Update/19H2)
Intel Core i5-5200U CPU 2.20GHz (Broadwell), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=3.1.201
  [Host]     : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT
  Job-LYLIXC : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT

Runtime=.NET Core 3.1  RunStrategy=Throughput  

```
|                              Method |       Mean |    Error |   StdDev |     Median |    Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------------------ |-----------:|---------:|---------:|-----------:|---------:|------:|------:|----------:|
|                     BinarySerialize |   930.3 μs | 28.55 μs | 78.15 μs |   906.3 μs | 154.2969 |     - |     - |  238.3 KB |
| BinarySerializeWithMetadataProvider |   182.0 μs |  3.73 μs | 10.69 μs |   179.4 μs |  37.8418 |     - |     - |  58.13 KB |
|                       System.Text.Json.JsonSerializer |   684.0 μs | 13.42 μs | 37.84 μs |   678.8 μs |  45.8984 |     - |     - |     71 KB |
|   System.Text.Json.JsonSerializerWithMetadataProvider |   255.6 μs |  5.03 μs | 12.25 μs |   253.9 μs |  20.9961 |     - |     - |  32.62 KB |
|                 NewtonSoftSerialize | 1,032.5 μs | 23.35 μs | 68.49 μs | 1,025.0 μs | 115.2344 |     - |     - | 178.68 KB |





