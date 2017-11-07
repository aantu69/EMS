using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Areas.Dashboard.Models
{
    public class Shift
    {
        public int Id { get; set; }
        public int InstId { get; set; }
        public int MediumId { get; set; }
        public string ShiftName { get; set; }
        public string ModifyId { get; set; }
        public DateTime ModifyTime { get; set; }

        public Medium Medium { get; set; }

    }
}