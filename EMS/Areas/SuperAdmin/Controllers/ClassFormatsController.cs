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
    public class ClassFormatsController : Controller
    {
        private string CurrentUserId = CommonFunction.CurrentUserId();

        // GET: SuperAdmin/ClassFormats
        public ActionResult Index()
        {
            return View();
        }
        // GET: SuperAdmin/ClassFormats/Create
        public ActionResult Create()
        {
            return View();
        }
        // GET: SuperAdmin/ClassFormats/Edit/5
        public ActionResult Edit()
        {
            return View();
        }

        public JsonResult GetDatas(int page = 1, int pageSize = 10, string sortBy = "Name", bool isAsc = true, string search = null)
        {
            using (EMSContext db = new EMSContext())
            {
                var results = db.ClassFormats.ToList();

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
                    var result = db.ClassFormats.Where(e => e.Id == Id.Value).SingleOrDefault();
                    if (result != null)
                    {
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }


            }
            return Json(new { status = "Failed" });
        }

        public JsonResult CreateData(ClassFormat model)
        {
            //var CurrentUserInstituteId = CommonFunction.CurrentUserInstituteId();
            string retn = String.Empty;
            using (EMSContext db = new EMSContext())
            {
                try
                {
                    var exist = db.ClassFormats.Where(e => e.Name == model.Name).FirstOrDefault();
                    if (exist == null)
                    {
                        var advanced = new ClassFormat();
                        advanced.Name = model.Name;
                        advanced.ModifyId = CurrentUserId;
                        advanced.ModifyTime = DateTime.Now;
                        db.ClassFormats.Add(advanced);
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
        //[HttpPost]
        public JsonResult UpdateData(ClassFormat model)
        {
            //var CurrentUserInstituteId = CommonFunction.CurrentUserInstituteId();
            string retn = String.Empty;
            using (EMSContext db = new EMSContext())
            {
                try
                {

                    if (model != null && model.Id > 0)
                    {
                        var exist = db.ClassFormats.Where(e => e.Id == model.Id).SingleOrDefault();
                        if (exist != null)
                        {
                            var duplicate = db.ClassFormats.Where(e => e.Id != model.Id && e.Name == model.Name).SingleOrDefault();
                            if (duplicate == null)
                            {
                                exist.Id = model.Id;
                                exist.Name = model.Name;
                                exist.ModifyId = CurrentUserId;
                                exist.ModifyTime = DateTime.Now;
                                db.ClassFormats.Attach(exist);
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
                            retn = "Shift ID is not found.";
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
                        var exist = db.ClassFormats.Where(e => e.Id == Id).FirstOrDefault();
                        if (exist != null)
                        {
                            db.ClassFormats.Remove(exist);
                            db.SaveChanges();
                            retn = "Success.";
                        }
                        else
                        {
                            retn = "Format ID is not found.";
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
