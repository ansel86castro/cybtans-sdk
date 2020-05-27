using System;
using Cybtans.Serialization;

namespace Catalog.Models
{
	public partial class Comment : IReflectorMetadataProvider
	{
		private static readonly CommentAccesor __accesor = new CommentAccesor();
		
		public int Id {get; set;}
		
		public string Text {get; set;}
		
		public string Username {get; set;}
		
		public int UserId {get; set;}
		
		public DateTime Date {get; set;}
		
		public byte Rating {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class CommentAccesor : IReflectorMetadata
	{
		public const int Id = 1;
		public const int Text = 2;
		public const int Username = 3;
		public const int UserId = 4;
		public const int Date = 5;
		public const int Rating = 6;
		private readonly int[] _props = new []
		{
			Id,Text,Username,UserId,Date,Rating
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Id => "Id",
		       Text => "Text",
		       Username => "Username",
		       UserId => "UserId",
		       Date => "Date",
		       Rating => "Rating",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Id" => Id,
		        "Text" => Text,
		        "Username" => Username,
		        "UserId" => UserId,
		        "Date" => Date,
		        "Rating" => Rating,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Id => typeof(int),
		        Text => typeof(string),
		        Username => typeof(string),
		        UserId => typeof(int),
		        Date => typeof(DateTime),
		        Rating => typeof(byte),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    Comment obj = (Comment)target;
		    return propertyCode switch
		    {
		        Id => obj.Id,
		        Text => obj.Text,
		        Username => obj.Username,
		        UserId => obj.UserId,
		        Date => obj.Date,
		        Rating => obj.Rating,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    Comment obj = (Comment)target;
		    switch (propertyCode)
		    {
		        case Id:  obj.Id = (int)value;break;
		        case Text:  obj.Text = (string)value;break;
		        case Username:  obj.Username = (string)value;break;
		        case UserId:  obj.UserId = (int)value;break;
		        case Date:  obj.Date = (DateTime)value;break;
		        case Rating:  obj.Rating = (byte)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
