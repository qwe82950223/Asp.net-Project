using PagedList;
using Supreme.Entities;
using Supreme.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Supreme.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products/ProductsList
        public ActionResult ProductsList(int? page)
        {
            List<ProductViewModels> productVMList;

            var pageNumber = page ?? 1;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                productVMList = db.Products.ToArray().Select(x => new ProductViewModels
                {
                    Id = x.Id,
                    ProductName = x.ProductName,
                    Description = x.Description,
                    Price = x.Price,
                    ImageName = x.ImageName,
                    BrandId = x.BrandId,
                    CategoryId = x.CategoryId
                }).ToList();
            }
            var onePageOfProducts = productVMList.ToPagedList(pageNumber, 6);
            ViewBag.OnePageOfProducts = onePageOfProducts;

            return View(onePageOfProducts);
        }

        // Get: Products/SingleProduct
        public ActionResult SingleProduct(int id)
        {
            ProductViewModels productVM = new ProductViewModels();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Product product = db.Products.Find(id);
                productVM.Id = product.Id;
                productVM.ProductName = product.ProductName;
                productVM.Description = product.Description;
                productVM.Price = product.Price;
                productVM.ProductType = product.ProductType;
                if(product.ProductType == "0")
                {
                    productVM.ClothSizeId = product.ClothSizeId;
                    productVM.ClothSizeList = db.ClothSizes.Find(product.ClothSizeId);
                }
                else {
                    productVM.ShoesSizeId = product.ShoesSizeId;
                    productVM.ShoesSizeList = db.ShoesSizes.Find(product.ShoesSizeId);
                }
                productVM.ImageName = product.ImageName;
                productVM.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + product.Id + "/Gallery/Thumbs")).Select(fn => Path.GetFileName(fn));
                productVM.BrandName = db.Brands.ToList().First(b => b.Id.Equals(product.BrandId)).Name;
                productVM.CategoryName = db.Categories.ToList().First(b => b.Id.Equals(product.CategoryId)).Name;
            }
            return View(productVM);
        }
    }
}