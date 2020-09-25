using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public partial class MultiPathRequest : IReflectorMetadataProvider
	{
		private static readonly MultiPathRequestAccesor __accesor = new MultiPathRequestAccesor();
		
		public string Param1 {get; set;}
		
		public string Param2 {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class MultiPathRequestAccesor : IReflectorMetadata
	{
		public const int Param1 = 1;
		public const int Param2 = 2;
		private readonly int[] _props = new []
		{
			Param1,Param2
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Param1 => "Param1",
		       Param2 => "Param2",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Param1" => Param1,
		        "Param2" => Param2,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Param1 => typeof(string),
		        Param2 => typeof(string),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    MultiPathRequest obj = (MultiPathRequest)target;
		    return propertyCode switch
		    {
		        Param1 => obj.Param1,
		        Param2 => obj.Param2,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    MultiPathRequest obj = (MultiPathRequest)target;
		    switch (propertyCode)
		    {
		        case Param1:  obj.Param1 = (string)value;break;
		        case Param2:  obj.Param2 = (string)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
