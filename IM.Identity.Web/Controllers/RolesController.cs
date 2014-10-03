using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using IM.Identity.BI.Enums;
using IM.Identity.BI.Repository.Interface;
using IM.Identity.BI.Repository.NInject;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;

namespace IM.Identity.Web.Controllers
{
    [Authorize(Roles = RoleConstants.AdminRoles)]
    public class RolesController : Controller
    {
        private readonly IIdentityRepository<IdentityRole> _rolesRepository;

        public RolesController()
        {
            var kernel = new StandardKernel(new RepositoryModule());
            _rolesRepository = kernel.Get<IIdentityRepository<IdentityRole>>();
        }

        // GET: Roles
        public ActionResult Index()
        {
            var roles = _rolesRepository.Get();

            return View(roles);
        }

        // GET: Roles/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var role = await _rolesRepository.Get(id);
            if (role == null)
            {
                return HttpNotFound();
            }

            return View(role);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] IdentityRole aspNetRole)
        {
            if (ModelState.IsValid)
            {
                var result = await _rolesRepository.Insert(aspNetRole);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }

                return RedirectToAction("Index");
            }

            return View(aspNetRole);
        }

        // GET: Roles/Edit/MyRole
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var role = await _rolesRepository.Get(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }


        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                var result = await _rolesRepository.Update(role);

                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(role);
        }

        // GET: Roles/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var role = await _rolesRepository.Get(id);

            if (role == null)
            {
                return HttpNotFound();
            }

            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var result = await _rolesRepository.Delete(id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _rolesRepository.Dispose();

            base.Dispose(disposing);
        }
    }
}
