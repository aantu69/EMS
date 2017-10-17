using EMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Areas.SuperAdmin.Models
{
    public class Menu
    {
        public Menu()
        {
            ApplicationRole = new HashSet<ApplicationRole>();
        }
        public int ID { get; set; }
        public string Area { get; set; }
        public string Name { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
        public string ModifyID { get; set; }
        public DateTime ModifyTime { get; set; }
        public virtual ICollection<ApplicationRole> ApplicationRole { get; set; }
    }
}