# cybtans-sdk
## Cybtans.Serialization

Use the `BinarySerializer` to serialize/deserialize objects to and from bytes arrays or stream. The binary serializer is very fast and when used with objects that implements `IReflectorMetadataProvider` is super fast. It also generates less than a half of the bytes that JSON generates ,thus improving the bandwidth for sending and receiving messages.

**Benchmark**

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
|                       JsonSerialize |   684.0 μs | 13.42 μs | 37.84 μs |   678.8 μs |  45.8984 |     - |     - |     71 KB |
|   JsonSerializeWithMetadataProvider |   255.6 μs |  5.03 μs | 12.25 μs |   253.9 μs |  20.9961 |     - |     - |  32.62 KB |
|                 NewtonSoftSerialize | 1,032.5 μs | 23.35 μs | 68.49 μs | 1,025.0 μs | 115.2344 |     - |     - | 178.68 KB |

μs = 1 microsecond

## Service Generator
Usages
Generates a microservice's project structure in a solution folder
ServiceGenerator -n Customer -o Customer -sln eShop.sln

Generates microservice's code including models, web api controllers, refit clients and app's services from a protobuff file
ServiceGenerator proto -n Customer -o . -f ./Proto/Customer.proto

Where -n Customer specify your microservice name and -o Customer specify the folder where your projects will be generated.

Note: When generating the project's structure like for example ServiceGenerator -n Customer -o Customer -sln eShop.sln
it will generate also a generate.bat file with the commands to generate the code from the proto file located in Proto/Customer.proto

It also integrates the refit clients and the web api with cybtans binary serialization library which is faster and generates less bytes than JSON. Also all the model classes the generator creates implements `IReflectorMetadataProvider` therefore the serialization/deserialization of the models is very fast.
