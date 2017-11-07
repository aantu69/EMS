using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class EMSContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public EMSContext() : base("name=EMSContext")
        {
        }

        public System.Data.Entity.DbSet<EMS.Areas.SuperAdmin.Models.Employee> Employees { get; set; }
        public System.Data.Entity.DbSet<EMS.Areas.SuperAdmin.Models.Institute> Institutes { get; set; }
        public System.Data.Entity.DbSet<EMS.Areas.SuperAdmin.Models.ClassFormat> ClassFormats { get; set; }
        public System.Data.Entity.DbSet<EMS.Areas.SuperAdmin.Models.Class> Classes { get; set; }


        public System.Data.Entity.DbSet<EMS.Areas.Dashboard.Models.Shift> Shifts { get; set; }
        public System.Data.Entity.DbSet<EMS.Areas.Dashboard.Models.Medium> Mediums { get; set; }
        public System.Data.Entity.DbSet<EMS.Areas.Dashboard.Models.ShiftMedium> ShiftsMediums { get; set; }


        public System.Data.Entity.DbSet<EMS.Areas.Dashboard.Models.Test> Tests { get; set; }

        public System.Data.Entity.DbSet<EMS.Models.Country> Countries { get; set; }

        public System.Data.Entity.DbSet<EMS.Models.State> States { get; set; }

        public System.Data.Entity.DbSet<EMS.Areas.SuperAdmin.Models.UploadedFile> UploadedFiles { get; set; }
    }
}
