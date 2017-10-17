using EMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using EMS.Areas.SuperAdmin.Models;
using System.Data.Entity;

namespace EMS.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AccountsController : Controller
    {
     
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Roles()
        {
            return View();
        }
        public ActionResult AddRole()
        {
            return View();
        }
        public ActionResult UpdateRole()
        {
            return View();
        }
        public ActionResult Users()
        {
            return View();
        }
        public ActionResult AddUser()
        {
            return View();
        }
        public ActionResult UpdateUser()
        {
            return View();
        }
        public JsonResult CreateRole(RoleViewModel model)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            string retn = String.Empty;

            if (!RoleManager.RoleExists(model.Name))
            {   
                var role = new IdentityRole();
                role.Name = model.Name;
                var result = RoleManager.Create(role);
                if (result.Succeeded)
                {
                    retn = "The role created.";
                }
                else
                {
                    retn = result.Errors.ToList()[0];
                }

            }
            else
            {
                retn = "This role already exist.";
            }

            return Json(retn, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ModifyRole(RoleViewModel model)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            string retn = String.Empty;

            if (!RoleManager.RoleExists(model.Name))
            {
                var role = new IdentityRole();
                role.Id = model.Id;
                role.Name = model.Name;
                var result = RoleManager.Update(role);
                if (result.Succeeded)
                {
                    retn = "The role created.";
                }
                else
                {
                    retn = result.Errors.ToList()[0];
                }

            }
            else
            {
                retn = "This role already exist.";
            }

            return Json(retn, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteRole(string Id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            string retn = String.Empty;

            var currentRole = RoleManager.FindById(Id);

            if (currentRole != null)
            {
                if (RoleManager.RoleExists(currentRole.Name))
                {
                    var result = RoleManager.Delete(RoleManager.FindByName(currentRole.Name));
                    if (result.Succeeded)
                    {
                        retn = "The role deleted.";
                    }
                    else
                    {
                        retn = result.Errors.ToList()[0];
                    }

                }
                else
                {
                    retn = "This role does not exist.";
                }
            }

                

            return Json(retn, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateUser(RegisterViewModel model, string[] selectedRoles)
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
                user.InstituteId = model.InstituteId;
                var result = UserManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        foreach (string role in selectedRoles)
                        {
                            var roleResult = UserManager.AddToRoles(user.Id, role);
                        }
                            
                        //if (roleResult.Succeeded)
                        //{
                        //    retn = "The user created.";
                        //}
                        //else
                        //{
                        //    retn = "The user created but role not assigned.";
                        //}
                    }
                    //if (!UserManager.IsInRole(user.Id, model.UserRoles))
                    //{
                    //    var result1 = UserManager.AddToRole(user.Id, model.UserRoles);
                    //    if (result1.Succeeded)
                    //    {
                    //        retn = "The user created.";
                    //    }
                    //    else
                    //    {
                    //        retn = "The user created but role not assigned.";
                    //    }
                    //}
                    //else
                    //{
                    //    retn = "The user created but role already assigned.";
                    //}
                    
                    
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

        public JsonResult ModifyUser(RegisterViewModel model)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            string retn = String.Empty;

            var currentUser = UserManager.FindByEmail(model.Email);
            if (currentUser != null)
            {
                var user = new ApplicationUser();
                //user.UserName = model.Email;
                //user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.InstituteId = model.InstituteId;
                var result = UserManager.Update(user);

                if (result.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, model.UserRoles);
                    if (result1.Succeeded)
                    {
                        retn = "The user updated.";
                    }
                    else
                    {
                        retn = "The user created but role not assigned.";
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

        public JsonResult DeleteUser(RegisterViewModel model)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            string retn = String.Empty;

            var currentUser = UserManager.FindByEmail(model.Email);
            if (currentUser != null)
            {
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

        public JsonResult RemoveUserFromRole(RegisterViewModel model)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var retn = String.Empty;
            var user = UserManager.FindByEmail(model.Email);
            var role = RoleManager.FindByName(model.UserRoles);
            if(user == null)
            {
                retn = "The user does not exist.";
            }
            else if (role == null)
            {
                retn = "The role does not exist.";
            }
            else
            {
                if (UserManager.IsInRole(user.Id, model.UserRoles))
                {
                    var result = UserManager.RemoveFromRole(user.Id, model.UserRoles);
                    if (result.Succeeded)
                    {
                        retn = "The user removed from the role.";
                    }
                    else
                    {
                        retn = result.Errors.ToList()[0];
                    }
                    
                }
                else
                {
                    retn = "The user does not exist in the role.";
                }
            }
            return Json(retn, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRolesPaging(int page = 1, int pageSize = 10, string sortBy = "Name", bool isAsc = true, string search = null)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var roles = RoleManager.Roles.ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                roles = roles.Where(x =>
                    x.Name.ToLower().Contains(search)).ToList();
            }

            switch (sortBy)
            {
                case "Name":
                    roles = isAsc ? roles.OrderBy(e => e.Name).ToList() : roles.OrderByDescending(e => e.Name).ToList();
                    break;

                default:
                    roles = isAsc ? roles.OrderBy(e => e.Id).ToList() : roles.OrderByDescending(e => e.Id).ToList();
                    break;
            }

            var TotalPages = (int)Math.Ceiling((double)roles.Count() / pageSize);

            roles = roles
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new { TotalPages = TotalPages, Roles = roles, CurrentPage = page, PageSize = pageSize, Search = search };
            return Json(result, JsonRequestBehavior.AllowGet);


        }

        public JsonResult GetRoles()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var roles = RoleManager.Roles.ToList();
            var result = new { Roles = roles };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRole(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var result = RoleManager.FindById(id);
            if (result != null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("No Result Found.", JsonRequestBehavior.AllowGet);
            }
            //using (ApplicationDbContext db = new ApplicationDbContext())
            //{
            //    var result = db.Roles.Where(r => r.Id == id).SingleOrDefault();
            //    if (result != null)
            //    {
            //        return Json(result, JsonRequestBehavior.AllowGet);
            //    }
            //    else
            //    {
            //        return Json("No Result Found.", JsonRequestBehavior.AllowGet);
            //    }
            //}

        }

        public JsonResult GetUsersPaging(int page = 1, int pageSize = 10, string sortBy = "UserName", bool isAsc = true, string search = null)
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

            var result = new { TotalPages = TotalPages, Users = users, CurrentPage = page, PageSize = pageSize, Search = search };
            return Json(result, JsonRequestBehavior.AllowGet);


        }

        public JsonResult GetUsers(int page = 1, int pageSize = 10, string sortBy = "UserName", bool isAsc = true, string search = null)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = UserManager.Users.ToList();
            var result = new { Users = users };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUser(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var result = UserManager.FindById(id);
            if (result != null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("No Result Found.", JsonRequestBehavior.AllowGet);

            }
            //using (ApplicationDbContext db = new ApplicationDbContext())
            //{
            //    var result = db.Users.Where(r => r.Id == id).SingleOrDefault();
            //    if (result != null)
            //    {
            //        return Json(result, JsonRequestBehavior.AllowGet);
            //    }
            //}
            //return Json("No Result Found.", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRolesforUser(string email)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var result = UserManager.FindByEmail(email).Roles.ToList();
            if (result != null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("No Result Found.", JsonRequestBehavior.AllowGet);

            }

        }

        public JsonResult GetUsersforRole(string role)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var result = RoleManager.FindByName(role).Users.ToList();
            if (result != null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("No Result Found.", JsonRequestBehavior.AllowGet);

            }

        }

    }
}