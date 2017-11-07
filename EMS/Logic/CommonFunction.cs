using EMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace EMS.Logic
{
    public static class CommonFunction
    {
        static CommonFunction()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        

        public static string CurrentUserId()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }
        public static string CurrentUserName()
        {
            string CurrentUserName = "";
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            var currentUser = userManager.FindById(currentUserId);
            if (currentUser != null)
            {
                CurrentUserName = currentUser.FirstName + " " + currentUser.LastName;
            }

            return CurrentUserName;
        }

        public static int CurrentUserInstituteId()
        {
            int instituteId = 0;
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            var currentUser = userManager.FindById(currentUserId);
            if (currentUser != null)
            {
                instituteId = currentUser.InstId;
            }

            return instituteId;
        }
        //public static string CurrentUserInstituteId()
        //{
        //    string instituteId = "";
        //    ApplicationDbContext db = new ApplicationDbContext();
        //    UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        //    var currentUserId = HttpContext.Current.User.Identity.GetUserId();
        //    var currentUser = userManager.FindById(currentUserId);
        //    if (currentUser != null)
        //    {
        //        instituteId = currentUser.InstId.ToString();
        //    }

        //    return instituteId;
        //}
    }
}