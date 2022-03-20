
using System;
using AutoMapper;
using Cybtans.Tests.Models;

namespace Cybtans.Tests.Services
{
    public class GeneratedAutoMapperProfile:Profile
    {
        public GeneratedAutoMapperProfile()
        {
           
           CreateMap<global::Cybtans.Tests.Domain.Customer, CustomerDto>();
           CreateMap<CustomerDto, global::Cybtans.Tests.Domain.Customer>();
           
           CreateMap<global::Cybtans.Tests.Domain.CustomerProfile, CustomerProfileDto>();
           CreateMap<CustomerProfileDto, global::Cybtans.Tests.Domain.CustomerProfile>();
           
           CreateMap<global::Cybtans.Tests.Domain.CustomerEvent, CustomerEventDto>();
           CreateMap<CustomerEventDto, global::Cybtans.Tests.Domain.CustomerEvent>();
           
           CreateMap<global::Cybtans.Tests.Domain.OrderItem, OrderItemDto>();
           CreateMap<OrderItemDto, global::Cybtans.Tests.Domain.OrderItem>();
           
           CreateMap<global::Cybtans.Tests.Domain.Product, ProductDto>();
           CreateMap<ProductDto, global::Cybtans.Tests.Domain.Product>();
           
           CreateMap<global::Cybtans.Tests.Domain.Order, OrderDto>();
           CreateMap<OrderDto, global::Cybtans.Tests.Domain.Order>();
           
           CreateMap<global::Cybtans.Tests.Domain.OrderState, OrderStateDto>();
           CreateMap<OrderStateDto, global::Cybtans.Tests.Domain.OrderState>();
           
           CreateMap<global::Cybtans.Tests.Domain.ReadOnlyEntity, ReadOnlyEntityDto>();
           CreateMap<ReadOnlyEntityDto, global::Cybtans.Tests.Domain.ReadOnlyEntity>();
           
           CreateMap<global::Cybtans.Tests.Domain.SoftDeleteOrder, SoftDeleteOrderDto>();
           CreateMap<SoftDeleteOrderDto, global::Cybtans.Tests.Domain.SoftDeleteOrder>();
           
           CreateMap<global::Cybtans.Tests.Domain.SoftDeleteOrderItem, SoftDeleteOrderItemDto>();
           CreateMap<SoftDeleteOrderItemDto, global::Cybtans.Tests.Domain.SoftDeleteOrderItem>();
        
        }
    }
}