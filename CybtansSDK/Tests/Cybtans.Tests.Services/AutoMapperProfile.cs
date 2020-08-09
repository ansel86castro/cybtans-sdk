
using System;
using AutoMapper;
using Cybtans.Test.Domain;
using Cybtans.Tests.Models;

namespace Cybtans.Tests.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Customer, CustomerEvent>()
                  .ForMember(x => x.FullName, o => o.MapFrom(x => $"{x.Name} {x.FirstLastName} {x.SecondLastName}"));
        
        }
    }
}