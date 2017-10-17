using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.Models
{
    public class DynamicRoleAuthorizeAttribute : AuthorizeAttribute
    {
        RoleProvider roleProvider = new RoleProvider();
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var controller = httpContext.Request.RequestContext.RouteData.GetRequiredString("controller");
            var action = httpContext.Request.RequestContext.RouteData.GetRequiredString("action");
            // feed the roles here
            Roles = string.Join(",", roleProvider.GetRoles(controller, action));
            return base.AuthorizeCore(httpContext);
        }
    }
}