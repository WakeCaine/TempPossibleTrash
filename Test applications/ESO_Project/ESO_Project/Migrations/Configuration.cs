namespace ESO_Project.Migrations
{
    using ESO_Project.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;

    internal sealed class Configuration : DbMigrationsConfiguration<ESO_Project.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ESO_Project.Models.ApplicationDbContext";
        }

        protected override void Seed(ESO_Project.Models.ApplicationDbContext context)
        {
            try
            {
                //Passy admina
                //name = "admin@example.com";
                //password = "Admin@123456";

                SeedingDatabase seedClass = new SeedingDatabase(context);

                //Dodanie przyk³adowego admina
                SeedingDatabase.InitializeIdentityForEF(context);

                //Testing users /username/phone/email/password
                seedClass.SeedUsers("dj@dj.com", "0797697898", "dj@dj.com", "pass@123","Premium");
                seedClass.SeedUsers("test@dj.com", "0797697898", "test@dj.com", "pass@123", "User");
                seedClass.SeedUsers("test2@dj.com", "0797697898", "test2@dj.com", "pass@123", "User");
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

        }
    }

    public class SeedingDatabase
    {
        //Current Directory
        String Root;
        //Lorem Ipsum Text
        string text;

        //New context
        ApplicationDbContext db;
        //Controllers/Types

        public SeedingDatabase(ApplicationDbContext context)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            Root = Directory.GetCurrentDirectory();
            db = context;
        }

        public void SeedUsers(string username, string phone, string email, string password , string roleName)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var roleStore = new RoleStore<ApplicationRole>(db);

            var userManager = new UserManager<ApplicationUser>(userStore);
            var roleManager = new RoleManager<ApplicationRole>(roleStore);

            var user = userManager.FindByName(username);
            if (user == null)
            {
                user = new ApplicationUser { UserName = username, PhoneNumber = phone, Email = email };
                user.RegisterDateTime = DateTime.Now;
                user.ActiveFlag = 1;
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, true);
            }
          
            //Create Role User if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            // Add user to Role User if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
        }

        public static void InitializeIdentityForEF(ApplicationDbContext db)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var roleStore = new RoleStore<ApplicationRole>(db);

            var userManager = new UserManager<ApplicationUser>(userStore);
            var roleManager = new RoleManager<ApplicationRole>(roleStore);
            const string name = "admin@example.com";
            const string password = "Admin@123456";
            string roleName = "Admin";
            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);

            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name };
                user.RegisterDateTime = DateTime.Now;
                user.ActiveFlag = 1;
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
        }

        public void AddUserRoleToExampleAdmin()
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var roleStore = new RoleStore<ApplicationRole>(db);

            var userManager = new UserManager<ApplicationUser>(userStore);
            var roleManager = new RoleManager<ApplicationRole>(roleStore);
            const string name = "admin@example.com";
            string roleName = "User";
            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
        }

    }
}
