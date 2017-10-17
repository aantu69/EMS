using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeUserAttribute : AuthorizeAttribute
    {
        public string Permissions { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            EMSContext db = new EMSContext();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationDbContext dbu = new ApplicationDbContext();

            bool isUserAuthorized = base.AuthorizeCore(httpContext);

            string[] permissions = Permissions.Split(',').ToArray();

            //IEnumerable<string> perms = permissions.Intersect(db.Permissions.Select(p => p.ActionName));
            //List<IdentityRole> roles = new List<IdentityRole>();

            //if (perms.Count() > 0)
            //{
            //    foreach (var item in perms)
            //    {
            //        var currentUserId = httpContext.User.Identity.GetUserId();
            //        var relatedPermisssionRole = dbu.Roles.Find(db.Permissions.Single(p => p.ActionName == item).RoleId).Name;
            //        if (userManager.IsInRole(currentUserId, relatedPermisssionRole))
            //        {
            //            return true;
            //        }
            //    }
            //}
            return false;
        }
    }
}