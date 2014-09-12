using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IM.Identity.BI.Repository.Interface;
using IM.Identity.BI.Repository.NInject;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;

namespace IM.Identity.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Constants

        private const string AdminRoleName = "Admin";

        #endregion

        private readonly IRoleIdentityRepository<IdentityRole> _rolesRepository;

        public HomeController()
        {
            var kernel = new StandardKernel(new RepositoryModule());
            _rolesRepository = kernel.Get<IRoleIdentityRepository<IdentityRole>>();
        }

        public async Task<ActionResult> Index()
        {
            var roleExists = await _rolesRepository.RoleExists(AdminRoleName);
            if (!roleExists)
            {
                return RedirectToAction("Register", "Account");
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}