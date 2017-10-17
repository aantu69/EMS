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

namespace EMS.Areas.SuperAdmin.Controllers
{
    public class EmployeesController : Controller
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

        public JsonResult GetDatas(int page = 1, int pageSize = 10, string sortBy = "Name", bool isAsc = true, string search = null)
        {
            using (EMSContext db = new EMSContext())
            {
                var employees = db.Employees.ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.ToLower();
                    employees = employees.Where(x =>
                        x.Name.ToLower().Contains(search) ||
                        x.Address.ToLower().Contains(search)).ToList();
                }

                switch (sortBy)
                {
                    case "Name":
                        employees = isAsc ? employees.OrderBy(e => e.Name).ToList() : employees.OrderByDescending(e => e.Name).ToList();
                        break;
                    case "Address":
                        employees = isAsc ? employees.OrderBy(e => e.Address).ToList() : employees.OrderByDescending(e => e.Address).ToList();
                        break;
                    default:
                        employees = isAsc ? employees.OrderBy(e => e.ID).ToList() : employees.OrderByDescending(e => e.ID).ToList();
                        break;
                }

                var TotalPages = (int)Math.Ceiling((double)employees.Count() / pageSize);

                employees = employees
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var result = new { TotalPages = TotalPages, Employees = employees, CurrentPage = page, PageSize = pageSize, Search = search };
                return Json(result, JsonRequestBehavior.AllowGet);

            }      
       
        }

        public JsonResult GetData(int? id)
        {
            if (id.HasValue && id.Value > 0)
            {
                using (EMSContext db = new EMSContext())
                {
                    var result = db.Employees.Where(e => e.ID == id.Value).SingleOrDefault();
                    if (result != null)
                    {
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                

            }
            return Json(new { status = "Failed" });
        }
        //[HttpPost]
        public JsonResult CreateData(Employee employee)
        {
            string retn = String.Empty;
            using (EMSContext db = new EMSContext())
            {
                try
                {
                    var currentEmp = db.Employees.Where(e => e.Name == employee.Name).FirstOrDefault();
                    if (currentEmp == null)
                    {
                        db.Employees.Add(employee);
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
        public JsonResult UpdateData(Employee employee)
        {
            string retn = String.Empty;
            using (EMSContext db = new EMSContext())
            {
                try
                {

                    if (employee != null && employee.ID > 0)
                    {
                        var CurrentEmployee = db.Employees.Where(e => e.ID == employee.ID).SingleOrDefault();
                        if (CurrentEmployee != null)
                        {
                            var UpdateEmployee = db.Employees.Where(e => e.ID != employee.ID && e.Name == employee.Name).SingleOrDefault();
                            if (UpdateEmployee == null)
                            {
                                CurrentEmployee.ID = employee.ID;
                                CurrentEmployee.Name = employee.Name;
                                CurrentEmployee.Address = employee.Address;
                                db.Employees.Attach(CurrentEmployee);
                                db.Entry(CurrentEmployee).State = EntityState.Modified;
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
                            retn = "Employee ID is not found.";
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
                        var emp = db.Employees.Where(e => e.ID == id).FirstOrDefault();
                        if (emp != null)
                        {
                            db.Employees.Remove(emp);
                            db.SaveChanges();
                            retn = "Success.";
                        }
                        else
                        {
                            retn = "Employee ID is not found.";
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
