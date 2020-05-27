using System;
using Cybtans.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace Cybtans.Serialization.Tests.Models
{
	public partial class Product : IReflectorMetadataProvider
	{
		private static readonly ProductAccesor __accesor = new ProductAccesor();

		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string Description { get; set; }

		public float? Price { get; set; }

		public string PictureFileName { get; set; }

		public string PictureUrl { get; set; }

		public int BrandId { get; set; }

		public int CatalogId { get; set; }

		public int AvalaibleStock { get; set; }

		public int RestockThreshold { get; set; }

		public DateTime CreateDate { get; set; }

		public DateTime? UpdateDate { get; set; }

		public Catalog Catalog { get; set; }

		public Brand Brand { get; set; }

		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}

		public static List<Product> GetProducts()
		{
			List<Product> products = new List<Product>();

			var brands = Enumerable.Range(1, 5).Select(i => new Brand { Id = i, Name = $"Brand {i}" }).ToList();
			var catalogs = Enumerable.Range(1, 3).Select(i => new Models.Catalog { Id = i, Name = $"Catalog {i}" });

			var rand = new Random();
			foreach (var catalog in catalogs)
			{
				products.AddRange(Enumerable.Range(1, 10)
					 .Select(i => new Product
					 {
						 Brand = brands[i % 5],
						 Catalog = catalog,
						 AvalaibleStock = rand.Next(1000),
						 CreateDate = DateTime.Now,
						 Name = $"Product {catalog.Id}-{i}",
						 Price = i % 2 == 0 ? i : float.Parse($"{rand.Next(100, 500)}.{rand.Next(0, 100)}"),
						 RestockThreshold = 5,
						 Description = $"Description for Product {catalog.Id}{i}",
						 BrandId = brands[i % 5].Id,
						 CatalogId = catalog.Id
					 }));

			}

			return products;
		}
	}


	public sealed class ProductAccesor : IReflectorMetadata
	{
		public const int Id = 1;
		public const int Name = 2;
		public const int Description = 3;
		public const int Price = 4;
		public const int PictureFileName = 5;
		public const int PictureUrl = 6;
		public const int BrandId = 7;
		public const int CatalogId = 8;
		public const int AvalaibleStock = 9;
		public const int RestockThreshold = 10;
		public const int CreateDate = 11;
		public const int UpdateDate = 12;
		public const int Catalog = 13;
		public const int Brand = 14;
		private readonly int[] _props = new[]
		{
			Id,Name,Description,Price,PictureFileName,PictureUrl,BrandId,CatalogId,AvalaibleStock,RestockThreshold,CreateDate,UpdateDate,Catalog,Brand
		};

		public int[] GetPropertyCodes() => _props;

		public string GetPropertyName(int propertyCode)
		{
			return propertyCode switch
			{
				Id => "Id",
				Name => "Name",
				Description => "Description",
				Price => "Price",
				PictureFileName => "PictureFileName",
				PictureUrl => "PictureUrl",
				BrandId => "BrandId",
				CatalogId => "CatalogId",
				AvalaibleStock => "AvalaibleStock",
				RestockThreshold => "RestockThreshold",
				CreateDate => "CreateDate",
				UpdateDate => "UpdateDate",
				Catalog => "Catalog",
				Brand => "Brand",

				_ => throw new InvalidOperationException("property code not supported"),
			};
		}

		public int GetPropertyCode(string propertyName)
		{
			return propertyName switch
			{
				"Id" => Id,
				"Name" => Name,
				"Description" => Description,
				"Price" => Price,
				"PictureFileName" => PictureFileName,
				"PictureUrl" => PictureUrl,
				"BrandId" => BrandId,
				"CatalogId" => CatalogId,
				"AvalaibleStock" => AvalaibleStock,
				"RestockThreshold" => RestockThreshold,
				"CreateDate" => CreateDate,
				"UpdateDate" => UpdateDate,
				"Catalog" => Catalog,
				"Brand" => Brand,

				_ => -1,
			};
		}

		public Type GetPropertyType(int propertyCode)
		{
			return propertyCode switch
			{
				Id => typeof(int),
				Name => typeof(string),
				Description => typeof(string),
				Price => typeof(float?),
				PictureFileName => typeof(string),
				PictureUrl => typeof(string),
				BrandId => typeof(int),
				CatalogId => typeof(int),
				AvalaibleStock => typeof(int),
				RestockThreshold => typeof(int),
				CreateDate => typeof(DateTime),
				UpdateDate => typeof(DateTime?),
				Catalog => typeof(Catalog),
				Brand => typeof(Brand),

				_ => throw new InvalidOperationException("property code not supported"),
			};
		}

		public object GetValue(object target, int propertyCode)
		{
			Product obj = (Product)target;
			return propertyCode switch
			{
				Id => obj.Id,
				Name => obj.Name,
				Description => obj.Description,
				Price => obj.Price,
				PictureFileName => obj.PictureFileName,
				PictureUrl => obj.PictureUrl,
				BrandId => obj.BrandId,
				CatalogId => obj.CatalogId,
				AvalaibleStock => obj.AvalaibleStock,
				RestockThreshold => obj.RestockThreshold,
				CreateDate => obj.CreateDate,
				UpdateDate => obj.UpdateDate,
				Catalog => obj.Catalog,
				Brand => obj.Brand,

				_ => throw new InvalidOperationException("property code not supported"),
			};
		}

		public void SetValue(object target, int propertyCode, object value)
		{
			Product obj = (Product)target;
			switch (propertyCode)
			{
				case Id: obj.Id = (int)value; break;
				case Name: obj.Name = (string)value; break;
				case Description: obj.Description = (string)value; break;
				case Price: obj.Price = (float?)value; break;
				case PictureFileName: obj.PictureFileName = (string)value; break;
				case PictureUrl: obj.PictureUrl = (string)value; break;
				case BrandId: obj.BrandId = (int)value; break;
				case CatalogId: obj.CatalogId = (int)value; break;
				case AvalaibleStock: obj.AvalaibleStock = (int)value; break;
				case RestockThreshold: obj.RestockThreshold = (int)value; break;
				case CreateDate: obj.CreateDate = (DateTime)value; break;
				case UpdateDate: obj.UpdateDate = (DateTime?)value; break;
				case Catalog: obj.Catalog = (Catalog)value; break;
				case Brand: obj.Brand = (Brand)value; break;

				default: throw new InvalidOperationException("property code not supported");
			}
		}

	}

}
