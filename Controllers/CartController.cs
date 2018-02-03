using Supreme.Entities;
using Supreme.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Supreme.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult CartDetail()
        {
            var cart = Session["cart"] as List<CartViewModels> ?? new List<CartViewModels>();

            if(cart.Count == 0 || Session["cart"] == null)
            {
                ViewBag.Message = "Your cart is empty.";
                return View();

            }

            float total = 0;
            foreach( var item in cart)
            {
                total += item.Total;
            }
            ViewBag.GrandTotal = total;

            return View(cart);
        }

        public ActionResult CartPartial()
        {

            List<CartViewModels> cart = Session["cart"] as List<CartViewModels>;

            if (Session["cart"] == null)
            {
                return PartialView();
            }

            float total = 0;
            int qtyTotal = 0;
            foreach (var item in cart)
            {
                qtyTotal += item.Quantity;
                total += item.Total;
            }
            ViewBag.GrandTotal = total;
            ViewBag.QtyTotal = qtyTotal;

            return PartialView(cart);
        }

        public ActionResult AddCartPartial(int id, int currentqty, string size, int maxQty)
        {
            List<CartViewModels> cart = Session["cart"] as List<CartViewModels> ?? new List<CartViewModels>();

            CartViewModels model = new CartViewModels();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Product product = db.Products.Find(id);

                var productInCart = cart.FirstOrDefault(x => x.ProductId == id && x.ProductSize == size);
                if (productInCart == null)
                {
                    cart.Add(new CartViewModels()
                    {
                        ProductId = product.Id,
                        ProductName = product.ProductName,
                        Quantity = currentqty,
                        maxQty = maxQty,
                        ProductSize = size,
                        Price = product.Price,
                        ImageName = product.ImageName

                    });
                }
                else
                {
                    productInCart.Quantity+=currentqty;
                }
            }

            int qty = 0;
            float price = 0;

            foreach (var item in cart)
            {
                qty += item.Quantity;
                price += item.Quantity * price;
            }

            model.Quantity = qty;
            model.Price = price;

            Session["cart"] = cart; 

            return PartialView(model);
        }

        public ActionResult IncrementProduct(int id, string size)
        {
            List<CartViewModels> cart = Session["cart"] as List<CartViewModels>;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                CartViewModels model = cart.FirstOrDefault(x => x.ProductId == id && x.ProductSize == size);
                model.Quantity++;
                var result = new { qty = model.Quantity, price = model.Price };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
               
        }

        public ActionResult DecreaseProduct(int id, string size)
        {
            List<CartViewModels> cart = Session["cart"] as List<CartViewModels>;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                CartViewModels model = cart.FirstOrDefault(x => x.ProductId == id && x.ProductSize == size);
                model.Quantity--;
                var result = new { qty = model.Quantity, price = model.Price };

                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult RemoveProduct(int id, string size)
        {
            List<CartViewModels> cart = Session["cart"] as List<CartViewModels>;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                CartViewModels model = cart.FirstOrDefault(x => x.ProductId == id && x.ProductSize == size);
                cart.Remove(model);
            }
                return View();
        }

        [HttpPost]
        public ActionResult CheckOut(float total)
        {
            var cart = Session["cart"] as List<CartViewModels> ?? new List<CartViewModels>();
            if (cart.Count() == 0)
            {
                ViewBag.Message = "Your cart is empty.";
                return RedirectToAction("~/Cart/CartDetail");
            }
            int id;
            string username = User.Identity.Name;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Order order = new Order();
                order.UserName = username;
                order.TotalPrice = total;
                order.OderTime = DateTime.Now;
                order.UserId = 3;

                db.Orders.Add(order);
                db.SaveChanges();
                id = order.Id;

                foreach (var item in cart)
                {
                    OrderDetail OD = new OrderDetail();
                    OD.ProductName = item.ProductName;
                    OD.Quantity = item.Quantity;
                    OD.Price = item.Price;
                    OD.OrderId = id;

                    db.OrderDetails.Add(OD);
                    db.SaveChanges();
                }


            }
            return Redirect("Home");
        }

    }
}