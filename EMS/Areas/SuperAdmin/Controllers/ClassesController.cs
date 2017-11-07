using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EMS.Areas.SuperAdmin.Models;
using EMS.Models;
using EMS.Logic;

namespace EMS.Areas.SuperAdmin.Controllers
{
    [DynamicRoleAuthorize]
    public class ClassesController : Controller
    {
        private string CurrentUserId = CommonFunction.CurrentUserId();

        // GET: SuperAdmin/Classes
        public ActionResult Index()
        {
            return View();
        }
        // GET: SuperAdmin/Classes/Create
        public ActionResult Create()
        {
            return View();
        }
        // GET: SuperAdmin/Classes/Edit/5
        public ActionResult Edit()
        {
            return View();
        }
        public JsonResult GetDatas(int page = 1, int pageSize = 10, string sortBy = "Name", bool isAsc = true, string search = null)
        {
            using (EMSContext db = new EMSContext())
            {
                //var results = db.Classes.ToList();
                var results = (from c in db.Classes
                               join cf in db.ClassFormats on c.FormatId equals cf.Id
                               select new
                               {
                                   Id = c.Id,
                                   Name = c.Name,
                                   FormatName = cf.Name
                               }).ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.ToLower();
                    results = results.Where(x =>
                        x.Name.ToLower().Contains(search)).ToList();
                }

                switch (sortBy)
                {
                    case "Name":
                        results = isAsc ? results.OrderBy(e => e.Name).ToList() : results.OrderByDescending(e => e.Name).ToList();
                        break;

                    default:
                        results = isAsc ? results.OrderBy(e => e.Id).ToList() : results.OrderByDescending(e => e.Id).ToList();
                        break;
                }

                var TotalPages = (int)Math.Ceiling((double)results.Count() / pageSize);

                results = results
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var result = new { TotalPages = TotalPages, Results = results, CurrentPage = page, PageSize = pageSize, Search = search };
                return Json(result, JsonRequestBehavior.AllowGet);

            }

        }
        public JsonResult GetData(int? Id)
        {
            if (Id.HasValue && Id.Value > 0)
            {
                using (EMSContext db = new EMSContext())
                {
                    var result = db.Classes.Where(e => e.Id == Id.Value).SingleOrDefault();
                    if (result != null)
                    {
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }


            }
            return Json(new { status = "Failed" });
        }
        public JsonResult GetFormats()
        {
            using (EMSContext db = new EMSContext())
            {
                var result = db.ClassFormats.ToList();
                if (result != null)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Failed" });
        }
        public JsonResult CreateData(Class model)
        {
            //var CurrentUserInstituteId = CommonFunction.CurrentUserInstituteId();
            string retn = String.Empty;
            using (EMSContext db = new EMSContext())
            {
                try
                {
                    var exist = db.Classes.Where(e => e.Name == model.Name && e.FormatId == model.FormatId).FirstOrDefault();
                    if (exist == null)
                    {
                        var advanced = new Class();
                        advanced.FormatId = model.FormatId;
                        advanced.Name = model.Name;
                        advanced.ModifyId = CurrentUserId;
                        advanced.ModifyTime = DateTime.Now;
                        db.Classes.Add(advanced);
                        db.SaveChanges();
                        retn = "Success";
                    }
                    else
                    {
                        retn = "This Name is already exist.";
                    }

                }
                catch
                {
                    retn = "Failed";
                }
            }

            return Json(retn, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateData(Class model)
        {
            string retn = String.Empty;
            using (EMSContext db = new EMSContext())
            {
                try
                {
                    if (model != null && model.Id > 0)
                    {
                        var exist = db.Classes.Where(e => e.Id == model.Id).SingleOrDefault();
                        if (exist != null)
                        {
                            var duplicate = db.Classes.Where(e => e.Id != model.Id && e.Name == model.Name && e.FormatId == model.FormatId).SingleOrDefault();
                            if (duplicate == null)
                            {
                                exist.Id = model.Id;
                                exist.Name = model.Name;
                                exist.FormatId = model.FormatId;
                                exist.ModifyId = CurrentUserId;
                                exist.ModifyTime = DateTime.Now;
                                db.Classes.Attach(exist);
                                db.Entry(exist).State = EntityState.Modified;
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
                            retn = "Class ID is not found.";
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
                using (EMSContext db = new EMSContext())
                {
                    try
                    {
                        var exist = db.Classes.Where(e => e.Id == Id).FirstOrDefault();
                        if (exist != null)
                        {
                            db.Classes.Remove(exist);
                            db.SaveChanges();
                            retn = "Success.";
                        }
                        else
                        {
                            retn = "Class ID is not found.";
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
