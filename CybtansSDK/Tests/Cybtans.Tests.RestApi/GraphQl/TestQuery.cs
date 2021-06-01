using Cybtans.Tests.Models;
using Cybtans.Tests.Services;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Cybtans.Tests.RestApi.GraphQl
{

    public class CustomerDtoGraphType : ObjectGraphType<CustomerDto>
    {
        public CustomerDtoGraphType()
        {
            Field(x => x.Name).Description("Customer's Name");
            Field(x => x.FirstLastName, nullable: true).Description("Customer's FirstLastName");
            Field(x => x.SecondLastName, nullable: true).Description("Customer's SecondLastName");
            Field(x => x.CustomerProfileId, nullable: true).Description("Customer's Profile Id, can be null");
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
            Field(x => x.Name, nullable: true);
            Field(x => x.Id);
            Field<DateTimeGraphType>("CreateDate");
            Field<DateTimeGraphType>("UpdateDate");

        }
    }

    public class OrderStateType : ObjectGraphType<OrderStateDto>
    {
        public OrderStateType()
        {
            Field(x => x.Id).Description("The Id of the Droid.");
            Field(x => x.Name).Description("The name of the Droid.");
            Field<StringGraphType>("type", resolve: context =>
            {
                return context.Source.Id == 1 ? "First" : "NotFirst";
            });
            Field<TimeSpanSecondsGraphType>("time", resolve: context =>
            {
                return TimeSpan.FromSeconds(120);
            });
        }
    }

    public class OrderType: ObjectGraphType<OrderDto>
    {
        public OrderType()
        {
            Field(x => x.Description, nullable: true);
            Field(x => x.CustomerId);
            Field(x => x.OrderStateId);
            //Field(x => x.OrderType);
            Field<OrderStateType>("OrderState");
            Field<CustomerDtoGraphType>("Customer");
            Field<ListGraphType<OrderItemDtoGraphType>>("Items");
            Field(x => x.Id);
            Field<DateTimeGraphType>("CreateDate");
            Field<DateTimeGraphType>("UpdateDate");
        }
    }

    public class OrderItemDtoGraphType : ObjectGraphType<OrderItemDto>
    {
        public OrderItemDtoGraphType()
        {
            Field(x => x.ProductName, nullable: true);
            Field(x => x.Price);
            Field(x => x.Discount, nullable: true);
            Field(x => x.OrderId);
            Field(x => x.ProductId, nullable: true);
           // Field(x => x.Product, nullable: true);
            Field(x => x.Id);

        }
    }

    public class GetAllOrderStateResponseType: ObjectGraphType<GetAllOrderStateResponse>
    {
        public GetAllOrderStateResponseType()
        {
            Field<ListGraphType<OrderStateType>>("Items",  description: "asdasd" );
            Field(x => x.Page);
            Field(x => x.TotalCount);
            Field(x => x.TotalPages);
        }
    }

    public class GetAllOrderResponseType : ObjectGraphType<GetAllOrderResponse>
    {
        public GetAllOrderResponseType()
        {
            Field<ListGraphType<OrderType>>("Items", description: "asdasd");
            Field(x => x.Page);
            Field(x => x.TotalCount);
            Field(x => x.TotalPages);
        }
    }

    public class Query : ObjectGraphType
    {
        public Query(IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService)
        {
            FieldAsync<OrderStateType>("orderState", 
                arguments: new QueryArguments()
                {
                    new QueryArgument<IntGraphType>(){ Name = "id", Description = "Type Ids" },
                },
                resolve: async context => 
                {                    
                    //if(!httpContextAccessor.HttpContext.User.IsInRole("admin"))
                    //{                        
                    //    throw new UnauthorizedAccessException();
                    //}
                    //if(!(await authorizationService.AuthorizeAsync(httpContextAccessor.HttpContext.User, "PolicyName")).Succeeded)
                    //{
                    //    throw new UnauthorizedAccessException();
                    //}

                    var service = context.RequestServices.GetRequiredService<IOrderStateService>();

                    var request = new GetOrderStateRequest();
                    request.Id = context.GetArgument<int>("id", default(int));

                    var item = await service.Get(request);
                    return item;
                });

            FieldAsync<GetAllOrderStateResponseType>("orderStates",
                resolve: async context => {                    
                    var service = context.RequestServices.GetRequiredService<IOrderStateService>();

                    var request = new GetAllRequest();

                    var items = await service.GetAll(request);
                    return items;
                });

            FieldAsync<GetAllOrderResponseType>("orders",
               resolve: async context => {
                   var service = context.RequestServices.GetRequiredService<IOrderService>();

                   var request = new GetAllRequest();

                   var items = await service.GetAll(request);
                   return items;
               });


        }
    }

    public class QuerySchema : Schema
    {
        public QuerySchema(IServiceProvider provider, IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService)
            : base(provider)
        {            
            Query = new Query(httpContextAccessor, authorizationService); 
        }
    }

}
