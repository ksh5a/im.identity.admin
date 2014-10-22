using System.Web.Mvc;
using IM.Identity.BI.Enums;

namespace IM.Identity.Web.Controllers
{
    [Authorize(Roles = RoleConstants.AdminRoles)]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}