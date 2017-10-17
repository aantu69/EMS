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
    public class ShiftsController : Controller
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

        public JsonResult GetDatas(int page = 1, int pageSize = 10, string sortBy = "ShiftName", bool isAsc = true, string search = null)
        {
            using (EMSContext db = new EMSContext())
            {
                var shifts = db.Shifts.ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.ToLower();
                    shifts = shifts.Where(x =>
                        x.ShiftName.ToLower().Contains(search)).ToList();
                }

                switch (sortBy)
                {
                    case "ShiftName":
                        shifts = isAsc ? shifts.OrderBy(e => e.ShiftName).ToList() : shifts.OrderByDescending(e => e.ShiftName).ToList();
                        break;
 
                    default:
                        shifts = isAsc ? shifts.OrderBy(e => e.ID).ToList() : shifts.OrderByDescending(e => e.ID).ToList();
                        break;
                }

                var TotalPages = (int)Math.Ceiling((double)shifts.Count() / pageSize);

                shifts = shifts
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var result = new { TotalPages = TotalPages, Shifts = shifts, CurrentPage = page, PageSize = pageSize, Search = search };
                return Json(result, JsonRequestBehavior.AllowGet);

            }

        }

        public JsonResult GetData(int? id)
        {
            if (id.HasValue && id.Value > 0)
            {
                using (EMSContext db = new EMSContext())
                {
                    var result = db.Shifts.Where(e => e.ID == id.Value).SingleOrDefault();
                    if (result != null)
                    {
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }


            }
            return Json(new { status = "Failed" });
        }
        //[HttpPost]
        public JsonResult CreateData(Shift shift)
        {
            string retn = String.Empty;
            using (EMSContext db = new EMSContext())
            {
                try
                {
                    var currentShift = db.Shifts.Where(e => e.ShiftName == shift.ShiftName).FirstOrDefault();
                    if (currentShift == null)
                    {
                        var newShift = new Shift();
                        newShift.ShiftName = shift.ShiftName;
                        newShift.ModifyID = CommonFunction.CurrentUserId();
                        newShift.ModifyTime = DateTime.Now;
                        db.Shifts.Add(newShift);
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
        public JsonResult UpdateData(Shift shift)
        {
            string retn = String.Empty;
            using (EMSContext db = new EMSContext())
            {
                try
                {

                    if (shift != null && shift.ID > 0)
                    {
                        var CurrentShift = db.Shifts.Where(e => e.ID == shift.ID).SingleOrDefault();
                        if (CurrentShift != null)
                        {
                            var UpdateShift = db.Shifts.Where(e => e.ID != shift.ID && e.ShiftName == shift.ShiftName).SingleOrDefault();
                            if (UpdateShift == null)
                            {
                                CurrentShift.ID = shift.ID;
                                CurrentShift.ShiftName = shift.ShiftName;
                                CurrentShift.ModifyID= CommonFunction.CurrentUserId();
                                CurrentShift.ModifyTime = DateTime.Now;
                                db.Shifts.Attach(CurrentShift);
                                db.Entry(CurrentShift).State = EntityState.Modified;
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

        public JsonResult DeleteData(int id)
        {
            string retn = String.Empty;
            if (!String.IsNullOrEmpty(id.ToString()))
            {
                using (EMSContext db = new EMSContext())
                {
                    try
                    {
                        var shift = db.Shifts.Where(e => e.ID == id).FirstOrDefault();
                        if (shift != null)
                        {
                            db.Shifts.Remove(shift);
                            db.SaveChanges();
                            retn = "Success.";
                        }
                        else
                        {
                            retn = "Shift ID is not found.";
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
