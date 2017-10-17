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

            List<string> list = new List<string>();
            //list.Add("Admin");
            list.Add("Teacher");
            string[] roles = list.ToArray();

            //return new string[] { "Admin", "Teacher" };
            return roles;
        }
    }
}