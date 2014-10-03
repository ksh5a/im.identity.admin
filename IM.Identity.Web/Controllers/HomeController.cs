using System.Web.Mvc;
using IM.Identity.BI.Enums;
using IM.Identity.BI.Repository.Interface;
using IM.Identity.BI.Repository.NInject;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;

namespace IM.Identity.Web.Controllers
{
    [Authorize(Roles = RoleConstants.AdminRoles)]
    public class HomeController : Controller
    {
        public HomeController()
        {
            var kernel = new StandardKernel(new RepositoryModule());
            kernel.Get<IRoleIdentityRepository<IdentityRole>>();
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}