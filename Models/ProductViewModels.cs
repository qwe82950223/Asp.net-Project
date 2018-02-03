using Supreme.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Supreme.Models
{
    public class ProductViewModels
    {
        public ProductViewModels()
        {
        }

        public ProductViewModels(Product row)
        {
            Id = row.Id;
            ProductName = row.ProductName;
            Description = row.Description;
            Price = row.Price;
            ProductType = row.ProductType;
            ClothSizeId = row.ClothSizeId;
            ShoesSizeId = row.ShoesSizeId;
            BrandId = row.BrandId;
            CategoryId = row.CategoryId;
            ImageName = row.ImageName;
        }

        public int Id { get; set; }      
        public string ProductName { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string ProductType { get; set; }
        public int ClothSizeId { get; set; }
        public int ShoesSizeId { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public string ImageName { get; set; }

        public ClothSizeTable ClothSizeList { get; set; }
        public ShoesSizeTable ShoesSizeList { get; set; }

        public IEnumerable<SelectListItem> BrandsList { get; set; }
        public IEnumerable<SelectListItem> CategoriesList { get; set; }
        public IEnumerable<string> GalleryImages { get; set; }

        public string BrandName { get; set; }
        public string CategoryName { get; set; }
    }
}