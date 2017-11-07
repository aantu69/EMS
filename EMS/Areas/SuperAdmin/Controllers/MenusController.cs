using EMS.Areas.SuperAdmin.Models;
using EMS.Logic;
using EMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.Areas.SuperAdmin.Controllers
{
    [DynamicRoleAuthorize]
    public class MenusController : Controller
    {

        // GET: SuperAdmin/Employees
        [DynamicRoleAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [DynamicRoleAuthorize]
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
            var menus = db.Menus.ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                menus = menus.Where(x =>
                    x.Name.ToLower().Contains(search)).ToList();
            }

            switch (sortBy)
            {
                case "Name":
                    menus = isAsc ? menus.OrderBy(e => e.Name).ToList() : menus.OrderByDescending(e => e.Name).ToList();
                    break;

                case "Area":
                    menus = isAsc ? menus.OrderBy(e => e.Area).ToList() : menus.OrderByDescending(e => e.Area).ToList();
                    break;

                case "ControllerName":
                    menus = isAsc ? menus.OrderBy(e => e.ControllerName).ToList() : menus.OrderByDescending(e => e.ControllerName).ToList();
                    break;

                case "ActionName":
                    menus = isAsc ? menus.OrderBy(e => e.ActionName).ToList() : menus.OrderByDescending(e => e.ActionName).ToList();
                    break;

                case "IsActive":
                    menus = isAsc ? menus.OrderBy(e => e.IsActive).ToList() : menus.OrderByDescending(e => e.IsActive).ToList();
                    break;

                default:
                    menus = isAsc ? menus.OrderBy(e => e.Id).ToList() : menus.OrderByDescending(e => e.Id).ToList();
                    break;
            }

            var TotalPages = (int)Math.Ceiling((double)menus.Count() / pageSize);

            menus = menus
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new { TotalPages = TotalPages, Menus = menus, CurrentPage = page, PageSize = pageSize, Search = search };
            return Json(result, JsonRequestBehavior.AllowGet);

            

        }

        public JsonResult GetData(int? Id)
        {
            if (Id.HasValue && Id.Value > 0)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var result = db.Menus.Where(e => e.Id == Id.Value).SingleOrDefault();
                if (result != null)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }


            }
            return Json(new { status = "Failed" });
        }
        //[HttpPost]
        public JsonResult CreateData(Menu menu)
        {
            string retn = String.Empty;
            ApplicationDbContext db = new ApplicationDbContext();
            var currentMenu = db.Menus.Where(e => e.Name == menu.Name && e.ControllerName == menu.ControllerName && e.ActionName == menu.ActionName).FirstOrDefault();
            if (currentMenu == null)
            {
                var newMenu = new Menu();
                newMenu.Name = menu.Name;
                newMenu.Area = menu.Area;
                newMenu.ControllerName = menu.ControllerName;
                newMenu.ActionName = menu.ActionName;
                newMenu.IsActive = menu.IsActive;
                newMenu.Order = menu.Order;
                newMenu.ModifyId = CommonFunction.CurrentUserId();
                newMenu.ModifyTime = DateTime.Now;
                db.Menus.Add(newMenu);
                db.SaveChanges();
                retn = "Success";
            }
            else
            {
                retn = "This Name is already exist.";
            }

            return Json(retn, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        public JsonResult UpdateData(Menu menu)
        {
            string retn = String.Empty;
            //using (EMSContext db = new EMSContext())
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {

                    if (menu != null && menu.Id > 0)
                    {
                        var CurrentMenu = db.Menus.Where(e => e.Id == menu.Id).SingleOrDefault();
                        if (CurrentMenu != null)
                        {
                            var UpdateMenu = db.Menus.Where(e => e.Id != menu.Id && e.Name == menu.Name && e.ControllerName == menu.ControllerName && e.ActionName == menu.ActionName).FirstOrDefault();
                            if (UpdateMenu == null)
                            {
                                CurrentMenu.Id = menu.Id;
                                CurrentMenu.Name = menu.Name;
                                CurrentMenu.Area = menu.Area;
                                CurrentMenu.ControllerName = menu.ControllerName;
                                CurrentMenu.ActionName = menu.ActionName;
                                CurrentMenu.IsActive = menu.IsActive;
                                CurrentMenu.Order = menu.Order;
                                CurrentMenu.ModifyId = CommonFunction.CurrentUserId();
                                CurrentMenu.ModifyTime = DateTime.Now;
                                db.Menus.Attach(CurrentMenu);
                                db.Entry(CurrentMenu).State = EntityState.Modified;
                                db.SaveChanges();
                                retn = "Success.";
                            }
                            else
                            {
                                retn = "This Name Already Exist.";
                            }

                        }
                        else
                        {
                            retn = "Menu ID is not found.";
                        }
                    }
                }
                catch
                {
                    retn = "Failed";
                }
            }

            return Json(retn, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteData(int Id)
        {
            string retn = String.Empty;
            if (!String.IsNullOrEmpty(Id.ToString()))
            {
                //using (EMSContext db = new EMSContext())
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    try
                    {
                        var menu = db.Menus.Where(e => e.Id == Id).FirstOrDefault();
                        if (menu != null)
                        {
                            db.Menus.Remove(menu);
                            db.SaveChanges();
                            retn = "Success.";
                        }
                        else
                        {
                            retn = "Menu ID is not found.";
                        }

                    }
                    catch
                    {
                        retn = "Failed";
                    }
                }
            }
            else
            {
                retn = "Failed";
            }

            return Json(retn, JsonRequestBehavior.AllowGet);
        }


    }
}