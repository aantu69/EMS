using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Areas.SuperAdmin.Models
{
    public class UploadedFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public int FileSize { get; set; }
    }
}