using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Areas.SuperAdmin.Models
{
    public class Shift
    {
        public int ID { get; set; }
        public string ShiftName { get; set; }
        public string ModifyID { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}