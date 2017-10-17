using EMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System;

namespace EMS.Areas.SuperAdmin.Controllers
{
    public class RolesController : Controller
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

        public JsonResult GetDatas(int page = 1, int pageSize = 10, string sortBy = "Name", bool isAsc = true, string search = null)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var RoleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(db));
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

        public JsonResult GetData(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var RoleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(db));
            if (id == null)
            {
                return Json(new { status = new HttpStatusCodeResult(HttpStatusCode.BadRequest) });
            }
            var result = RoleManager.FindById(id);
            if (result == null)
            {
                return Json(new { status = HttpNotFound() });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMenus()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var menus = db.Menus.ToList();
            var result = new { Menus = menus };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateData(RoleViewModel model)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var RoleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(db));
            string retn = String.Empty;
            if (ModelState.IsValid)
            {
                if (!RoleManager.RoleExists(model.Name))
                {
                    var role = new ApplicationRole();
                    role.Name = model.Name;
                    role.Description = model.Description;
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

            }

            return Json(retn, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateData(RoleViewModel model)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var RoleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(db));
            string retn = String.Empty;
            if (ModelState.IsValid)
            {
                var CurrentRole = RoleManager.Roles.Where(e => e.Id == model.Id).SingleOrDefault();
                if (CurrentRole != null)
                {
                    var UpdateRole = RoleManager.Roles.Where(e => e.Id != model.Id && e.Name == model.Name).SingleOrDefault();
                    if (UpdateRole == null)
                    {
                        CurrentRole.Id = model.Id;
                        CurrentRole.Name = model.Name;
                        CurrentRole.Description = model.Description;
                        var result = RoleManager.Update(CurrentRole);
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
                        retn = "This Role Already Exist.";
                    }
                }
                else
                {
                    retn = "Role Id is not found.";
                }
                
            }

            return Json(retn, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteData(string Id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var RoleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(db));
            string retn = String.Empty;
            if (Id == null)
            {
                return Json(new { status = new HttpStatusCodeResult(HttpStatusCode.BadRequest) });
            }
            var currentRole = RoleManager.FindById(Id);
            if (currentRole == null)
            {
                return Json(new { status = HttpNotFound() });
            }

            var result = RoleManager.Delete(RoleManager.FindByName(currentRole.Name));
            if (result.Succeeded)
            {
                retn = "The role deleted.";
            }
            else
            {
                retn = result.Errors.ToList()[0];
            }
            return Json(retn, JsonRequestBehavior.AllowGet);
        }
  
    }
}