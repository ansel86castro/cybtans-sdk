
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   Powered By Cybtans    
// </auto-generated>
//******************************************************

using System;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Cybtans.Tests.Models;

namespace Cybtans.Tests.Gateway.GraphQL
{
	public class GetAllCustomerResponseGraphType : ObjectGraphType<GetAllCustomerResponse>
	{
		public GetAllCustomerResponseGraphType()
		{
			Field<ListGraphType<CustomerDtoGraphType>>("Items");
			Field<LongGraphType>(nameof(GetAllCustomerResponse.Page));
			Field<LongGraphType>(nameof(GetAllCustomerResponse.TotalPages));
			Field<LongGraphType>(nameof(GetAllCustomerResponse.TotalCount));
		
		}
	}
	
	
	public class CustomerDtoGraphType : ObjectGraphType<CustomerDto>
	{
		public CustomerDtoGraphType()
		{
			Field<StringGraphType>(nameof(CustomerDto.Name)).Description("Customer's Name");
			Field<StringGraphType>(nameof(CustomerDto.FirstLastName), nullable:true).Description("Customer's FirstLastName");
			Field<StringGraphType>(nameof(CustomerDto.SecondLastName), nullable:true).Description("Customer's SecondLastName");
			Field<GuidGraphType>(nameof(CustomerDto.CustomerProfileId), nullable:true).Description("Customer's Profile Id, can be null");
			Field<CustomerProfileDtoGraphType>("CustomerProfile");
			Field<GuidGraphType>(nameof(CustomerDto.Id));
			Field<DateTimeGraphType>("CreateDate");
			Field<DateTimeGraphType>("UpdateDate");
		
		}
	}
	
	
	public class CustomerProfileDtoGraphType : ObjectGraphType<CustomerProfileDto>
	{
		public CustomerProfileDtoGraphType()
		{
			Field<StringGraphType>(nameof(CustomerProfileDto.Name), nullable:true);
			Field<GuidGraphType>(nameof(CustomerProfileDto.Id));
			Field<DateTimeGraphType>("CreateDate");
			Field<DateTimeGraphType>("UpdateDate");
		
		}
	}
	
	
	public class GetAllCustomerEventResponseGraphType : ObjectGraphType<GetAllCustomerEventResponse>
	{
		public GetAllCustomerEventResponseGraphType()
		{
			Field<ListGraphType<CustomerEventDtoGraphType>>("Items");
			Field<LongGraphType>(nameof(GetAllCustomerEventResponse.Page));
			Field<LongGraphType>(nameof(GetAllCustomerEventResponse.TotalPages));
			Field<LongGraphType>(nameof(GetAllCustomerEventResponse.TotalCount));
		
		}
	}
	
	
	public class CustomerEventDtoGraphType : ObjectGraphType<CustomerEventDto>
	{
		public CustomerEventDtoGraphType()
		{
			Field<StringGraphType>(nameof(CustomerEventDto.FullName), nullable:true);
			Field<GuidGraphType>(nameof(CustomerEventDto.CustomerProfileId), nullable:true);
			Field<GuidGraphType>(nameof(CustomerEventDto.Id));
		
		}
	}
	
	
	public class GetAllOrderResponseGraphType : ObjectGraphType<GetAllOrderResponse>
	{
		public GetAllOrderResponseGraphType()
		{
			Field<ListGraphType<OrderDtoGraphType>>("Items");
			Field<LongGraphType>(nameof(GetAllOrderResponse.Page));
			Field<LongGraphType>(nameof(GetAllOrderResponse.TotalPages));
			Field<LongGraphType>(nameof(GetAllOrderResponse.TotalCount));
		
		}
	}
	
	
	public class OrderDtoGraphType : ObjectGraphType<OrderDto>
	{
		public OrderDtoGraphType()
		{
			Field<StringGraphType>(nameof(OrderDto.Description), nullable:true);
			Field<GuidGraphType>(nameof(OrderDto.CustomerId));
			Field<IntGraphType>(nameof(OrderDto.OrderStateId));
			Field<OrderTypeEnumGraphType>("OrderType");
			Field<OrderStateDtoGraphType>("OrderState");
			Field<CustomerDtoGraphType>("Customer", description:"Customer");
			Field<ListGraphType<OrderItemDtoGraphType>>("Items");
			Field<GuidGraphType>(nameof(OrderDto.Id));
			Field<DateTimeGraphType>("CreateDate");
			Field<DateTimeGraphType>("UpdateDate");
		
		}
	}
	
	
	public class OrderTypeEnumGraphType : EnumerationGraphType<OrderTypeEnum>
	{
	}
	
	
	public class OrderStateDtoGraphType : ObjectGraphType<OrderStateDto>
	{
		public OrderStateDtoGraphType()
		{
			Field<StringGraphType>(nameof(OrderStateDto.Name), nullable:true);
			Field<IntGraphType>(nameof(OrderStateDto.Id));
		
		}
	}
	
	
	public class OrderItemDtoGraphType : ObjectGraphType<OrderItemDto>
	{
		public OrderItemDtoGraphType()
		{
			Field<StringGraphType>(nameof(OrderItemDto.ProductName), nullable:true);
			Field<FloatGraphType>(nameof(OrderItemDto.Price));
			Field<FloatGraphType>(nameof(OrderItemDto.Discount), nullable:true);
			Field<GuidGraphType>(nameof(OrderItemDto.OrderId));
			Field<GuidGraphType>(nameof(OrderItemDto.ProductId), nullable:true);
			Field<ProductDtoGraphType>("Product");
			Field<GuidGraphType>(nameof(OrderItemDto.Id));
		
		}
	}
	
	
	public class ProductDtoGraphType : ObjectGraphType<ProductDto>
	{
		public ProductDtoGraphType()
		{
			Field<StringGraphType>(nameof(ProductDto.Name), nullable:true);
			Field<StringGraphType>(nameof(ProductDto.Model), nullable:true);
			Field<GuidGraphType>(nameof(ProductDto.Id));
		
		}
	}
	
	
	public class GetAllOrderStateResponseGraphType : ObjectGraphType<GetAllOrderStateResponse>
	{
		public GetAllOrderStateResponseGraphType()
		{
			Field<ListGraphType<OrderStateDtoGraphType>>("Items");
			Field<LongGraphType>(nameof(GetAllOrderStateResponse.Page));
			Field<LongGraphType>(nameof(GetAllOrderStateResponse.TotalPages));
			Field<LongGraphType>(nameof(GetAllOrderStateResponse.TotalCount));
		
		}
	}
	
	
	public class GetAllReadOnlyEntityResponseGraphType : ObjectGraphType<GetAllReadOnlyEntityResponse>
	{
		public GetAllReadOnlyEntityResponseGraphType()
		{
			Field<ListGraphType<ReadOnlyEntityDtoGraphType>>("Items");
			Field<LongGraphType>(nameof(GetAllReadOnlyEntityResponse.Page));
			Field<LongGraphType>(nameof(GetAllReadOnlyEntityResponse.TotalPages));
			Field<LongGraphType>(nameof(GetAllReadOnlyEntityResponse.TotalCount));
		
		}
	}
	
	
	public class ReadOnlyEntityDtoGraphType : ObjectGraphType<ReadOnlyEntityDto>
	{
		public ReadOnlyEntityDtoGraphType()
		{
			Field<StringGraphType>(nameof(ReadOnlyEntityDto.Name), nullable:true);
			Field<DateTimeGraphType>("CreateDate");
			Field<DateTimeGraphType>("UpdateDate");
			Field<IntGraphType>(nameof(ReadOnlyEntityDto.Id));
		
		}
	}
	
	
	public class GetAllSoftDeleteOrderResponseGraphType : ObjectGraphType<GetAllSoftDeleteOrderResponse>
	{
		public GetAllSoftDeleteOrderResponseGraphType()
		{
			Field<ListGraphType<SoftDeleteOrderDtoGraphType>>("Items");
			Field<LongGraphType>(nameof(GetAllSoftDeleteOrderResponse.Page));
			Field<LongGraphType>(nameof(GetAllSoftDeleteOrderResponse.TotalPages));
			Field<LongGraphType>(nameof(GetAllSoftDeleteOrderResponse.TotalCount));
		
		}
	}
	
	
	public class SoftDeleteOrderDtoGraphType : ObjectGraphType<SoftDeleteOrderDto>
	{
		public SoftDeleteOrderDtoGraphType()
		{
			Field<StringGraphType>(nameof(SoftDeleteOrderDto.Name), nullable:true);
			Field<BooleanGraphType>(nameof(SoftDeleteOrderDto.IsDeleted));
			Field<ListGraphType<SoftDeleteOrderItemDtoGraphType>>("Items");
			Field<GuidGraphType>(nameof(SoftDeleteOrderDto.Id));
			Field<DateTimeGraphType>("CreateDate");
			Field<DateTimeGraphType>("UpdateDate");
		
		}
	}
	
	
	public class SoftDeleteOrderItemDtoGraphType : ObjectGraphType<SoftDeleteOrderItemDto>
	{
		public SoftDeleteOrderItemDtoGraphType()
		{
			Field<StringGraphType>(nameof(SoftDeleteOrderItemDto.Name), nullable:true);
			Field<BooleanGraphType>(nameof(SoftDeleteOrderItemDto.IsDeleted));
			Field<GuidGraphType>(nameof(SoftDeleteOrderItemDto.SoftDeleteOrderId));
			Field<GuidGraphType>(nameof(SoftDeleteOrderItemDto.Id));
			Field<DateTimeGraphType>("CreateDate");
			Field<DateTimeGraphType>("UpdateDate");
		
		}
	}
	
	
	public class DowndloadImageResponseGraphType : ObjectGraphType<DowndloadImageResponse>
	{
		public DowndloadImageResponseGraphType()
		{
			Field<StringGraphType>(nameof(DowndloadImageResponse.FileName), nullable:true);
			Field<StringGraphType>(nameof(DowndloadImageResponse.ContentType), nullable:true);
		
		}
	}
	
	
	public class GetAllNamesResponseGraphType : ObjectGraphType<GetAllNamesResponse>
	{
		public GetAllNamesResponseGraphType()
		{
			Field<ListGraphType<OrderNamesDtoGraphType>>("Items");
		
		}
	}
	
	
	public class OrderNamesDtoGraphType : ObjectGraphType<OrderNamesDto>
	{
		public OrderNamesDtoGraphType()
		{
			Field<StringGraphType>(nameof(OrderNamesDto.Id), nullable:true);
			Field<StringGraphType>(nameof(OrderNamesDto.Description), nullable:true);
		
		}
	}
	
	
	public class ClientDtoGraphType : ObjectGraphType<ClientDto>
	{
		public ClientDtoGraphType()
		{
			Field<GuidGraphType>(nameof(ClientDto.Id));
			Field<StringGraphType>(nameof(ClientDto.Name), nullable:true);
			Field<IntGraphType>(nameof(ClientDto.ClientTypeId));
			Field<IntGraphType>(nameof(ClientDto.ClientStatusId), nullable:true);
			Field<DateTimeGraphType>("CreatedAt");
			Field<IntGraphType>(nameof(ClientDto.CreatorId));
			Field<ClientTypeGraphType>("Type");
			Field<ListGraphType<IntGraphType>>("ItemIds");
		
		}
	}
	
	
	public class ClientTypeGraphType : EnumerationGraphType<ClientType>
	{
	}
	
	
	public partial class GraphQLQueryDefinitions : ObjectGraphType
	{
		public void AddTestDefinitions()
		{
			#region CustomerService
			
			Field<GetAllCustomerResponseGraphType>("customers")
				.Description("Returns a collection of CustomerDto")
				.Arguments(
					new QueryArgument<StringGraphType>(){ Name = "Filter" }
					,new QueryArgument<StringGraphType>(){ Name = "Sort" }
					,new QueryArgument<IntGraphType>(){ Name = "Skip" }
					,new QueryArgument<IntGraphType>(){ Name = "Take" }
				)
				.ResolveAsync(async context =>
				{
					var request = new GetAllRequest();
					request.Filter = context.GetArgument<string>("filter", default(string));
					request.Sort = context.GetArgument<string>("sort", default(string));
					request.Skip = context.GetArgument<int?>("skip", default(int?));
					request.Take = context.GetArgument<int?>("take", default(int?));
					
					using var scope = context.RequestServices.CreateScope();
					var serviceProvider = scope.ServiceProvider;
					
					var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
					var authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();
					var policyResult = await authorizationService.AuthorizeAsync(httpContext.User, httpContext, "AdminUser").ConfigureAwait(false);
					if (!policyResult.Succeeded)
					{
						 throw new UnauthorizedAccessException($"Authorization Failed: { string.Join(", ", policyResult.Failure.FailedRequirements) }");
					}
					
					var service = serviceProvider.GetRequiredService<global::Cybtans.Tests.Services.ICustomerService>();
					var result = await service.GetAll(request).ConfigureAwait(false);
					return result;
				}
			);
			
			Field<CustomerDtoGraphType>("customer")
				.Description("Returns one CustomerDto by Id")
				.Arguments(
					new QueryArgument<GuidGraphType>(){ Name = "Id" }
				)
				.ResolveAsync(async context =>
				{
					var request = new GetCustomerRequest();
					request.Id = context.GetArgument<Guid>("id", default(Guid));
					
					using var scope = context.RequestServices.CreateScope();
					var serviceProvider = scope.ServiceProvider;
					
					var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
					var authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();
					var policyResult = await authorizationService.AuthorizeAsync(httpContext.User, httpContext, "AdminUser").ConfigureAwait(false);
					if (!policyResult.Succeeded)
					{
						 throw new UnauthorizedAccessException($"Authorization Failed: { string.Join(", ", policyResult.Failure.FailedRequirements) }");
					}
					
					var service = serviceProvider.GetRequiredService<global::Cybtans.Tests.Services.ICustomerService>();
					var result = await service.Get(request).ConfigureAwait(false);
					return result;
				}
			);
			
			#endregion CustomerService
			
			#region CustomerEventService
			
			Field<GetAllCustomerEventResponseGraphType>("customerEvents")
				.Description("Returns a collection of CustomerEventDto")
				.Arguments(
					new QueryArgument<StringGraphType>(){ Name = "Filter" }
					,new QueryArgument<StringGraphType>(){ Name = "Sort" }
					,new QueryArgument<IntGraphType>(){ Name = "Skip" }
					,new QueryArgument<IntGraphType>(){ Name = "Take" }
				)
				.ResolveAsync(async context =>
				{
					var request = new GetAllRequest();
					request.Filter = context.GetArgument<string>("filter", default(string));
					request.Sort = context.GetArgument<string>("sort", default(string));
					request.Skip = context.GetArgument<int?>("skip", default(int?));
					request.Take = context.GetArgument<int?>("take", default(int?));
					
					using var scope = context.RequestServices.CreateScope();
					var serviceProvider = scope.ServiceProvider;
					
					var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
					if (!httpContext.User.Identity.IsAuthenticated)
					{
						 throw new UnauthorizedAccessException("Authentication Required");
					}
					
					var service = serviceProvider.GetRequiredService<global::Cybtans.Tests.Services.ICustomerEventService>();
					var result = await service.GetAll(request).ConfigureAwait(false);
					return result;
				}
			);
			
			Field<CustomerEventDtoGraphType>("customerEvent")
				.Description("Returns one CustomerEventDto by Id")
				.Arguments(
					new QueryArgument<GuidGraphType>(){ Name = "Id" }
				)
				.ResolveAsync(async context =>
				{
					var request = new GetCustomerEventRequest();
					request.Id = context.GetArgument<Guid>("id", default(Guid));
					
					using var scope = context.RequestServices.CreateScope();
					var serviceProvider = scope.ServiceProvider;
					
					var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
					if (!httpContext.User.Identity.IsAuthenticated)
					{
						 throw new UnauthorizedAccessException("Authentication Required");
					}
					
					var service = serviceProvider.GetRequiredService<global::Cybtans.Tests.Services.ICustomerEventService>();
					var result = await service.Get(request).ConfigureAwait(false);
					return result;
				}
			);
			
			#endregion CustomerEventService
			
			#region OrderService
			
			Field<DowndloadImageResponseGraphType>("OrderService_DownloadImage")
				.Arguments(
					new QueryArgument<StringGraphType>(){ Name = "Name" }
				)
				.ResolveAsync(async context =>
				{
					var request = new DownloadImageRequest();
					request.Name = context.GetArgument<string>("name", default(string));
					
					using var scope = context.RequestServices.CreateScope();
					var serviceProvider = scope.ServiceProvider;
					
					var service = serviceProvider.GetRequiredService<global::Cybtans.Tests.Services.IOrderService>();
					var result = await service.DownloadImage(request).ConfigureAwait(false);
					return result;
				}
			);
			
			Field<GetAllNamesResponseGraphType>("ordernames")
				.ResolveAsync(async context =>
				{
					
					using var scope = context.RequestServices.CreateScope();
					var serviceProvider = scope.ServiceProvider;
					var service = serviceProvider.GetRequiredService<global::Cybtans.Tests.Services.IOrderService>();
					var result = await service.GetAllNames().ConfigureAwait(false);
					return result;
				}
			);
			
			Field<OrderNamesDtoGraphType>("OrderService_OrderName")
				.Arguments(
					new QueryArgument<StringGraphType>(){ Name = "Id" }
				)
				.ResolveAsync(async context =>
				{
					var request = new GetOrderNameRequest();
					request.Id = context.GetArgument<string>("id", default(string));
					
					using var scope = context.RequestServices.CreateScope();
					var serviceProvider = scope.ServiceProvider;
					
					var service = serviceProvider.GetRequiredService<global::Cybtans.Tests.Services.IOrderService>();
					var result = await service.GetOrderName(request).ConfigureAwait(false);
					
					var interceptor = serviceProvider.GetService<global::Cybtans.AspNetCore.Interceptors.IMessageInterceptor>();
					if( interceptor != null )
					{
						await interceptor.HandleResult(result).ConfigureAwait(false);
					}
					
					return result;
				}
			);
			
			Field<GetAllOrderResponseGraphType>("orders")
				.Description("Returns a collection of OrderDto")
				.Arguments(
					new QueryArgument<StringGraphType>(){ Name = "Filter" }
					,new QueryArgument<StringGraphType>(){ Name = "Sort" }
					,new QueryArgument<IntGraphType>(){ Name = "Skip" }
					,new QueryArgument<IntGraphType>(){ Name = "Take" }
				)
				.ResolveAsync(async context =>
				{
					var request = new GetAllRequest();
					request.Filter = context.GetArgument<string>("filter", default(string));
					request.Sort = context.GetArgument<string>("sort", default(string));
					request.Skip = context.GetArgument<int?>("skip", default(int?));
					request.Take = context.GetArgument<int?>("take", default(int?));
					
					using var scope = context.RequestServices.CreateScope();
					var serviceProvider = scope.ServiceProvider;
					
					var service = serviceProvider.GetRequiredService<global::Cybtans.Tests.Services.IOrderService>();
					var result = await service.GetAll(request).ConfigureAwait(false);
					return result;
				}
			);
			
			Field<OrderDtoGraphType>("order")
				.Description("Returns one OrderDto by Id")
				.Arguments(
					new QueryArgument<GuidGraphType>(){ Name = "Id" }
				)
				.ResolveAsync(async context =>
				{
					var request = new GetOrderRequest();
					request.Id = context.GetArgument<Guid>("id", default(Guid));
					
					using var scope = context.RequestServices.CreateScope();
					var serviceProvider = scope.ServiceProvider;
					
					var service = serviceProvider.GetRequiredService<global::Cybtans.Tests.Services.IOrderService>();
					var result = await service.Get(request).ConfigureAwait(false);
					return result;
				}
			);
			
			#endregion OrderService
			
			#region OrderStateService
			
			Field<GetAllOrderStateResponseGraphType>("orderStates")
				.Description("Returns a collection of OrderStateDto")
				.Arguments(
					new QueryArgument<StringGraphType>(){ Name = "Filter" }
					,new QueryArgument<StringGraphType>(){ Name = "Sort" }
					,new QueryArgument<IntGraphType>(){ Name = "Skip" }
					,new QueryArgument<IntGraphType>(){ Name = "Take" }
				)
				.ResolveAsync(async context =>
				{
					var request = new GetAllRequest();
					request.Filter = context.GetArgument<string>("filter", default(string));
					request.Sort = context.GetArgument<string>("sort", default(string));
					request.Skip = context.GetArgument<int?>("skip", default(int?));
					request.Take = context.GetArgument<int?>("take", default(int?));
					
					using var scope = context.RequestServices.CreateScope();
					var serviceProvider = scope.ServiceProvider;
					
					var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
					if (!httpContext.User.IsInRole("admin"))
					{
						 throw new UnauthorizedAccessException("Roles Authorization Required");
					}
					
					var service = serviceProvider.GetRequiredService<global::Cybtans.Tests.Services.IOrderStateService>();
					var result = await service.GetAll(request).ConfigureAwait(false);
					return result;
				}
			);
			
			Field<OrderStateDtoGraphType>("orderState")
				.Description("Returns one OrderStateDto by Id")
				.Arguments(
					new QueryArgument<IntGraphType>(){ Name = "Id" }
				)
				.ResolveAsync(async context =>
				{
					var request = new GetOrderStateRequest();
					request.Id = context.GetArgument<int>("id", default(int));
					
					using var scope = context.RequestServices.CreateScope();
					var serviceProvider = scope.ServiceProvider;
					
					var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
					if (!httpContext.User.IsInRole("admin"))
					{
						 throw new UnauthorizedAccessException("Roles Authorization Required");
					}
					
					var service = serviceProvider.GetRequiredService<global::Cybtans.Tests.Services.IOrderStateService>();
					var result = await service.Get(request).ConfigureAwait(false);
					return result;
				}
			);
			
			#endregion OrderStateService
			
			#region ReadOnlyEntityService
			
			Field<GetAllReadOnlyEntityResponseGraphType>("readOnlyEntities")
				.Description("Returns a collection of ReadOnlyEntityDto")
				.Arguments(
					new QueryArgument<StringGraphType>(){ Name = "Filter" }
					,new QueryArgument<StringGraphType>(){ Name = "Sort" }
					,new QueryArgument<IntGraphType>(){ Name = "Skip" }
					,new QueryArgument<IntGraphType>(){ Name = "Take" }
				)
				.ResolveAsync(async context =>
				{
					var request = new GetAllRequest();
					request.Filter = context.GetArgument<string>("filter", default(string));
					request.Sort = context.GetArgument<string>("sort", default(string));
					request.Skip = context.GetArgument<int?>("skip", default(int?));
					request.Take = context.GetArgument<int?>("take", default(int?));
					
					using var scope = context.RequestServices.CreateScope();
					var serviceProvider = scope.ServiceProvider;
					
					var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
					if (!httpContext.User.IsInRole("admin"))
					{
						 throw new UnauthorizedAccessException("Roles Authorization Required");
					}
					
					var service = serviceProvider.GetRequiredService<global::Cybtans.Tests.Services.IReadOnlyEntityService>();
					var result = await service.GetAll(request).ConfigureAwait(false);
					return result;
				}
			);
			
			Field<ReadOnlyEntityDtoGraphType>("readOnlyEntity")
				.Description("Returns one ReadOnlyEntityDto by Id")
				.Arguments(
					new QueryArgument<IntGraphType>(){ Name = "Id" }
				)
				.ResolveAsync(async context =>
				{
					var request = new GetReadOnlyEntityRequest();
					request.Id = context.GetArgument<int>("id", default(int));
					
					using var scope = context.RequestServices.CreateScope();
					var serviceProvider = scope.ServiceProvider;
					
					var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
					if (!httpContext.User.IsInRole("admin"))
					{
						 throw new UnauthorizedAccessException("Roles Authorization Required");
					}
					
					var service = serviceProvider.GetRequiredService<global::Cybtans.Tests.Services.IReadOnlyEntityService>();
					var result = await service.Get(request).ConfigureAwait(false);
					return result;
				}
			);
			
			#endregion ReadOnlyEntityService
			
			#region SoftDeleteOrderService
			
			Field<GetAllSoftDeleteOrderResponseGraphType>("softDeleteOrders")
				.Description("Returns a collection of SoftDeleteOrderDto")
				.Arguments(
					new QueryArgument<StringGraphType>(){ Name = "Filter" }
					,new QueryArgument<StringGraphType>(){ Name = "Sort" }
					,new QueryArgument<IntGraphType>(){ Name = "Skip" }
					,new QueryArgument<IntGraphType>(){ Name = "Take" }
				)
				.ResolveAsync(async context =>
				{
					var request = new GetAllRequest();
					request.Filter = context.GetArgument<string>("filter", default(string));
					request.Sort = context.GetArgument<string>("sort", default(string));
					request.Skip = context.GetArgument<int?>("skip", default(int?));
					request.Take = context.GetArgument<int?>("take", default(int?));
					
					using var scope = context.RequestServices.CreateScope();
					var serviceProvider = scope.ServiceProvider;
					
					var service = serviceProvider.GetRequiredService<global::Cybtans.Tests.Services.ISoftDeleteOrderService>();
					var result = await service.GetAll(request).ConfigureAwait(false);
					return result;
				}
			);
			
			Field<SoftDeleteOrderDtoGraphType>("softDeleteOrder")
				.Description("Returns one SoftDeleteOrderDto by Id")
				.Arguments(
					new QueryArgument<GuidGraphType>(){ Name = "Id" }
				)
				.ResolveAsync(async context =>
				{
					var request = new GetSoftDeleteOrderRequest();
					request.Id = context.GetArgument<Guid>("id", default(Guid));
					
					using var scope = context.RequestServices.CreateScope();
					var serviceProvider = scope.ServiceProvider;
					
					var service = serviceProvider.GetRequiredService<global::Cybtans.Tests.Services.ISoftDeleteOrderService>();
					var result = await service.Get(request).ConfigureAwait(false);
					return result;
				}
			);
			
			#endregion SoftDeleteOrderService
			
			#region ClientService
			
			Field<ClientDtoGraphType>("ClientService_Client")
				.Arguments(
					new QueryArgument<NonNullGraphType<GuidGraphType>>(){ Name = "Id" }
				)
				.ResolveAsync(async context =>
				{
					var request = new ClientRequest();
					request.Id = context.GetArgument<Guid>("id", default(Guid));
					
					using var scope = context.RequestServices.CreateScope();
					var serviceProvider = scope.ServiceProvider;
					
					var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
					if (!httpContext.User.Identity.IsAuthenticated)
					{
						 throw new UnauthorizedAccessException("Authentication Required");
					}
					
					var authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();
					var policyResult = await authorizationService.AuthorizeAsync(httpContext.User, request, "ClientPolicy").ConfigureAwait(false);
					if (!policyResult.Succeeded)
					{
						 throw new UnauthorizedAccessException($"Request Authorization Failed: { string.Join(", ", policyResult.Failure.FailedRequirements) }");
					}
					
					var service = serviceProvider.GetRequiredService<global::Cybtans.Tests.Services.IClientService>();
					var result = await service.GetClient(request).ConfigureAwait(false);
					if (result != null)
					{
						policyResult = await authorizationService.AuthorizeAsync(httpContext.User, result, "ClientCreator").ConfigureAwait(false);
						if (!policyResult.Succeeded)
						{
							 throw new UnauthorizedAccessException($"Result Authorization Failed: { string.Join(", ", policyResult.Failure.FailedRequirements) }");
						}
					}
					
					return result;
				}
			);
			
			Field<ClientDtoGraphType>("ClientService_Client2")
				.Arguments(
					new QueryArgument<NonNullGraphType<GuidGraphType>>(){ Name = "Id" }
				)
				.ResolveAsync(async context =>
				{
					var request = new ClientRequest();
					request.Id = context.GetArgument<Guid>("id", default(Guid));
					
					using var scope = context.RequestServices.CreateScope();
					var serviceProvider = scope.ServiceProvider;
					
					var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
					if (!httpContext.User.Identity.IsAuthenticated)
					{
						 throw new UnauthorizedAccessException("Authentication Required");
					}
					
					var authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();
					var policyResult = await authorizationService.AuthorizeAsync(httpContext.User, request, "ClientPolicy").ConfigureAwait(false);
					if (!policyResult.Succeeded)
					{
						 throw new UnauthorizedAccessException($"Request Authorization Failed: { string.Join(", ", policyResult.Failure.FailedRequirements) }");
					}
					
					var service = serviceProvider.GetRequiredService<global::Cybtans.Tests.Services.IClientService>();
					var result = await service.GetClient2(request).ConfigureAwait(false);
					return result;
				}
			);
			
			Field<ClientDtoGraphType>("ClientService_Client3")
				.Arguments(
					new QueryArgument<NonNullGraphType<GuidGraphType>>(){ Name = "Id" }
				)
				.ResolveAsync(async context =>
				{
					var request = new ClientRequest();
					request.Id = context.GetArgument<Guid>("id", default(Guid));
					
					using var scope = context.RequestServices.CreateScope();
					var serviceProvider = scope.ServiceProvider;
					
					var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
					if (!httpContext.User.Identity.IsAuthenticated)
					{
						 throw new UnauthorizedAccessException("Authentication Required");
					}
					
					var service = serviceProvider.GetRequiredService<global::Cybtans.Tests.Services.IClientService>();
					var result = await service.GetClient3(request).ConfigureAwait(false);
					if (result != null)
					{
						var authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();
						var policyResult = await authorizationService.AuthorizeAsync(httpContext.User, result, "ClientCreator").ConfigureAwait(false);
						if (!policyResult.Succeeded)
						{
							 throw new UnauthorizedAccessException($"Result Authorization Failed: { string.Join(", ", policyResult.Failure.FailedRequirements) }");
						}
					}
					
					return result;
				}
			);
			
			#endregion ClientService
			
		
		}
	}
	

}
