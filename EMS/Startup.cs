using EMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EMS.Startup))]
namespace EMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesAndUsers();
        }

        private void createRolesAndUsers()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(db));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));


            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("SuperAdmin"))
            {

                // first we create Admin rool    
                var role = new ApplicationRole();
                role.Name = "SuperAdmin";
                role.Description = "All Access";
                roleManager.Create(role);

            }

            //Here we create a SuperAdmin super user who will maintain the website 
            var user = new ApplicationUser();
            user.UserName = "aantu69@yahoo.com";
            user.Email = "aantu69@yahoo.com";
            user.FirstName = "Sohrab";
            user.LastName = "Hossan";

            string userPassword = "S187580884t@";

            var currentUser = UserManager.FindByEmail("aantu69@yahoo.com");
            if (currentUser == null)
            {
                var chkUser = UserManager.Create(user, userPassword);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "SuperAdmin");

                }
            }

            // creating Creating Admin role     
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new ApplicationRole();
                role.Name = "Admin";
                role.Description = "Admin Access";
                roleManager.Create(role);

            }

            var user2 = new ApplicationUser();
            user2.UserName = "admin@yahoo.com";
            user2.Email = "admin@yahoo.com";
            user2.FirstName = "Sohrab";
            user2.LastName = "Hossan";

            var userPassword2 = "S187580884t@";

            var currentUser2 = UserManager.FindByEmail("admin@yahoo.com");
            if (currentUser2 == null)
            {
                var chkUser = UserManager.Create(user2, userPassword2);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user2.Id, "Admin");

                }
            }

            // creating Creating Teacher role     
            if (!roleManager.RoleExists("Teacher"))
            {
                var role = new ApplicationRole();
                role.Name = "Teacher";
                role.Description = "Teacher Access";
                roleManager.Create(role);

            }

            var user3 = new ApplicationUser();
            user3.UserName = "teacher@yahoo.com";
            user3.Email = "teacher@yahoo.com";
            user3.FirstName = "Sohrab";
            user3.LastName = "Hossan";

            //var userPassword3 = "S187580884t@";

            //var currentUser3 = UserManager.FindByEmail("teacher@yahoo.com");
            //if (currentUser3 == null)
            //{
            //    var chkUser = UserManager.Create(user3, userPassword3);

            //    //Add default User to Role Admin    
            //    if (chkUser.Succeeded)
            //    {
            //        var result1 = UserManager.AddToRole(user3.Id, "Teacher");

            //    }
            //}
        }
    }
}
