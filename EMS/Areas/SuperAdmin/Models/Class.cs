using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Areas.SuperAdmin.Models
{
    public class Class
    {
        public int Id { get; set; }
        public int FormatId { get; set; }
        public string Name { get; set; }
        public string ModifyId { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}