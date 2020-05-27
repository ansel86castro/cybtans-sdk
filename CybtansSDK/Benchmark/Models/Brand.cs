using System;
using Cybtans.Serialization;

namespace Cybtans.Serialization.Tests.Models
{
	public partial class Brand : IReflectorMetadataProvider
	{
		private static readonly BrandAccesor __accesor = new BrandAccesor();

		public int Id { get; set; }

		public string Name { get; set; }

		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}


	public sealed class BrandAccesor : IReflectorMetadata
	{
		public const int Id = 1;
		public const int Name = 2;
		private readonly int[] _props = new[]
		{
			Id,Name
		};

		public int[] GetPropertyCodes() => _props;

		public string GetPropertyName(int propertyCode)
		{
			return propertyCode switch
			{
				Id => "Id",
				Name => "Name",

				_ => throw new InvalidOperationException("property code not supported"),
			};
		}

		public int GetPropertyCode(string propertyName)
		{
			return propertyName switch
			{
				"Id" => Id,
				"Name" => Name,

				_ => -1,
			};
		}

		public Type GetPropertyType(int propertyCode)
		{
			return propertyCode switch
			{
				Id => typeof(int),
				Name => typeof(string),

				_ => throw new InvalidOperationException("property code not supported"),
			};
		}

		public object GetValue(object target, int propertyCode)
		{
			Brand obj = (Brand)target;
			return propertyCode switch
			{
				Id => obj.Id,
				Name => obj.Name,

				_ => throw new InvalidOperationException("property code not supported"),
			};
		}

		public void SetValue(object target, int propertyCode, object value)
		{
			Brand obj = (Brand)target;
			switch (propertyCode)
			{
				case Id: obj.Id = (int)value; break;
				case Name: obj.Name = (string)value; break;

				default: throw new InvalidOperationException("property code not supported");
			}
		}

	}

}
