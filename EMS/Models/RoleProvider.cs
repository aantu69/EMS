using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class RoleProvider
    {
        public string[] GetRoles(string controller, string action)
        {
            // get your roles based on the controller and the action name 
            // wherever you want such as db
            // I hardcoded for the sake of simplicity 
            ApplicationDbContext db = new ApplicationDbContext();

            var results = (from r in db.Roles
                           join rm in db.ApplicationRoleMenus on r.Id equals rm.ApplicationRoleId
                           join m in db.Menus on rm.MenuId equals m.Id
                           where m.ControllerName == controller && m.ActionName == action
                           select new
                           {
                               r.Name,
                               m.ControllerName,
                               m.ActionName
                           }).ToList();

            List<string> list = new List<string>();
            foreach (var result in results)
            {
                list.Add(result.Name);
            }
            //list.Add("Admin");
            
            string[] roles = list.ToArray();

            //return new string[] { "Admin", "Teacher" };
            return roles;
        }
    }
}