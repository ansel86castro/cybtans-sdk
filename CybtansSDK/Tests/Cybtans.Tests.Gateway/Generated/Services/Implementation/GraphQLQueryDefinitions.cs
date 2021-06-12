
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
using Cybtans.Tests.Gateway.Models;

namespace Cybtans.Tests.Gateway.GraphQL
{
	public class HelloReplyGraphType : ObjectGraphType<HelloReply>
	{
		public HelloReplyGraphType()
		{
			Field(x => x.Message, nullable:true);
			Field<ListGraphType<StringGraphType>>("Keywords");
			Field<HellowInfoGraphType>("Info");
			Field<ListGraphType<HellowInfoGraphType>>("InfoArray");
			Field<DateTimeGraphType>("Date");
			Field<DateTimeGraphType>("Time");
			Field(x => x.Observations, nullable:true);
			Field(x => x.NullableInt, nullable:true);
			Field<HelloModelModelGraphType>("HelloModel");
		
		}
	}
	
	
	public class HellowInfoGraphType : ObjectGraphType<HellowInfo>
	{
		public HellowInfoGraphType()
		{
			Field(x => x.Id);
			Field(x => x.Name, nullable:true);
			Field<HellowInfo_Types_TypeInfoGraphType>("Type");
			Field<HellowInfo_Types_InnerAGraphType>("InnerA");
		
		}
	}
	
	
	public class HellowInfo_Types_TypeInfoGraphType : EnumerationGraphType<HellowInfo.Types.TypeInfo>
	{
	}
	
	
	public class HellowInfo_Types_InnerAGraphType : ObjectGraphType<HellowInfo.Types.InnerA>
	{
		public HellowInfo_Types_InnerAGraphType()
		{
			Field<HellowInfo_Types_InnerA_Types_InnerBGraphType>("B");
		
		}
	}
	
	
	public class HellowInfo_Types_InnerA_Types_InnerBGraphType : ObjectGraphType<HellowInfo.Types.InnerA.Types.InnerB>
	{
		public HellowInfo_Types_InnerA_Types_InnerBGraphType()
		{
			Field<HellowInfo_Types_InnerA_Types_InnerB_Types_TypeBGraphType>("Type");
		
		}
	}
	
	
	public class HellowInfo_Types_InnerA_Types_InnerB_Types_TypeBGraphType : EnumerationGraphType<HellowInfo.Types.InnerA.Types.InnerB.Types.TypeB>
	{
	}
	
	
	public class HelloModelModelGraphType : ObjectGraphType<HelloModelModel>
	{
		public HelloModelModelGraphType()
		{
			Field(x => x.Id, nullable:true);
			Field(x => x.Message, nullable:true);
		
		}
	}
	
	
	public partial class GraphQLQueryDefinitions : ObjectGraphType
	{
		public void AddGreetDefinitions()
		{
			#region Greeter
			
			FieldAsync<HelloReplyGraphType>("hello",
			 	arguments: new QueryArguments()
				{
					new QueryArgument<StringGraphType>(){ Name = "Name" },
					new QueryArgument<StringGraphType>(){ Name = "Observations" },
					new QueryArgument<DateTimeGraphType>(){ Name = "Date" },
					new QueryArgument<IntGraphType>(){ Name = "NullableInt" },
					new QueryArgument<TimeSpanSecondsGraphType>(){ Name = "Time" },
				},
				resolve: async context =>
				{
					var request = new HelloRequest();
					request.Name = context.GetArgument<string>("name", default(string));
					request.Observations = context.GetArgument<string>("observations", default(string));
					request.Date = context.GetArgument<DateTime?>("date", default(DateTime?));
					request.Data = context.GetArgument<byte[]>("data", default(byte[]));
					request.NullableInt = context.GetArgument<int?>("nullableInt", default(int?));
					request.Time = context.GetArgument<DateTime?>("time", default(DateTime?));
					
					var service = context.RequestServices.GetRequiredService<global::Cybtans.Test.Gateway.Services.Definition.IGreeter>();
					var result = await service.SayHello(request).ConfigureAwait(false);
					return result;
				}
			);
			
			#endregion Greeter
			
		
		}}
	

}
