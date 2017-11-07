using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EMS.Areas.Dashboard.Models;
using EMS.Models;
using EMS.Logic;

namespace EMS.Areas.Dashboard.Controllers
{
    [DynamicRoleAuthorize]
    public class ShiftsController : Controller
    {
        private int InstId;
        private string CurrentUserId;
        public ShiftsController()
        {
            InstId = CommonFunction.CurrentUserInstituteId();
            CurrentUserId = CommonFunction.CurrentUserId();
        }
        // GET: SuperAdmin/Shifts
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
                var shifts = db.Shifts.Include(e => e.Medium).Where(e => e.InstId == InstId).ToList();

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
                        shifts = isAsc ? shifts.OrderBy(e => e.Id).ToList() : shifts.OrderByDescending(e => e.Id).ToList();
                        break;
                }

                var TotalPages = (int)Math.Ceiling((double)shifts.Count() / pageSize);

                shifts = shifts
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                var shiftsList = shifts.Select(s => new { Id = s.Id, ShiftName = s.ShiftName, MediumName = s.Medium.MediumName}).ToList();

                var result = new { TotalPages = TotalPages, Shifts = shiftsList, CurrentPage = page, PageSize = pageSize, Search = search };
                return Json(result, JsonRequestBehavior.AllowGet);

            }

        }
        public JsonResult GetData(int? Id)
        {
            if (Id.HasValue && Id.Value > 0)
            {
                using (EMSContext db = new EMSContext())
                {
                    var result = db.Shifts.Where(e => e.Id == Id.Value).SingleOrDefault();
                    if (result != null)
                    {
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }


            }
            return Json(new { status = "Failed" });
        }
        public JsonResult GetMediums()
        {
            using (EMSContext db = new EMSContext())
            {
                var results = db.Mediums.Where(e => e.InstId == InstId).ToList();
                if (results != null)
                {
                    var result = results.Select(s => new { Id = s.Id, MediumName = s.MediumName }).ToList();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Failed" });
        }

        public JsonResult CreateData(Shift model)
        {
            string retn = String.Empty;
            using (EMSContext db = new EMSContext())
            {
                try
                {
                    var exist = db.Shifts.Where(e => e.ShiftName == model.ShiftName && e.MediumId == model.MediumId && e.InstId == InstId).FirstOrDefault();
                    if (exist == null)
                    {
                        var advanced = new Shift();
                        advanced.ShiftName = model.ShiftName;
                        advanced.InstId = InstId;
                        advanced.MediumId = model.MediumId;
                        advanced.ModifyId = CurrentUserId;
                        advanced.ModifyTime = DateTime.Now;
                        db.Shifts.Add(advanced);
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
        public JsonResult UpdateData(Shift model)
        {
            //var CurrentUserInstituteId = CommonFunction.CurrentUserInstituteId();
            string retn = String.Empty;
            using (EMSContext db = new EMSContext())
            {
                try
                {

                    if (model != null && model.Id > 0)
                    {
                        var exist = db.Shifts.Where(e => e.Id == model.Id).SingleOrDefault();
                        if (exist != null)
                        {
                            var duplicate = db.Shifts.Where(e => e.Id != model.Id && e.ShiftName == model.ShiftName && e.MediumId == model.MediumId && e.InstId == InstId).SingleOrDefault();
                            if (duplicate == null)
                            {
                                exist.Id = model.Id;
                                exist.InstId = InstId;
                                exist.MediumId = model.MediumId;
                                exist.ShiftName = model.ShiftName;
                                exist.ModifyId= CurrentUserId;
                                exist.ModifyTime = DateTime.Now;
                                db.Shifts.Attach(exist);
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
                        var exist = db.Shifts.Where(e => e.Id == Id).FirstOrDefault();
                        if (exist != null)
                        {
                            db.Shifts.Remove(exist);
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
