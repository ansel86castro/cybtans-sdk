using System;
using Cybtans.Serialization;
using System.ComponentModel;

namespace Cybtans.Tests.Models
{
	/// <summary>
	/// Authentication response
	/// </summary>
	[Description("Authentication response")]
	public class LoginResponse : IReflectorMetadataProvider
	{
		private static readonly LoginResponseAccesor __accesor = new LoginResponseAccesor();
		
		/// <summary>
		/// Jwt Access Token
		/// </summary>
		[Description("Jwt Access Token")]
		public string Token {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
		
		public static implicit operator LoginResponse(string token)
		{
			return new LoginResponse { Token = token };
		}
	}
	
	
	internal sealed class LoginResponseAccesor : IReflectorMetadata
	{
		public const int Token = 1;
		private readonly int[] _props = new []
		{
			Token
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Token => "Token",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Token" => Token,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Token => typeof(string),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    LoginResponse obj = (LoginResponse)target;
		    return propertyCode switch
		    {
		        Token => obj.Token,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    LoginResponse obj = (LoginResponse)target;
		    switch (propertyCode)
		    {
		        case Token:  obj.Token = (string)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
