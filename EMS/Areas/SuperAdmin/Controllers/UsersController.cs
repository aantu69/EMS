using EMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EMS.Areas.SuperAdmin.Controllers
{
    [DynamicRoleAuthorize]
    public class UsersController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }
        public JsonResult GetDatas(int page = 1, int pageSize = 10, string sortBy = "UserName", bool isAsc = true, string search = null)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = UserManager.Users.ToList();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                users = users.Where(x =>
                    x.Email.ToLower().Contains(search) ||
                    x.FirstName.ToLower().Contains(search) ||
                    x.LastName.ToLower().Contains(search) ||
                    x.UserName.ToLower().Contains(search)).ToList();
            }

            switch (sortBy)
            {
                case "UserName":
                    users = isAsc ? users.OrderBy(e => e.UserName).ToList() : users.OrderByDescending(e => e.UserName).ToList();
                    break;

                case "Email":
                    users = isAsc ? users.OrderBy(e => e.Email).ToList() : users.OrderByDescending(e => e.Email).ToList();
                    break;

                case "FirstName":
                    users = isAsc ? users.OrderBy(e => e.FirstName).ToList() : users.OrderByDescending(e => e.FirstName).ToList();
                    break;

                case "LastName":
                    users = isAsc ? users.OrderBy(e => e.LastName).ToList() : users.OrderByDescending(e => e.LastName).ToList();
                    break;

                default:
                    users = isAsc ? users.OrderBy(e => e.Id).ToList() : users.OrderByDescending(e => e.Id).ToList();
                    break;
            }

            var TotalPages = (int)Math.Ceiling((double)users.Count() / pageSize);

            users = users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var userRoles = new List<UserRolesViewModel>();
            foreach (var user in users)
            {
                var r = new UserRolesViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };
                userRoles.Add(r);
            }
            //Get all the Roles for our users
            foreach (var user in userRoles)
            {
                user.RoleNames = UserManager.GetRoles(users.First(s => s.UserName == user.UserName).Id);
            }

            var result = new { TotalPages = TotalPages, Users = userRoles, CurrentPage = page, PageSize = pageSize, Search = search };
            return Json(result, JsonRequestBehavior.AllowGet);


        }
        public JsonResult GetRoles()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var RoleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(db));
            var roles = RoleManager.Roles.ToList();
            if (roles != null)
            {
                return Json(roles, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("No Result Found.", JsonRequestBehavior.AllowGet);

            }
        }
        public JsonResult GetInsts()
        {
            EMSContext db = new EMSContext();
            var insts = db.Institutes.ToList();
            if (insts != null)
            {
                return Json(insts, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("No Result Found.", JsonRequestBehavior.AllowGet);

            }
        }
        public JsonResult GetData(string Id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            if (Id == null)
            {
                return Json(new { status = new HttpStatusCodeResult(HttpStatusCode.BadRequest) });
            }
            var result = UserManager.FindById(Id);
            if (result != null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("No Result Found.", JsonRequestBehavior.AllowGet);

            }
            
        }
        public JsonResult GetSelectedRoles(string Id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            if (Id == null)
            {
                return Json(new { status = new HttpStatusCodeResult(HttpStatusCode.BadRequest) });
            }

            var result = UserManager.GetRoles(Id).ToList().ToArray();

            if (result != null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("No Result Found.", JsonRequestBehavior.AllowGet);

            }

        }
        public JsonResult CreateData(RegisterViewModel model, string[] selectedRoles)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            string retn = String.Empty;

            var currentUser = UserManager.FindByEmail(model.Email);
            if (currentUser == null)
            {
                var user = new ApplicationUser();
                user.UserName = model.Email;
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.InstId = model.InstId;
                var result = UserManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        foreach (string role in selectedRoles)
                        {
                            var roleResult = UserManager.AddToRoles(user.Id, role);
                        }
                        retn = "The user created.";
                    }          

                }
                else
                {
                    retn = result.Errors.ToList()[0];
                }
            }
            else
            {
                retn = "This user already exist.";
            }

            return Json(retn, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateData(RegisterViewModel model, string[] selectedRoles)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            string retn = String.Empty;

            //var currentUser = UserManager.FindById(model.Id);
            var currentUser = UserManager.Users.Where(e => e.Id == model.Id).SingleOrDefault();
            if (currentUser != null)
            {
                //var user = new ApplicationUser();
                //user.UserName = model.Email;
                currentUser.Id = model.Id;
                currentUser.FirstName = model.FirstName;
                currentUser.LastName = model.LastName;
                currentUser.InstId = model.InstId;
                var result = UserManager.Update(currentUser);

                if (result.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        UserManager.RemoveFromRoles(currentUser.Id, UserManager.GetRoles(currentUser.Id).ToArray());
                        foreach (string role in selectedRoles)
                        {
                            var roleResult = UserManager.AddToRoles(currentUser.Id, role);
                        }
                        retn = "The user created.";
                    }
                    
                }
                else
                {
                    retn = result.Errors.ToList()[0];
                }
            }
            else
            {
                retn = "This user does not exist.";
            }

            return Json(retn, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteData(string Id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            string retn = String.Empty;

            var currentUser = UserManager.FindById(Id);
            if (currentUser != null)
            {
                //UserManager.RemoveFromRoles(Id, UserManager.GetRoles(Id).ToArray());
                var result = UserManager.Delete(currentUser);

                if (result.Succeeded)
                {
                    retn = "The user deleted.";
                }
                else
                {
                    retn = result.Errors.ToList()[0];
                }
            }
            else
            {
                retn = "This user does not exist.";
            }

            return Json(retn, JsonRequestBehavior.AllowGet);
        }

    }
}