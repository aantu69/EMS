using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Areas.SuperAdmin.Models
{
    public class Institute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Address { get; set; }
        public string Logo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Contact { get; set; }
        public bool IsActive { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string ModifyId { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}