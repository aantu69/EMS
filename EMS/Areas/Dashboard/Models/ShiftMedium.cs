using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Areas.Dashboard.Models
{
    public class ShiftMedium
    {
        public int ShiftMediumId { get; set; }
        public int ShiftId { get; set; }
        public int MediumId { get; set; }

    }
}