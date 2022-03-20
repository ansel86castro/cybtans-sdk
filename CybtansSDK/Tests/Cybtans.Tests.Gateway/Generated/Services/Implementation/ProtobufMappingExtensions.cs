
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   Powered By Cybtans    
// </auto-generated>
//******************************************************

using System;
using System.Linq;

using mds = global::Cybtans.Tests.Gateway.Models;

namespace Cybtans.Test.Gateway.Services.Implementation
{
	public static class ProtobufMappingExtensions
	{
		public static global::Cybtans.Tests.Grpc.HelloRequest ToProtobufModel(this mds::HelloRequest model)
		{
			if(model == null) return null;
			
			global::Cybtans.Tests.Grpc.HelloRequest result = new global::Cybtans.Tests.Grpc.HelloRequest();
			result.Name = model.Name ?? string.Empty;
			result.Observations = model.Observations;
			result.Date = model.Date.HasValue ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.SpecifyKind(model.Date.Value, DateTimeKind.Utc)) : null;
			result.Data = model.Data != null ? Google.Protobuf.ByteString.CopyFrom(model.Data) : Google.Protobuf.ByteString.Empty;
			result.NullableInt = model.NullableInt;
			result.Time = model.Time.HasValue ? Google.Protobuf.WellKnownTypes.Duration.FromTimeSpan(model.Time.Value.TimeOfDay) : null;
			return result;
		}
		
		public static mds::HelloReply ToPocoModel(this global::Cybtans.Tests.Grpc.HelloReply model)
		{
			if(model == null) return null;
			
			mds::HelloReply result = new mds::HelloReply();
			result.Message = model.Message;
			result.Keywords = model.Keywords.ToList();
			result.Info = ToPocoModel(model.Info);
			result.info_array = model.InfoArray.Select(x => ToPocoModel(x)).ToList();
			result.Date = model.Date?.ToDateTime();
			result.Time = model.Time != null ? DateTime.UnixEpoch.Add(model.Time.ToTimeSpan()) : new DateTime?();
			result.Observations = model.Observations;
			result.NullableInt = model.NullableInt;
			result.Data = model.Data?.ToByteArray();
			result.HelloModel = ToPocoModel(model.HelloModel);
			return result;
		}
		
		public static mds::HellowInfo ToPocoModel(this global::Cybtans.Tests.Grpc.HellowInfo model)
		{
			if(model == null) return null;
			
			mds::HellowInfo result = new mds::HellowInfo();
			result.Id = model.Id;
			result.Name = model.Name;
			result.Type = (mds::HellowInfo.Types.TypeInfo)model.Type;
			result.InnerA = ToPocoModel(model.InnerA);
			return result;
		}
		
		public static mds::HellowInfo.Types.InnerA ToPocoModel(this global::Cybtans.Tests.Grpc.HellowInfo.Types.InnerA model)
		{
			if(model == null) return null;
			
			mds::HellowInfo.Types.InnerA result = new mds::HellowInfo.Types.InnerA();
			result.B = ToPocoModel(model.B);
			return result;
		}
		
		public static mds::HellowInfo.Types.InnerA.Types.InnerB ToPocoModel(this global::Cybtans.Tests.Grpc.HellowInfo.Types.InnerA.Types.InnerB model)
		{
			if(model == null) return null;
			
			mds::HellowInfo.Types.InnerA.Types.InnerB result = new mds::HellowInfo.Types.InnerA.Types.InnerB();
			result.Type = (mds::HellowInfo.Types.InnerA.Types.InnerB.Types.TypeB)model.Type;
			return result;
		}
		
		public static mds::HelloModelModel ToPocoModel(this global::Cybtans.Tests.Grpc.HelloModelModel model)
		{
			if(model == null) return null;
			
			mds::HelloModelModel result = new mds::HelloModelModel();
			result.Id = model.Id;
			result.Message = model.Message;
			return result;
		}
		
	}

}
