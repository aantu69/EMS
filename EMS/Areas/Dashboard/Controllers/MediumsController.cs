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
using EMS.Areas.Dashboard.ViewModels;

namespace EMS.Areas.Dashboard.Controllers
{
    [DynamicRoleAuthorize]
    public class MediumsController : Controller
    {
        private int InstId;
        private string CurrentUserId;
        public MediumsController()
        {
            InstId = CommonFunction.CurrentUserInstituteId();
            CurrentUserId = CommonFunction.CurrentUserId();
        }

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
                var mediums = db.Mediums.Where(e => e.InstId == InstId).ToList();

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
                        mediums = isAsc ? mediums.OrderBy(e => e.Id).ToList() : mediums.OrderByDescending(e => e.Id).ToList();
                        break;
                }

                var TotalPages = (int)Math.Ceiling((double)mediums.Count() / pageSize);

                mediums = mediums
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var mediumShifts = new List<MediumShiftsViewModel>();
                foreach (var medium in mediums)
                {
                    var r = new MediumShiftsViewModel
                    {
                        Id = medium.Id,
                        MediumName = medium.MediumName,
                    };
                    mediumShifts.Add(r);
                }
                //Get all the Roles for our users
                foreach (var medium in mediumShifts)
                {
                    //medium.ShiftName = db.GetRoles(users.First(s => s.UserName == user.UserName).Id);
                    medium.ShiftName = (from a in db.Shifts
                                        join b in db.ShiftsMediums on a.Id equals b.ShiftId
                                        where (b.MediumId == medium.Id)
                                        select a.ShiftName).ToList();
                }

                var result = new { TotalPages = TotalPages, Mediums = mediumShifts, CurrentPage = page, PageSize = pageSize, Search = search };
                return Json(result, JsonRequestBehavior.AllowGet);

            }

        }

        public JsonResult GetData(int? Id)
        {
            if (Id.HasValue && Id.Value > 0)
            {
                using (EMSContext db = new EMSContext())
                {
                    var result = db.Mediums.Where(e => e.Id == Id.Value).SingleOrDefault();
                    if (result != null)
                    {
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }


            }
            return Json(new { status = "Failed" });
        }

        public JsonResult GetShifts()
        {
            using (EMSContext db = new EMSContext())
            {
                var result = db.Shifts.Where(e => e.InstId == InstId).ToList();
                if (result != null)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Failed" });
        }

        public JsonResult GetSelectedShifts(int? Id)
        {
            if (Id.HasValue && Id.Value > 0)
            {
                using (EMSContext db = new EMSContext())
                {
                    var result = (from a in db.ShiftsMediums
                                  where (a.MediumId == Id)
                                  select a.ShiftId).ToList();
                    if (result != null)
                    {
                        return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("No Result Found.", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                return Json(new { status = new HttpStatusCodeResult(HttpStatusCode.BadRequest) });
            }
            

        }

        //[HttpPost]
        public JsonResult CreateData(Medium model)
        {
            string retn = String.Empty;
            using (EMSContext db = new EMSContext())
            {
                try
                {
                    var exist = db.Mediums.Where(e => e.MediumName == model.MediumName && e.InstId == InstId).FirstOrDefault();
                    if (exist == null)
                    {
                        var advanced = new Medium();
                        advanced.MediumName = model.MediumName;
                        advanced.InstId = InstId;
                        advanced.ModifyId = CurrentUserId;
                        advanced.ModifyTime = DateTime.Now;
                        db.Mediums.Add(advanced);
                        db.SaveChanges();
                        //if (selectedShifts != null)
                        //{
                        //    foreach (string shiftId in selectedShifts)
                        //    {
                        //        var shiftMedium = new ShiftMedium();
                        //        shiftMedium.MediumId = advanced.Id;
                        //        shiftMedium.ShiftId = Convert.ToInt32(shiftId);
                        //        db.ShiftsMediums.Add(shiftMedium);
                        //    }
                        //    db.SaveChanges();
                        //}
                        
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
        public JsonResult UpdateData(Medium model)
        {
            string retn = String.Empty;
            using (EMSContext db = new EMSContext())
            {
                try
                {

                    if (model != null && model.Id > 0)
                    {
                        var exist = db.Mediums.Where(e => e.Id == model.Id).SingleOrDefault();
                        if (exist != null)
                        {
                            var duplicate = db.Mediums.Where(e => e.Id != model.Id && e.MediumName == model.MediumName && e.InstId == InstId).SingleOrDefault();
                            if (duplicate == null)
                            {
                                exist.Id = model.Id;
                                exist.InstId = InstId;
                                exist.MediumName = model.MediumName;
                                exist.ModifyId = CurrentUserId;
                                exist.ModifyTime = DateTime.Now;
                                db.Mediums.Attach(exist);
                                db.Entry(exist).State = EntityState.Modified;
                                db.SaveChanges();
                                //if (selectedShifts != null)
                                //{
                                //    db.ShiftsMediums.RemoveRange(db.ShiftsMediums.Where(c => c.MediumId == model.Id));
                                //    foreach (string shiftId in selectedShifts)
                                //    {
                                //        var shiftMedium = new ShiftMedium();
                                //        shiftMedium.MediumId = model.Id;
                                //        shiftMedium.ShiftId = Convert.ToInt32(shiftId);
                                //        db.ShiftsMediums.Add(shiftMedium);
                                //    }
                                //    db.SaveChanges();
                                //}
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

        public JsonResult DeleteData(int Id)
        {
            string retn = String.Empty;
            if (!String.IsNullOrEmpty(Id.ToString()))
            {
                using (EMSContext db = new EMSContext())
                {
                    try
                    {
                        var exist = db.Mediums.Where(e => e.Id == Id).FirstOrDefault();
                        if (exist != null)
                        {
                            db.Mediums.Remove(exist);                          
                            db.ShiftsMediums.RemoveRange(db.ShiftsMediums.Where(c => c.MediumId == Id));
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
