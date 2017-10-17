using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EMS.Areas.SuperAdmin.Models
{
    [Table("Mediums")]
    public class Medium
    {
        public int ID { get; set; }
        public string MediumName { get; set; }
        public string ModifyID { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}