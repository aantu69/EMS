using EMS.Areas.SuperAdmin.Models;
using EMS.Logic;
using EMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.Areas.SuperAdmin.Controllers
{
    [DynamicRoleAuthorize]
    public class InstitutesController : Controller
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

            EMSContext db = new EMSContext();
            var insts = db.Institutes.ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                insts = insts.Where(x =>
                    x.Name.ToLower().Contains(search)).ToList();
            }

            switch (sortBy)
            {
                case "Name":
                    insts = isAsc ? insts.OrderBy(e => e.Name).ToList() : insts.OrderByDescending(e => e.Name).ToList();
                    break;

                case "ShortName":
                    insts = isAsc ? insts.OrderBy(e => e.ShortName).ToList() : insts.OrderByDescending(e => e.ShortName).ToList();
                    break;

                case "IsActive":
                    insts = isAsc ? insts.OrderBy(e => e.IsActive).ToList() : insts.OrderByDescending(e => e.IsActive).ToList();
                    break;

                default:
                    insts = isAsc ? insts.OrderBy(e => e.Id).ToList() : insts.OrderByDescending(e => e.Id).ToList();
                    break;
            }

            var TotalPages = (int)Math.Ceiling((double)insts.Count() / pageSize);

            insts = insts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new { TotalPages = TotalPages, Insts = insts, CurrentPage = page, PageSize = pageSize, Search = search };
            return Json(result, JsonRequestBehavior.AllowGet);



        }

        public JsonResult GetData(int? Id)
        {
            if (Id.HasValue && Id.Value > 0)
            {
                EMSContext db = new EMSContext();
                var result = db.Institutes.Where(e => e.Id == Id.Value).SingleOrDefault();
                if (result != null)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }


            }
            return Json(new { status = "Failed" });
        }
        //[HttpPost]
        public JsonResult CreateData(Institute model)
        {
            string retn = String.Empty;
            EMSContext db = new EMSContext();
            var exist = db.Institutes.Where(e => e.ShortName == model.ShortName).FirstOrDefault();
            if (exist == null)
            {
                string Message, fileName, actualFileName;
                Message = fileName = actualFileName = string.Empty;
                bool flag = false;
                if (Request.Files != null)
                {
                    var file = Request.Files[0];
                    actualFileName = file.FileName;
                    fileName = model.ShortName + Path.GetExtension(file.FileName);
                    int size = file.ContentLength;
                }
                var advanced = new Institute();
                advanced.Name = model.Name;
                advanced.ShortName = model.ShortName;
                advanced.Address = model.Address;
                advanced.Email = model.Email;
                advanced.Phone = model.Phone;
                advanced.Mobile = model.Mobile;
                advanced.Contact = model.Contact;
                advanced.IsActive = model.IsActive;
                advanced.JoinDate = model.JoinDate;
                advanced.ExpireDate = Convert.ToDateTime(model.ExpireDate);
                advanced.ModifyId = CommonFunction.CurrentUserId();
                advanced.ModifyTime = DateTime.Now;
                db.Institutes.Add(advanced);
                db.SaveChanges();
                retn = "Success";
            }
            else
            {
                retn = "This Short Name is already exist.";
            }

            return Json(retn, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        public JsonResult UpdateData(Institute model)
        {
            string retn = String.Empty;
            using (EMSContext db = new EMSContext())
            {
                try
                {

                    if (model != null && model.Id > 0)
                    {
                        var exist = db.Institutes.Where(e => e.Id == model.Id).SingleOrDefault();
                        if (exist != null)
                        {
                            var duplicate = db.Institutes.Where(e => e.Id != model.Id && e.ShortName == model.ShortName).FirstOrDefault();
                            if (duplicate == null)
                            {
                                exist.Id = model.Id;
                                exist.Name = model.Name;
                                exist.ShortName = model.ShortName;
                                exist.Address = model.Address;
                                exist.Email = model.Email;
                                exist.Phone = model.Phone;
                                exist.Mobile = model.Mobile;
                                exist.Contact = model.Contact;
                                exist.IsActive = model.IsActive;
                                exist.JoinDate = model.JoinDate;
                                exist.ExpireDate = model.ExpireDate;
                                exist.ModifyId = CommonFunction.CurrentUserId();
                                exist.ModifyTime = DateTime.Now;
                                db.Institutes.Attach(exist);
                                db.Entry(exist).State = EntityState.Modified;
                                db.SaveChanges();
                                retn = "Success.";
                            }
                            else
                            {
                                retn = "This Short Name Already Exist.";
                            }

                        }
                        else
                        {
                            retn = "Institute Id is not found.";
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
                using (EMSContext db = new EMSContext())
                {
                    try
                    {
                        var exist = db.Institutes.Where(e => e.Id == Id).FirstOrDefault();
                        if (exist != null)
                        {
                            db.Institutes.Remove(exist);
                            db.SaveChanges();
                            retn = "Success.";
                        }
                        else
                        {
                            retn = "Institute Id is not found.";
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