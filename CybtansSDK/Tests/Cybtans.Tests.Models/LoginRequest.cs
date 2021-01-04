using System;
using Cybtans.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cybtans.Tests.Models
{
	/// <summary>
	/// Authentication Request
	/// </summary>
	[Description("Authentication Request")]
	public partial class LoginRequest : IReflectorMetadataProvider
	{
		private static readonly LoginRequestAccesor __accesor = new LoginRequestAccesor();
		
		/// <summary>
		/// The username
		/// </summary>
		[Required]
		[Description("The username")]
		public string Username {get; set;}
		
		/// <summary>
		/// The password
		/// </summary>
		[Required]
		[Description("The password")]
		public string Password {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class LoginRequestAccesor : IReflectorMetadata
	{
		public const int Username = 1;
		public const int Password = 2;
		private readonly int[] _props = new []
		{
			Username,Password
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Username => "Username",
		       Password => "Password",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Username" => Username,
		        "Password" => Password,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Username => typeof(string),
		        Password => typeof(string),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    LoginRequest obj = (LoginRequest)target;
		    return propertyCode switch
		    {
		        Username => obj.Username,
		        Password => obj.Password,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    LoginRequest obj = (LoginRequest)target;
		    switch (propertyCode)
		    {
		        case Username:  obj.Username = (string)value;break;
		        case Password:  obj.Password = (string)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
