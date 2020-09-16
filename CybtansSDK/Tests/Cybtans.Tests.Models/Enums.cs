using System;
using System.ComponentModel;

namespace Cybtans.Tests.Models
{
	/// <summary>
	/// Enum Type Description
	/// </summary>
	[Description("Enum Type Description")]
	public enum OrderTypeEnum 
	{
		/// <summary>
		/// Default
		/// </summary>
		[Description("Default")]
		Default = 0,
		
		/// <summary>
		/// Normal
		/// </summary>
		[Description("Normal")]
		Normal = 1,
		
		/// <summary>
		/// Shipping
		/// </summary>
		[Description("Shipping")]
		Shipping = 2,
		
	}

}
