# ![Logo](CybtansSDK/cybtan.png) Cybtans SDK
[![Cybtans.Serialization package in cybtans-sdk@Release feed in Azure Artifacts](https://feeds.dev.azure.com/cybtans/f890b23d-d5ed-45bb-8308-1fe776f4fa8a/_apis/public/Packaging/Feeds/c8742fb6-b06b-42df-b89a-4dc8a8b66de6%40c6956002-6052-4d67-940b-9bcb161382ee/Packages/337fb4d3-cdc2-472e-89d0-f13069e7c086/Badge)](https://dev.azure.com/cybtans/CybtansSDK/_packaging?_a=package&feed=c8742fb6-b06b-42df-b89a-4dc8a8b66de6%40c6956002-6052-4d67-940b-9bcb161382ee&package=337fb4d3-cdc2-472e-89d0-f13069e7c086&preferRelease=true)
[![Build Status](https://dev.azure.com/cybtans/CybtansSDK/_apis/build/status/ansel86castro.cybtans-sdk?branchName=master)](https://dev.azure.com/cybtans/CybtansSDK/_build/latest?definitionId=1&branchName=master)

## Cybtans.Serialization

Use the `BinarySerializer` to serialize/deserialize objects to and from bytes arrays or stream. The binary serializer is very fast and when used with objects that implements `IReflectorMetadataProvider` is super fast. It also generates less than a half of the bytes that JSON generates ,thus improving the bandwidth for sending and receiving messages.

### Usage. 
Nuget Console.  
    Install-Package Cybtans.Serialization -Version 1.0.6

DotNet Core CLI.  
    dotnet add package Cybtans.Serialization --version 1.0.6

### Example

```csharp
    byte[] buffer = BinaryConvert.Serialize(modelA);
    Model result = BinaryConvert.Deserialize<Model>(buffer);

    //Using Unicode encoding
    var serializer = new BinarySerializer(Encoding.Unicode);
    buffer = serializer.Serialize(model);
    result = serializer.Deserialize<Model>(buffer);
```


## Service Generator
Cybtan's Service Generator is a tool for simplifying the development of microservices. It can generate a project structure and code ready for microservice development. The code is generated from a protobuff file containing services definitions and messages.

[Download link](https://github.com/ansel86castro/cybtans-sdk/releases/download/v1.0.6/ServiceGenerator.x64.zip) 

### Example
Generates the Customer microservice's project structure
```bash
  ServiceGenerator -n Customer -o Customer -sln eShop.sln
```

Generates the Customer microservice's code including **models**, **web api controllers**, **typed [refit](https://github.com/reactiveui/refit) clients** and **app's services** from the protobuff file.  
```bash
ServiceGenerator proto -n Customer -o . -f ./Proto/Customer.proto
```

Where -n Customer specify your microservice name and -o Customer specify the folder where your projects will be generated.

Note: When generating the project's structure like for example `ServiceGenerator -n Customer -o Customer -sln eShop.sln`
it will also generate a `generate.bat` so you can run it form the command line to generate the code after changing the proto file

The code template generated when creating the projects integrates the refit clients and the web api with **cybtans binary serialization library** which is faster and output less bytes than JSON. Also all the messages generated from the proto file implements `IReflectorMetadataProvider` therefore the serialization/deserialization of the models is very fast due to it does not use reflection.

## Usage 
Creating a Customer Microservice for an e-store.  
First create a solution  
``` 
dotnet new sln --name eStore 
```
Then generate the microservice structure.  
```
ServiceGenerator -n Customers -o Customers -sln eStore.sln
```
It will generate the following projects inside the `Customer` folder.  
  - *Customers.Services* : Contains your service logic the only project you need to focus
  - *Customers.Clients* : The client library for calling your services. Use this library into client applications or gateways. You will need to import the Customer.Models library.
  - *Customers.Models* : The request and response message of your services
  - *Customers.RestApi* : Transport layer for calling your services through a web api interface.
  - *Customers.Services.Tests* : Test proyect for your services

  Then modify the proto file located in `Proto/Customers.proto`

  ```proto
  syntax = "proto3";

package Customers;

// ******************** MODELS ***********************

message Customer {
	guid id = 1;
	string name = 2 [required = true];
	string description = 3;	
	string pictureUrl = 4;	
	datetime birthday = 5;
	datetime createDate = 6;
	datetime updateDate = 7 [optional = true];	
}

// ******************* REQUESTS **********************

message GetCustomerListRequest {
	string filter = 1;
	string sort = 2;
	int32 page = 3;
}

message GetCustomerRequest {
	guid id = 1;
}

message UpdateCustomerRequest {
	guid id = 1;	
	map<string, object> data = 2 [optional = true];
}

message DeleteCustomerRequest{
	guid id = 1;
}

// ***************** RESPONSES ***********************

message GetCustomerResponse {
	repeated Customer items = 1;
	int32 page = 2;
	int32 totalPages = 3;
}

// ***************** SERVICES ***********************

service CustomerService {
	option (prefix) ="api/customer";

	rpc GetCustomer(GetCustomerRequest) returns (GetCustomerResponse){		
		option method = "GET";
	};

	rpc GetCustomer(GetCustomerRequest) returns (Customer){	
		option template = "{id}"; 
		option method = "GET";
	};

	rpc CreateCustomer(Customer) returns (Customer){			
		option method = "POST";
		option roles = "customer.create";
	};

	rpc UpdateCustomer(UpdateCustomerRequest) returns (Customer){			
		option template = "{id}"; 
		option method = "PUT";		
	};

	rpc DeleteCustomer(DeleteCustomerRequest) returns (void){
		option template = "{id}"; 		
		option method = "DELETE";
		option roles = "customer.delete";
	};
}

```
Generate the code with. 
```
ServiceGenerator proto -n Customer -o . -f ./Proto/Customer.proto
```

or executing the `generate.bat` file. 

Then create a class implementing the CustomerService abstract class

```csharp
public class CustomerServiceImpl : CustomerService
{
    public override Task<Customer> CreateCustomer(Customer request)
    {
        throw new NotImplementedException();
    }

    public override Task DeleteCustomer(DeleteCustomerRequest request)
    {
        throw new NotImplementedException();
    }

    public override Task<GetCustomerResponse> GetCustomer(GetCustomerRequest request)
    {
        throw new NotImplementedException();
    }

    public override Task<Customer> GetCustomer(GetCustomerRequest request)
    {
        throw new NotImplementedException();
    }

    public override Task<Customer> UpdateCustomer(UpdateCustomerRequest request)
    {
        throw new NotImplementedException();
    }
}
```

Register your implementation in the `Customer.RestApi/Startup.cs` 

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Register the Swagger services
    services.AddOpenApiDocument();

    //Register your service implementation here
    services.AddScoped<CustomerService, CustomerServiceImpl>();

    services.AddControllers()
        //Add binary serialization support
        .AddCybtansFormatter();   
}
```



