using Supreme.Entities;
using Supreme.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Supreme.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {

            return View();
        }


        [HttpGet]
        public ActionResult Register()
        {
            UserViewModels model = new UserViewModels();

            return View(model);
        }

        [HttpPost]
        public ActionResult Register(UserViewModels model)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", model);
            }
            //check password is equal com password
            if (model.Password != model.ComfirmPassword)
            {
                ModelState.AddModelError("", "Password does not match!");
                return View("Register", model);
            }
            //check name is unqiue
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.Members.Any(x => x.UserName == model.UserName) || db.Members.Any(x => x.EmailAddress == model.Email))
                {
                    ModelState.AddModelError("", "Account already exist");
                    return View(model);
                }
            }
            //declare product id
            int id;
            //init and save product table
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Member user = new Member();
                user.UserName = model.UserName;
                user.EmailAddress = model.Email;
                user.Password = model.Password;
                user.RoleId = 3;

                db.Members.Add(user);
                db.SaveChanges();
                //get product id
                id = user.Id;

            }

            TempData["SM"] = "You are registed!";

            return RedirectToAction("UserList");
        }

        [HttpGet]
        public ActionResult Login()
        {
            string username = User.Identity.Name;
            if (!string.IsNullOrEmpty(username))
                return RedirectToAction("UserProfile");

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModels model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isValid = false;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if(db.Members.Any(x=>x.UserName.Equals(model.UserName) && x.Password.Equals(model.Password))){
                    isValid = true;
                }
            }

            if (!isValid)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }
            else
            {
                
                FormsAuthentication.SetAuthCookie(model.UserName,true);
                return Redirect(FormsAuthentication.GetRedirectUrl(model.UserName,true));
            }
        }


        public ActionResult logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/Account/login");
        }

        [HttpGet]
        [Authorize]
        public ActionResult AccountProfile()
        {

            string username = User.Identity.Name;
            AccountViewModels model;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Member user = db.Members.FirstOrDefault(x => x.UserName == username);
                model = new AccountViewModels(user);
            }
                return View(model);
        }

        public ActionResult AccountPartial()
        {
           
            return PartialView();

        }

        [HttpGet]
        [Authorize]
        public ActionResult OrderHistory()
        {
            List<Order> listOforder;
            string username = User.Identity.Name;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                listOforder = db.Orders.ToArray().Where(x => x.UserName == username).ToList();
                List<HistoryViewModels> listOfhistoryVM = new List<HistoryViewModels>();
                foreach (var item in listOforder)
                {
                    listOfhistoryVM.AddRange(db.OrderDetails.ToArray().Where(x => x.OrderId == item.Id).Select(x => new HistoryViewModels
                    {
                        OrderNumber = x.OrderId,
                        ProductName = x.ProductName,
                        Quantity = x.Quantity,
                        Price = x.Price,

                    }).ToList());
                }

                return View(listOfhistoryVM);
            }
        }
        
    }
}