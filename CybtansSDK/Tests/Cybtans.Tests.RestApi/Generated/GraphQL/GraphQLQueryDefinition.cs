
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
using Cybtans.Tests.Services;

namespace Cybtans.Tests.GraphQL
{
	public class GetAllCustomerResponseGraphType : ObjectGraphType<GetAllCustomerResponse>
	{
		public GetAllCustomerResponseGraphType()
		{
			Field<ListGraphType<CustomerDtoGraphType>>("Items");
			Field(x => x.Page);
			Field(x => x.TotalPages);
			Field(x => x.TotalCount);
		
		}
	}
	
	
	public class CustomerDtoGraphType : ObjectGraphType<CustomerDto>
	{
		public CustomerDtoGraphType()
		{
			Field(x => x.Name).Description("Customer's Name");
			Field(x => x.FirstLastName, nullable:true).Description("Customer's FirstLastName");
			Field(x => x.SecondLastName, nullable:true).Description("Customer's SecondLastName");
			Field(x => x.CustomerProfileId, nullable:true).Description("Customer's Profile Id, can be null");
			Field<CustomerProfileDtoGraphType>("CustomerProfile");
			Field(x => x.Id);
			Field<DateTimeGraphType>("CreateDate");
			Field<DateTimeGraphType>("UpdateDate");
		
		}
	}
	
	
	public class CustomerProfileDtoGraphType : ObjectGraphType<CustomerProfileDto>
	{
		public CustomerProfileDtoGraphType()
		{
			Field(x => x.Name, nullable:true);
			Field(x => x.Id);
			Field<DateTimeGraphType>("CreateDate");
			Field<DateTimeGraphType>("UpdateDate");
		
		}
	}
	
	
	public class GetAllCustomerEventResponseGraphType : ObjectGraphType<GetAllCustomerEventResponse>
	{
		public GetAllCustomerEventResponseGraphType()
		{
			Field<ListGraphType<CustomerEventDtoGraphType>>("Items");
			Field(x => x.Page);
			Field(x => x.TotalPages);
			Field(x => x.TotalCount);
		
		}
	}
	
	
	public class CustomerEventDtoGraphType : ObjectGraphType<CustomerEventDto>
	{
		public CustomerEventDtoGraphType()
		{
			Field(x => x.FullName, nullable:true);
			Field(x => x.CustomerProfileId, nullable:true);
			Field(x => x.Id);
		
		}
	}
	
	
	public class GetAllOrderResponseGraphType : ObjectGraphType<GetAllOrderResponse>
	{
		public GetAllOrderResponseGraphType()
		{
			Field<ListGraphType<OrderDtoGraphType>>("Items");
			Field(x => x.Page);
			Field(x => x.TotalPages);
			Field(x => x.TotalCount);
		
		}
	}
	
	
	public class OrderDtoGraphType : ObjectGraphType<OrderDto>
	{
		public OrderDtoGraphType()
		{
			Field(x => x.Description, nullable:true);
			Field(x => x.CustomerId);
			Field(x => x.OrderStateId);
			Field<OrderTypeEnumGraphType>("OrderType");
			Field<OrderStateDtoGraphType>("OrderState");
			Field<CustomerDtoGraphType>("Customer", description:"Customer");
			Field<ListGraphType<OrderItemDtoGraphType>>("Items");
			Field(x => x.Id);
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
			Field(x => x.Name, nullable:true);
			Field(x => x.Id);
		
		}
	}
	
	
	public class OrderItemDtoGraphType : ObjectGraphType<OrderItemDto>
	{
		public OrderItemDtoGraphType()
		{
			Field(x => x.ProductName, nullable:true);
			Field(x => x.Price);
			Field(x => x.Discount, nullable:true);
			Field(x => x.OrderId);
			Field(x => x.ProductId, nullable:true);
			Field<ProductDtoGraphType>("Product");
			Field(x => x.Id);
		
		}
	}
	
	
	public class ProductDtoGraphType : ObjectGraphType<ProductDto>
	{
		public ProductDtoGraphType()
		{
			Field(x => x.Name, nullable:true);
			Field(x => x.Model, nullable:true);
			Field(x => x.Id);
		
		}
	}
	
	
	public class GetAllOrderStateResponseGraphType : ObjectGraphType<GetAllOrderStateResponse>
	{
		public GetAllOrderStateResponseGraphType()
		{
			Field<ListGraphType<OrderStateDtoGraphType>>("Items");
			Field(x => x.Page);
			Field(x => x.TotalPages);
			Field(x => x.TotalCount);
		
		}
	}
	
	
	public class GetAllReadOnlyEntityResponseGraphType : ObjectGraphType<GetAllReadOnlyEntityResponse>
	{
		public GetAllReadOnlyEntityResponseGraphType()
		{
			Field<ListGraphType<ReadOnlyEntityDtoGraphType>>("Items");
			Field(x => x.Page);
			Field(x => x.TotalPages);
			Field(x => x.TotalCount);
		
		}
	}
	
	
	public class ReadOnlyEntityDtoGraphType : ObjectGraphType<ReadOnlyEntityDto>
	{
		public ReadOnlyEntityDtoGraphType()
		{
			Field(x => x.Name, nullable:true);
			Field<DateTimeGraphType>("CreateDate");
			Field<DateTimeGraphType>("UpdateDate");
			Field(x => x.Id);
		
		}
	}
	
	
	public class GetAllSoftDeleteOrderResponseGraphType : ObjectGraphType<GetAllSoftDeleteOrderResponse>
	{
		public GetAllSoftDeleteOrderResponseGraphType()
		{
			Field<ListGraphType<SoftDeleteOrderDtoGraphType>>("Items");
			Field(x => x.Page);
			Field(x => x.TotalPages);
			Field(x => x.TotalCount);
		
		}
	}
	
	
	public class SoftDeleteOrderDtoGraphType : ObjectGraphType<SoftDeleteOrderDto>
	{
		public SoftDeleteOrderDtoGraphType()
		{
			Field(x => x.Name, nullable:true);
			Field(x => x.IsDeleted);
			Field<ListGraphType<SoftDeleteOrderItemDtoGraphType>>("Items");
			Field(x => x.Id);
			Field<DateTimeGraphType>("CreateDate");
			Field<DateTimeGraphType>("UpdateDate");
		
		}
	}
	
	
	public class SoftDeleteOrderItemDtoGraphType : ObjectGraphType<SoftDeleteOrderItemDto>
	{
		public SoftDeleteOrderItemDtoGraphType()
		{
			Field(x => x.Name, nullable:true);
			Field(x => x.IsDeleted);
			Field(x => x.SoftDeleteOrderId);
			Field(x => x.Id);
			Field<DateTimeGraphType>("CreateDate");
			Field<DateTimeGraphType>("UpdateDate");
		
		}
	}
	
	
	public class DowndloadImageResponseGraphType : ObjectGraphType<DowndloadImageResponse>
	{
		public DowndloadImageResponseGraphType()
		{
			Field(x => x.FileName, nullable:true);
			Field(x => x.ContentType, nullable:true);
		
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
			Field(x => x.Id, nullable:true);
			Field(x => x.Description, nullable:true);
		
		}
	}
	
	
	public class GraphQLQueryDefinition : ObjectGraphType
	{
		public GraphQLQueryDefinition(IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService)
		{
			#region CustomerService
			
			FieldAsync<GetAllCustomerResponseGraphType>("customers",
			 	description: "Returns a collection of CustomerDto",
				arguments: new QueryArguments()
				{
					new QueryArgument<StringGraphType>(){ Name = "Filter" },
					new QueryArgument<StringGraphType>(){ Name = "Sort" },
					new QueryArgument<IntGraphType>(){ Name = "Skip" },
					new QueryArgument<IntGraphType>(){ Name = "Take" },
				},
				resolve: async context =>
				{
					var request = new GetAllRequest();
					request.Filter = context.GetArgument<string>("filter", default(string));
					request.Sort = context.GetArgument<string>("sort", default(string));
					request.Skip = context.GetArgument<int?>("skip", default(int?));
					request.Take = context.GetArgument<int?>("take", default(int?));
					
					var service = context.RequestServices.GetRequiredService<ICustomerService>();
					return await service.GetAll(request);
				}
			);
			
			FieldAsync<CustomerDtoGraphType>("customer",
			 	description: "Returns one CustomerDto by Id",
				arguments: new QueryArguments()
				{
					new QueryArgument<GuidGraphType>(){ Name = "Id" },
				},
				resolve: async context =>
				{
					var request = new GetCustomerRequest();
					request.Id = context.GetArgument<Guid>("id", default(Guid));
					
					var service = context.RequestServices.GetRequiredService<ICustomerService>();
					return await service.Get(request);
				}
			);
			
			#endregion CustomerService
			
			#region CustomerEventService
			
			FieldAsync<GetAllCustomerEventResponseGraphType>("customerEvents",
			 	description: "Returns a collection of CustomerEventDto",
				arguments: new QueryArguments()
				{
					new QueryArgument<StringGraphType>(){ Name = "Filter" },
					new QueryArgument<StringGraphType>(){ Name = "Sort" },
					new QueryArgument<IntGraphType>(){ Name = "Skip" },
					new QueryArgument<IntGraphType>(){ Name = "Take" },
				},
				resolve: async context =>
				{
					var request = new GetAllRequest();
					request.Filter = context.GetArgument<string>("filter", default(string));
					request.Sort = context.GetArgument<string>("sort", default(string));
					request.Skip = context.GetArgument<int?>("skip", default(int?));
					request.Take = context.GetArgument<int?>("take", default(int?));
					
					var service = context.RequestServices.GetRequiredService<ICustomerEventService>();
					return await service.GetAll(request);
				}
			);
			
			FieldAsync<CustomerEventDtoGraphType>("customerEvent",
			 	description: "Returns one CustomerEventDto by Id",
				arguments: new QueryArguments()
				{
					new QueryArgument<GuidGraphType>(){ Name = "Id" },
				},
				resolve: async context =>
				{
					var request = new GetCustomerEventRequest();
					request.Id = context.GetArgument<Guid>("id", default(Guid));
					
					var service = context.RequestServices.GetRequiredService<ICustomerEventService>();
					return await service.Get(request);
				}
			);
			
			#endregion CustomerEventService
			
			#region OrderService
			
			FieldAsync<DowndloadImageResponseGraphType>("OrderService_DownloadImage",
			 	arguments: new QueryArguments()
				{
					new QueryArgument<StringGraphType>(){ Name = "Name" },
				},
				resolve: async context =>
				{
					var request = new DownloadImageRequest();
					request.Name = context.GetArgument<string>("name", default(string));
					
					var service = context.RequestServices.GetRequiredService<IOrderService>();
					return await service.DownloadImage(request);
				}
			);
			
			FieldAsync<GetAllNamesResponseGraphType>("ordernames",
			 	resolve: async context =>
				{
					var service = context.RequestServices.GetRequiredService<IOrderService>();
					return await service.GetAllNames();
				}
			);
			
			FieldAsync<OrderNamesDtoGraphType>("OrderService_OrderName",
			 	arguments: new QueryArguments()
				{
					new QueryArgument<StringGraphType>(){ Name = "Id" },
				},
				resolve: async context =>
				{
					var request = new GetOrderNameRequest();
					request.Id = context.GetArgument<string>("id", default(string));
					
					var service = context.RequestServices.GetRequiredService<IOrderService>();
					return await service.GetOrderName(request);
				}
			);
			
			FieldAsync<GetAllOrderResponseGraphType>("orders",
			 	description: "Returns a collection of OrderDto",
				arguments: new QueryArguments()
				{
					new QueryArgument<StringGraphType>(){ Name = "Filter" },
					new QueryArgument<StringGraphType>(){ Name = "Sort" },
					new QueryArgument<IntGraphType>(){ Name = "Skip" },
					new QueryArgument<IntGraphType>(){ Name = "Take" },
				},
				resolve: async context =>
				{
					var request = new GetAllRequest();
					request.Filter = context.GetArgument<string>("filter", default(string));
					request.Sort = context.GetArgument<string>("sort", default(string));
					request.Skip = context.GetArgument<int?>("skip", default(int?));
					request.Take = context.GetArgument<int?>("take", default(int?));
					
					var service = context.RequestServices.GetRequiredService<IOrderService>();
					return await service.GetAll(request);
				}
			);
			
			FieldAsync<OrderDtoGraphType>("order",
			 	description: "Returns one OrderDto by Id",
				arguments: new QueryArguments()
				{
					new QueryArgument<GuidGraphType>(){ Name = "Id" },
				},
				resolve: async context =>
				{
					var request = new GetOrderRequest();
					request.Id = context.GetArgument<Guid>("id", default(Guid));
					
					var service = context.RequestServices.GetRequiredService<IOrderService>();
					return await service.Get(request);
				}
			);
			
			#endregion OrderService
			
			#region OrderStateService
			
			FieldAsync<GetAllOrderStateResponseGraphType>("orderStates",
			 	description: "Returns a collection of OrderStateDto",
				arguments: new QueryArguments()
				{
					new QueryArgument<StringGraphType>(){ Name = "Filter" },
					new QueryArgument<StringGraphType>(){ Name = "Sort" },
					new QueryArgument<IntGraphType>(){ Name = "Skip" },
					new QueryArgument<IntGraphType>(){ Name = "Take" },
				},
				resolve: async context =>
				{
					var request = new GetAllRequest();
					request.Filter = context.GetArgument<string>("filter", default(string));
					request.Sort = context.GetArgument<string>("sort", default(string));
					request.Skip = context.GetArgument<int?>("skip", default(int?));
					request.Take = context.GetArgument<int?>("take", default(int?));
					
					var service = context.RequestServices.GetRequiredService<IOrderStateService>();
					return await service.GetAll(request);
				}
			);
			
			FieldAsync<OrderStateDtoGraphType>("orderState",
			 	description: "Returns one OrderStateDto by Id",
				arguments: new QueryArguments()
				{
					new QueryArgument<IntGraphType>(){ Name = "Id" },
				},
				resolve: async context =>
				{
					var request = new GetOrderStateRequest();
					request.Id = context.GetArgument<int>("id", default(int));
					
					var service = context.RequestServices.GetRequiredService<IOrderStateService>();
					return await service.Get(request);
				}
			);
			
			#endregion OrderStateService
			
			#region ReadOnlyEntityService
			
			FieldAsync<GetAllReadOnlyEntityResponseGraphType>("readOnlyEntities",
			 	description: "Returns a collection of ReadOnlyEntityDto",
				arguments: new QueryArguments()
				{
					new QueryArgument<StringGraphType>(){ Name = "Filter" },
					new QueryArgument<StringGraphType>(){ Name = "Sort" },
					new QueryArgument<IntGraphType>(){ Name = "Skip" },
					new QueryArgument<IntGraphType>(){ Name = "Take" },
				},
				resolve: async context =>
				{
					var request = new GetAllRequest();
					request.Filter = context.GetArgument<string>("filter", default(string));
					request.Sort = context.GetArgument<string>("sort", default(string));
					request.Skip = context.GetArgument<int?>("skip", default(int?));
					request.Take = context.GetArgument<int?>("take", default(int?));
					
					var service = context.RequestServices.GetRequiredService<IReadOnlyEntityService>();
					return await service.GetAll(request);
				}
			);
			
			FieldAsync<ReadOnlyEntityDtoGraphType>("readOnlyEntity",
			 	description: "Returns one ReadOnlyEntityDto by Id",
				arguments: new QueryArguments()
				{
					new QueryArgument<IntGraphType>(){ Name = "Id" },
				},
				resolve: async context =>
				{
					var request = new GetReadOnlyEntityRequest();
					request.Id = context.GetArgument<int>("id", default(int));
					
					var service = context.RequestServices.GetRequiredService<IReadOnlyEntityService>();
					return await service.Get(request);
				}
			);
			
			#endregion ReadOnlyEntityService
			
			#region SoftDeleteOrderService
			
			FieldAsync<GetAllSoftDeleteOrderResponseGraphType>("softDeleteOrders",
			 	description: "Returns a collection of SoftDeleteOrderDto",
				arguments: new QueryArguments()
				{
					new QueryArgument<StringGraphType>(){ Name = "Filter" },
					new QueryArgument<StringGraphType>(){ Name = "Sort" },
					new QueryArgument<IntGraphType>(){ Name = "Skip" },
					new QueryArgument<IntGraphType>(){ Name = "Take" },
				},
				resolve: async context =>
				{
					var request = new GetAllRequest();
					request.Filter = context.GetArgument<string>("filter", default(string));
					request.Sort = context.GetArgument<string>("sort", default(string));
					request.Skip = context.GetArgument<int?>("skip", default(int?));
					request.Take = context.GetArgument<int?>("take", default(int?));
					
					var service = context.RequestServices.GetRequiredService<ISoftDeleteOrderService>();
					return await service.GetAll(request);
				}
			);
			
			FieldAsync<SoftDeleteOrderDtoGraphType>("softDeleteOrder",
			 	description: "Returns one SoftDeleteOrderDto by Id",
				arguments: new QueryArguments()
				{
					new QueryArgument<GuidGraphType>(){ Name = "Id" },
				},
				resolve: async context =>
				{
					var request = new GetSoftDeleteOrderRequest();
					request.Id = context.GetArgument<Guid>("id", default(Guid));
					
					var service = context.RequestServices.GetRequiredService<ISoftDeleteOrderService>();
					return await service.Get(request);
				}
			);
			
			#endregion SoftDeleteOrderService
			
		
		}}
	
	
	public class GraphQLQueryDefinitionSchema : Schema
	{
	    public GraphQLQueryDefinitionSchema(IServiceProvider provider, IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService)
	        : base(provider)
	    {            
	        Query = new GraphQLQueryDefinition(httpContextAccessor, authorizationService); 
	    }
	}

}
