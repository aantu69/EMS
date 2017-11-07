using EMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Areas.SuperAdmin.Models
{
    public class ApplicationRoleMenu
    {
        public int ApplicationRoleMenuId { get; set; }
        public string ApplicationRoleId { get; set; }
        public int MenuId { get; set; }

        //public virtual ApplicationRole ApplicationRole { get; set; }
        //public virtual Menu Menu { get; set; }
    }
}