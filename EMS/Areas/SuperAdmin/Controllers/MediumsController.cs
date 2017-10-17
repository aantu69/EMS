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
    public class MediumsController : Controller
    {

        // GET: SuperAdmin/Employees
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

        public JsonResult GetDatas(int page = 1, int pageSize = 10, string sortBy = "MediumName", bool isAsc = true, string search = null)
        {
            using (EMSContext db = new EMSContext())
            {
                var mediums = db.Mediums.ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.ToLower();
                    mediums = mediums.Where(x =>
                        x.MediumName.ToLower().Contains(search)).ToList();
                }

                switch (sortBy)
                {
                    case "MediumName":
                        mediums = isAsc ? mediums.OrderBy(e => e.MediumName).ToList() : mediums.OrderByDescending(e => e.MediumName).ToList();
                        break;

                    default:
                        mediums = isAsc ? mediums.OrderBy(e => e.ID).ToList() : mediums.OrderByDescending(e => e.ID).ToList();
                        break;
                }

                var TotalPages = (int)Math.Ceiling((double)mediums.Count() / pageSize);

                mediums = mediums
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var result = new { TotalPages = TotalPages, Mediums = mediums, CurrentPage = page, PageSize = pageSize, Search = search };
                return Json(result, JsonRequestBehavior.AllowGet);

            }

        }

        public JsonResult GetData(int? id)
        {
            if (id.HasValue && id.Value > 0)
            {
                using (EMSContext db = new EMSContext())
                {
                    var result = db.Mediums.Where(e => e.ID == id.Value).SingleOrDefault();
                    if (result != null)
                    {
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }


            }
            return Json(new { status = "Failed" });
        }
        //[HttpPost]
        public JsonResult CreateData(Medium medium)
        {
            string retn = String.Empty;
            using (EMSContext db = new EMSContext())
            {
                try
                {
                    var currentMedium = db.Mediums.Where(e => e.MediumName == medium.MediumName).FirstOrDefault();
                    if (currentMedium == null)
                    {
                        var newMedium = new Medium();
                        newMedium.MediumName = medium.MediumName;
                        newMedium.ModifyID = CommonFunction.CurrentUserId();
                        newMedium.ModifyTime = DateTime.Now;
                        db.Mediums.Add(newMedium);
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
        public JsonResult UpdateData(Medium medium)
        {
            string retn = String.Empty;
            using (EMSContext db = new EMSContext())
            {
                try
                {

                    if (medium != null && medium.ID > 0)
                    {
                        var CurrentMedium = db.Mediums.Where(e => e.ID == medium.ID).SingleOrDefault();
                        if (CurrentMedium != null)
                        {
                            var UpdateMedium = db.Mediums.Where(e => e.ID != medium.ID && e.MediumName == medium.MediumName).SingleOrDefault();
                            if (UpdateMedium == null)
                            {
                                CurrentMedium.ID = medium.ID;
                                CurrentMedium.MediumName = medium.MediumName;
                                CurrentMedium.ModifyID = CommonFunction.CurrentUserId();
                                CurrentMedium.ModifyTime = DateTime.Now;
                                db.Mediums.Attach(CurrentMedium);
                                db.Entry(CurrentMedium).State = EntityState.Modified;
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
                            retn = "Medium ID is not found.";
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

        public JsonResult DeleteData(int id)
        {
            string retn = String.Empty;
            if (!String.IsNullOrEmpty(id.ToString()))
            {
                using (EMSContext db = new EMSContext())
                {
                    try
                    {
                        var medium = db.Mediums.Where(e => e.ID == id).FirstOrDefault();
                        if (medium != null)
                        {
                            db.Mediums.Remove(medium);
                            db.SaveChanges();
                            retn = "Success.";
                        }
                        else
                        {
                            retn = "Medium ID is not found.";
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
