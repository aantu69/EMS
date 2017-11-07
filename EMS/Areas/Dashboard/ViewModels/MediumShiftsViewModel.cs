using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Areas.Dashboard.ViewModels
{
    public class MediumShiftsViewModel
    {
        public int Id { get; set; }
        public string MediumName { get; set; }
        public IEnumerable<string> ShiftName { get; set; }
    }
}