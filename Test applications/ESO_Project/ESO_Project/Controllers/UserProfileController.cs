using ESO_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

namespace ESO_Project.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        public UserProfileController()
        {
        }

        public UserProfileController(ApplicationUserManager userManager, ApplicationRoleManager roleManager, ApplicationUser user)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            UserSetter = user;
        }

        private ApplicationUserManager _userManager;

        private ApplicationUser _user;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        public ApplicationUser UserSetter
        {
            get
            {
                return _user ?? HttpContext.GetOwinContext().Get<ApplicationUser>();
            }
            private set
            {
                _user = value;
            }
        }

        //
        // GET: /Users/
        public async Task<ActionResult> Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(db);

            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
            ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);
            return View(user);
        }

        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(db);
            var roleStore = new RoleStore<ApplicationRole>(db);

            var userManager = new UserManager<ApplicationUser>(userStore);
            var roleManager = new RoleManager<ApplicationRole>(roleStore);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await userManager.FindByIdAsync(id);

            ViewBag.RoleNames = await userManager.GetRolesAsync(user.Id);

            return View(user);
        }
    }
}