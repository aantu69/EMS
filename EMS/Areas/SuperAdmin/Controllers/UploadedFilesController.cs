using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EMS.Areas.SuperAdmin.Models;
using EMS.Models;
using System.IO;

namespace EMS.Areas.SuperAdmin.Controllers
{
    public class UploadedFilesController : Controller
    {
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveFiles(UploadedFile model)
        {
            string Message, fileName, actualFileName;
            Message = fileName = actualFileName = string.Empty;
            bool flag = false;
            if (Request.Files != null)
            {
                var file = Request.Files[0];
                actualFileName = file.FileName;
                fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                int size = file.ContentLength;

                try
                {
                    file.SaveAs(Path.Combine(Server.MapPath("~/images/insts"), fileName));

                    UploadedFile f = new UploadedFile
                    {
                        FileName = actualFileName,
                        FilePath = fileName,
                        Description = model.Description,
                        FileSize = size
                    };
                    using (EMSContext dc = new EMSContext())
                    {
                        dc.UploadedFiles.Add(f);
                        dc.SaveChanges();
                        Message = "File uploaded successfully";
                        flag = true;
                    }
                }
                catch (Exception)
                {
                    Message = "File upload failed! Please try again";
                }

            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }
    }
}
