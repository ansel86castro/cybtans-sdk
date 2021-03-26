
using System;
using AutoMapper;
using Cybtans.Tests.Domain;
using Cybtans.Tests.Models;

namespace Cybtans.Tests.Services
{
    public class GeneratedAutoMapperProfile:Profile
    {
        public GeneratedAutoMapperProfile()
        {
           
           CreateMap<Customer, CustomerDto>();
           CreateMap<CustomerDto,Customer>();
           
           CreateMap<CustomerProfile, CustomerProfileDto>();
           CreateMap<CustomerProfileDto,CustomerProfile>();
           
           CreateMap<Order, OrderDto>();
           CreateMap<OrderDto,Order>();
           
           CreateMap<OrderState, OrderStateDto>();
           CreateMap<OrderStateDto,OrderState>();
           
           CreateMap<OrderItem, OrderItemDto>();
           CreateMap<OrderItemDto,OrderItem>();
           
           CreateMap<Product, ProductDto>();
           CreateMap<ProductDto,Product>();
           
           CreateMap<CustomerEvent, CustomerEventDto>();
           CreateMap<CustomerEventDto,CustomerEvent>();
           
           CreateMap<ReadOnlyEntity, ReadOnlyEntityDto>();
           CreateMap<ReadOnlyEntityDto,ReadOnlyEntity>();
           
           CreateMap<SoftDeleteOrder, SoftDeleteOrderDto>();
           CreateMap<SoftDeleteOrderDto,SoftDeleteOrder>();
           
           CreateMap<SoftDeleteOrderItem, SoftDeleteOrderItemDto>();
           CreateMap<SoftDeleteOrderItemDto,SoftDeleteOrderItem>();
        
        }
    }
}