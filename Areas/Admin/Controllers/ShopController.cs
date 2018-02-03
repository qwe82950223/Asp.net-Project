using PagedList;
using Supreme.Entities;
using Supreme.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Supreme.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        ///////////////////////////////////////////////////////////////////////////////////

        /*
        * Brand
        */

        // GET: Admin/Shop/Brands
        [Authorize(Roles ="Admin")]
        public ActionResult Brands()
        {
            //declare a list of models
            List<BrandViewModels> brandVMList;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                brandVMList = db.Brands.ToArray().Select(x => new BrandViewModels(x)).ToList();
            }
            return View(brandVMList);
        }

        //POST: Admin/Shop/CreateBrand
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public string CreateBrand(string bName)
        {
            string id;
            //declare id

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //check is name unique
                if (db.Brands.Any(x => x.Name == bName))
                {
                    return "titletaken";
                }
                //init brand table
                Brand brand = new Brand();
                brand.Name = bName;

                //add to brand table
                db.Brands.Add(brand);
                //save table
                db.SaveChanges();
                //get the id
                id = brand.Id.ToString();

            }

            return id;
        }

        //POST: Admin/Shop/DeleteBrand
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteBrand(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                //get the brand
                Brand brand = db.Brands.Find(id);

                //reomve the brand from table
                db.Brands.Remove(brand);
                //save table
                db.SaveChanges();

            }
            return RedirectToAction("Brands");
        }

        //POST: Admin/Shop/EditBrand
        [Authorize(Roles = "Admin")]
        public string EditBrand(int id, string bName)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.Brands.Any(x => x.Name == bName))
                {
                    return "titletaken";
                }
                Brand brand = db.Brands.Find(id);
                brand.Name = bName;
                db.SaveChanges();
                return "ok";

            }
        }

        ///////////////////////////////////////////////////////////////////////////////////

        /*
        * Category
        */

        // GET: Admin/Shop/Category
        [Authorize(Roles = "Admin")]
        public ActionResult Categories()
        {
            //declare a list of models
            List<CategoryViewModels> categoryVMList;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                categoryVMList = db.Categories.ToArray().Select(x => new CategoryViewModels(x)).ToList();
            }
            return View(categoryVMList);
        }

        //POST: Admin/Shop/CreateCategory
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public string CreateCategory(string cName)
        {
            string id;
            //declare id

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //check is name unique
                if (db.Categories.Any(x => x.Name == cName))
                {
                    return "titletaken";
                }
                //init brand table
                Category category = new Category();
                category.Name = cName;

                //add to brand table
                db.Categories.Add(category);
                //save table
                db.SaveChanges();
                //get the id
                id = category.Id.ToString();

            }

            return id;
        }

        //POST: Admin/Shop/DeleteCategory
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteCategory(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                //get the brand
                Category category = db.Categories.Find(id);

                //reomve the brand from table
                db.Categories.Remove(category);
                //save table
                db.SaveChanges();

            }
            return RedirectToAction("Categories");
        }

        //POST: Admin/Shop/EditCategory
        [Authorize(Roles = "Admin")]
        public string EditCategory(int id, string cName)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.Categories.Any(x => x.Name == cName))
                {
                    return "titletaken";
                }
                Category category = db.Categories.Find(id);
                category.Name = cName;
                db.SaveChanges();
                return "ok";

            }
        }

        ///////////////////////////////////////////////////////////////////////////////////

        /*
        * Product
        */
        // GET: Admin/Shop/Products
        [Authorize(Roles = "Admin")]
        public ActionResult Products(int? page, int? catId)
        {
            List<ProductViewModels> listOfProductVM;
            

            var pageNumber = page ?? 1;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                listOfProductVM = db.Products.ToArray().Where(x => catId == null || catId == 0 || x.CategoryId == catId).Select(x => new ProductViewModels {
                    Id = x.Id,
                    ProductName = x.ProductName,
                    Description = x.Description,
                    Price = x.Price,
                    ImageName = x.ImageName,
                    BrandName = db.Brands.ToList().First(b => b.Id.Equals(x.BrandId)).Name,
                    CategoryName = db.Categories.ToList().First(b => b.Id.Equals(x.CategoryId)).Name
                }).ToList();

                ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");

                ViewBag.SelectedCat = catId.ToString();

               
            }
            var onePageOfProducts = listOfProductVM.ToPagedList(pageNumber, 6);
            ViewBag.OnePageOfProducts = onePageOfProducts;

            return View(listOfProductVM);

        }

        // GET: Admin/Shop/AddProduct
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult AddProduct()
        {
            //declare a list of models
            ProductViewModels product = new ProductViewModels();


            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                product.BrandsList = new SelectList(db.Brands.ToList(), "Id", "Name");
                product.CategoriesList = new SelectList(db.Categories.ToList(), "Id", "Name");       
                
            }

            return View(product);
        }

        // POST: Admin/Shop/AddProduct
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddProduct(ProductViewModels model, HttpPostedFileBase file)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    model.BrandsList = new SelectList(db.Brands.ToList(), "Id", "Name");
                    model.CategoriesList = new SelectList(db.Categories.ToList(), "Id", "Name");
                    return View(model);
                }
            }
            //check name is unqiue
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.Products.Any(x => x.ProductName == model.ProductName))
                {
                    model.BrandsList = new SelectList(db.Brands.ToList(), "Id", "Name");
                    model.CategoriesList = new SelectList(db.Categories.ToList(), "Id", "Name");
                    ModelState.AddModelError("", "That product name is taken!");
                    return View(model);
                }
            }
            //declare product id
            int id;
            //init and save product table
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Product product = new Product();
                product.ProductName = model.ProductName;
                product.Description = model.Description;
                product.Price = (float)(Math.Round((double)model.Price, 2));
                product.ProductType = model.ProductType;
                product.addTime = DateTime.Now;
                product.BrandId = model.BrandId;
                product.CategoryId = model.CategoryId;
                product.ImageName = model.ImageName;

                if (model.ProductType == "0")
                {
                    product.ShoesSizeId = 2;
                    ClothSizeTable clothSize = new ClothSizeTable();
                    clothSize.XS = 0;
                    clothSize.S = 0;
                    clothSize.M = 0;
                    clothSize.L = 0;
                    clothSize.XL = 0;
                    db.ClothSizes.Add(clothSize);
                    db.SaveChanges();
                    product.ClothSizeId = clothSize.Id;
                }
                if (product.ProductType == "1")
                {
                    product.ClothSizeId = 3;
                    ShoesSizeTable shoesSize = new ShoesSizeTable();
                    shoesSize.seven = 0;
                    shoesSize.sevenhalf = 0;
                    shoesSize.eight = 0;
                    shoesSize.eighthalf = 0;
                    shoesSize.nine = 0;
                    shoesSize.ninehalf = 0;
                    shoesSize.ten = 0;
                    shoesSize.tenhalf = 0;
                    shoesSize.eleven = 0;
                    db.ShoesSizes.Add(shoesSize);
                    db.SaveChanges();
                    product.ShoesSizeId = shoesSize.Id;
                }

                db.Products.Add(product);
                db.SaveChanges();
                //get product id
                id = product.Id;

            }

            TempData["SM"] = "You have added the Product!";

            #region Upload Image
            //create necessary directories
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            var pathString1 = Path.Combine(originalDirectory.ToString(), "Products");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

            if (!Directory.Exists(pathString1))
                Directory.CreateDirectory(pathString1);
            if (!Directory.Exists(pathString2))
                Directory.CreateDirectory(pathString2);
            if (!Directory.Exists(pathString3))
                Directory.CreateDirectory(pathString3);
            if (!Directory.Exists(pathString4))
                Directory.CreateDirectory(pathString4);
            if (!Directory.Exists(pathString5))
                Directory.CreateDirectory(pathString5);


            //check if a file was uploaded
            if (file != null && file.ContentLength > 0)
            {
                //get file extension
                string ext = file.ContentType.ToLower();
                //verify extension
                if (ext != "image/jpg" && ext != "image/jpeg" && ext != "image/png")
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        model.BrandsList = new SelectList(db.Brands.ToList(), "Id", "Name");
                        model.CategoriesList = new SelectList(db.Categories.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "The image was not upload, wrong image extension!");
                        return View(model);
                    }
                }

                string imageName = file.FileName;

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Product table = db.Products.Find(id);
                    table.ImageName = imageName;

                    db.SaveChanges();
                }

                var path = string.Format("{0}\\{1}", pathString2, imageName);
                var path2 = string.Format("{0}\\{1}", pathString3, imageName);

                file.SaveAs(path);

                WebImage img = new WebImage(file.InputStream);
                img.Resize(150, 150);
                img.Save(path2);
            }
            #endregion


            //init image nam

            //save image name to table

            return RedirectToAction("EditProduct", new { id = id });
        }

        // Get:Admin/Shop/EditProduct
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditProduct(int id)
        {
            ProductViewModels productVM;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Product product = db.Products.Find(id);
                if(product == null)
                {
                    return Content("Product not exit!");             
                }
                productVM = new ProductViewModels(product);
                if (product.ProductType == "0")
                {
                    productVM.ClothSizeList = db.ClothSizes.ToList().First(s => s.Id.Equals(product.ClothSizeId));
                }
                if (product.ProductType == "1")
                {
                    productVM.ShoesSizeList = db.ShoesSizes.ToList().First(s => s.Id.Equals(product.ShoesSizeId));
                }
                productVM.BrandsList = new SelectList(db.Brands.ToList(), "id", "Name");
                productVM.CategoriesList = new SelectList(db.Categories.ToList(), "id", "Name");
                productVM.BrandName = db.Brands.ToList().First(b => b.Id.Equals(productVM.BrandId)).Name;
                productVM.CategoryName = db.Categories.ToList().First(b => b.Id.Equals(productVM.CategoryId)).Name;
                productVM.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs")).Select(fn => Path.GetFileName(fn));
            }
            return View(productVM);
        }

        // POST: Admin/Shop/EditProduct
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditProduct(ProductViewModels model,HttpPostedFileBase file)
        {
            int id = model.Id;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                model.BrandsList = new SelectList(db.Brands.ToList(), "id", "Name");
                model.CategoriesList = new SelectList(db.Categories.ToList(), "id", "Name");
                model.BrandName = db.Brands.ToList().First(b => b.Id.Equals(model.BrandId)).Name;
                model.CategoryName = db.Categories.ToList().First(b => b.Id.Equals(model.CategoryId)).Name;
            }

            if (!ModelState.IsValid)
            {
                return View();
            }
            
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                Product product = db.Products.Find(id);
                product.ProductName = model.ProductName;
                product.Description = model.Description;
                product.Price = (float)(Math.Round((double)model.Price, 2));
                product.BrandId = model.BrandId;
                product.CategoryId = model.CategoryId;
                db.SaveChanges();

                if (product.ProductType == "0")
                {

                    ClothSizeTable clothSize = db.ClothSizes.Find(product.ClothSizeId);
                    clothSize.XS = model.ClothSizeList.XS;
                    clothSize.S = model.ClothSizeList.S;
                    clothSize.M = model.ClothSizeList.M;
                    clothSize.L = model.ClothSizeList.L;
                    clothSize.XL = model.ClothSizeList.XL;
                    db.SaveChanges();
                    
                }
                if(product.ProductType == "1")
                {
                    ShoesSizeTable shoesSize = db.ShoesSizes.Find(product.ShoesSizeId);
                    shoesSize.seven = model.ShoesSizeList.seven;
                    shoesSize.sevenhalf = model.ShoesSizeList.sevenhalf;
                    shoesSize.eight = model.ShoesSizeList.eight;
                    shoesSize.eighthalf = model.ShoesSizeList.eighthalf;
                    shoesSize.nine = model.ShoesSizeList.nine;
                    shoesSize.ninehalf = model.ShoesSizeList.ninehalf;
                    shoesSize.ten = model.ShoesSizeList.ten;
                    shoesSize.tenhalf = model.ShoesSizeList.tenhalf;
                    shoesSize.eleven = model.ShoesSizeList.eleven;
                    db.SaveChanges();
                }

                TempData["SM"] = "You have edited the Product!";
            }

            

            //check if a file was uploaded
            if (file != null && file.ContentLength > 0)
            {
                //get file extension
                string ext = file.ContentType.ToLower();
                //verify extension
                if (ext != "image/jpg" && ext != "image/jpeg" && ext != "image/png")
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        ModelState.AddModelError("", "The image was not upload, wrong image extension!");
                        return View(model);
                    }
                }
                //delete old file

                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                var pathString1 = Path.Combine(originalDirectory.ToString(), "Products");
                var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
                var pathString3 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");

                DirectoryInfo di1 = new DirectoryInfo(pathString1);
                DirectoryInfo di2 = new DirectoryInfo(pathString2);

                foreach (FileInfo file2 in di1.GetFiles())
                    file2.Delete();
                foreach (FileInfo file3 in di2.GetFiles())
                    file3.Delete();


                //save change
                string imageName = file.FileName;

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Product product = db.Products.Find(id);
                    product.ImageName = imageName;

                    db.SaveChanges();
                }

                
                var path = string.Format("{0}\\{1}", pathString2, imageName);
                var path2 = string.Format("{0}\\{1}", pathString3, imageName);

                file.SaveAs(path);

                WebImage img = new WebImage(file.InputStream);
                img.Resize(350, 350);
                img.Save(path2);
            }

            return RedirectToAction("EditProduct");
        }

        // POST: Admin/Shop/DeleteProduct
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteProduct(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                Product product = db.Products.Find(id);
                string ptype = product.ProductType;
                int CSizeId = product.ClothSizeId;
                int SSiszeId = product.ShoesSizeId;

                db.Products.Remove(product);
                db.SaveChanges();

                if (ptype == "0")
                {
                    ClothSizeTable clothSize = db.ClothSizes.Find(CSizeId);
                    db.ClothSizes.Remove(clothSize);
                    db.SaveChanges();
                }
                if(ptype == "1")
                {
                    ShoesSizeTable shoesSize = db.ShoesSizes.Find(SSiszeId);
                    db.ShoesSizes.Remove(shoesSize);
                    db.SaveChanges();
                }
            }
            string fullPath1 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() );
            if (System.IO.File.Exists(fullPath1))
                System.IO.File.Delete(fullPath1);
            return RedirectToAction("Products");
        }


        //POST: Admin/Shop/SaveGalleryImages
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public void SaveGalleryImages(int id)
        {
            foreach(string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];

                if(file != null && file.ContentLength > 0)
                {
                    //set directory paths
                    var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                    
                    var pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
                    var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");
                    //set image path
                    var path = string.Format("{0}\\{1}", pathString1, file.FileName);
                    var path2 = string.Format("{0}\\{1}", pathString2, file.FileName);
                    //save original and thumb

                    file.SaveAs(path);
                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(350, 350);
                    img.Save(path2);
                }
            }
        }

        //POST: Admin/Shop/DeleteGalleryImage
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public void DeleteGalleryImage(int id, string imageName)
        {
            string fullPath1 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/Gallery/" + imageName);
            string fullPath2 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/Gallery/Thumbs/" + imageName);

            if (System.IO.File.Exists(fullPath1))
                System.IO.File.Delete(fullPath1);

            if (System.IO.File.Exists(fullPath2))
                System.IO.File.Delete(fullPath2);

        }
    }

        
}