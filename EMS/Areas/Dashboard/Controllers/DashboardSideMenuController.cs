using EMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.Areas.Dashboard.Controllers
{
    public class DashboardSideMenuController : Controller
    {
        // GET: SuperAdmin/SuperAdminSideMenu
        public ActionResult Index()
        {
            return PartialView("_MenuPartial");
        }
        public JsonResult GetDatas()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var menus = db.Menus.Where(m => m.ActionName == "Index" && m.Area == "Dashboard").OrderBy(m => m.Order).ToList();
            var result = new { Menus = menus };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }

}