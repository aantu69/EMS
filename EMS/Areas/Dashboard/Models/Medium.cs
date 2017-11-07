using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EMS.Areas.Dashboard.Models
{
    [Table("Mediums")]
    public class Medium
    {
        public int Id { get; set; }
        public int InstId { get; set; }
        public string MediumName { get; set; }
        public string ModifyId { get; set; }
        public DateTime ModifyTime { get; set; }

        public virtual ICollection<Shift> Shifts { get; set; }
    }
}