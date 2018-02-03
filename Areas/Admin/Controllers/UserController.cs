using PagedList;
using Supreme.Entities;
using Supreme.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Supreme.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: Admin/User/UserList
        [Authorize(Roles = "Admin")]
        public ActionResult UserList(int? page, int? roleId)
        {

            List<UserViewModels> listOfUserVM;


            var pageNumber = page ?? 1;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                listOfUserVM = db.Members.ToArray().Where(x => roleId == null || roleId == 0 || x.RoleId == roleId).Select(x => new UserViewModels
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.EmailAddress,
                    Photo = x.PhotoName,
                    RoleName = db.MemberRoles.ToList().First(b => b.Id.Equals(x.RoleId)).RoleName
                }).ToList();

                ViewBag.Roles = new SelectList(db.MemberRoles.ToList(), "Id", "RoleName");

                ViewBag.SelectedRole = roleId.ToString();


            }
            var onePageOfUsers = listOfUserVM.ToPagedList(pageNumber, 6);
            ViewBag.OnePageOfUsers = onePageOfUsers;

            return View(listOfUserVM);
        }

        // GET: Admin/User/CreateUser
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateUser()
        {
            //declare a list of models
            UserViewModels user = new UserViewModels();


            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                user.RoleList = new SelectList(db.MemberRoles.ToList(), "Id", "RoleName");

            }

            return View(user);
        }
        // POST: Admin/User/CreateUser
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateUser(UserViewModels model)
        {
            if (!ModelState.IsValid)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    model.RoleList = new SelectList(db.MemberRoles.ToList(), "Id", "RoleName");
                    return View(model);
                }
            }
            //check name is unqiue
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.Members.Any(x => x.UserName == model.UserName)|| db.Members.Any(x => x.EmailAddress == model.Email))
                {
                    model.RoleList = new SelectList(db.MemberRoles.ToList(), "Id", "RoleName");
                    ModelState.AddModelError("", "You have registered already!");
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
                user.LastName = model.LastName;
                user.FirstName = model.FirstName;
                user.EmailAddress = model.Email;
                user.Address = model.Address;
                user.RoleId = model.RoleId;
                user.Password = model.Password;

                db.Members.Add(user);
                db.SaveChanges();
                //get product id
                id = user.Id;

            }

            TempData["SM"] = "You have added the User!";

            return RedirectToAction("UserList");
        }
        // GET: Admin/User/EditUser
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(int id)
        {
            UserViewModels UserVM;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Member user = db.Members.Find(id);
                if (user == null)
                {
                    return Content("User not exit!");
                }
                UserVM = new UserViewModels(user);

                UserVM.RoleList = new SelectList(db.MemberRoles.ToList(), "Id", "RoleName");
                UserVM.RoleName = db.MemberRoles.ToList().First(b => b.Id.Equals(UserVM.RoleId)).RoleName;
                return View(UserVM);
            }
            
        }
        // POST: Admin/User/EditUser
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(UserViewModels model)
        {
            int id = model.Id;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                model.RoleList = new SelectList(db.MemberRoles.ToList(), "Id", "RoleName");
                model.RoleName = db.MemberRoles.ToList().First(b => b.Id.Equals(model.RoleId)).RoleName;
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Member user = db.Members.Find(id);
                user.UserName = model.UserName;
                user.LastName = model.LastName;
                user.FirstName = model.FirstName;
                user.PhoneNumber = model.PhoneNumber;
                user.EmailAddress = model.Email;
                user.Address = model.Address;
                user.RoleId = model.RoleId;
                db.SaveChanges();

                TempData["SM"] = "You have edited the Product!";
            }

            return RedirectToAction("EditUser");
        }

        ///////////////////////////////////////////////////////////////////////////////////

        /*
        * User Role
        */

        // GET: Admin/User/Roles
        [Authorize(Roles = "Admin")]
        public ActionResult Roles()
        {
            //declare a list of models
            List<RoleViewModel> roleVMList;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                roleVMList = db.MemberRoles.ToArray().Select(x => new RoleViewModel(x)).ToList();
            }
            return View(roleVMList);
        }
        //POST: Admin/User/CreateRole
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public string CreateRole(string rName)
        {
            string id;
            //declare id

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //check is name unique
                if (db.MemberRoles.Any(x => x.RoleName == rName))
                {
                    return "titletaken";
                }
                //init brand table
                MemberRole role = new MemberRole();
                role.RoleName = rName;

                //add to brand table
                db.MemberRoles.Add(role);
                //save table
                db.SaveChanges();
                //get the id
                id = role.Id.ToString();

            }

            return id;
        }

        //POST: Admin/User/DeleteRole
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteRole(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                //get the role
                MemberRole role = db.MemberRoles.Find(id);

                //reomve the brand from table
                db.MemberRoles.Remove(role);
                //save table
                db.SaveChanges();

            }
            return RedirectToAction("Roles");
        }

        //POST: Admin/User/EditRole
        [Authorize(Roles = "Admin")]
        public string EditRole(int id, string rName)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.MemberRoles.Any(x => x.RoleName == rName))
                {
                    return "titletaken";
                }
                MemberRole role = db.MemberRoles.Find(id);
                role.RoleName = rName;
                db.SaveChanges();
                return "ok";

            }
        }

    }
}