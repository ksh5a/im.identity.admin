using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using IM.Identity.BI.Service;
using IM.Identity.BI.Service.Manager;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IM.Identity.Web.Controllers
{
    public class RolesController : Controller
    {
        private IdentityDbContext db = new IdentityDbContext();

        // GET: Roles
        public async Task<ActionResult> Index()
        {
            var rolesService = (RolesService)ServiceManager.GetServiceInstance(typeof(RolesService));
            var roles = await rolesService.Get();

            return View(roles);
        }

        // GET: Roles/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var rolesService = (RolesService)ServiceManager.GetServiceInstance(typeof(RolesService));
            var role = await rolesService.Get(id);
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
                var rolesService = (RolesService)ServiceManager.GetServiceInstance(typeof(RolesService));
                var result = await rolesService.Insert(aspNetRole);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First().ToString());
                    return View();
                }
                return RedirectToAction("Index");
            }

            return View(aspNetRole);
        }

        // GET: Roles/Edit/5
//        public async Task<ActionResult> Edit(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetRole aspNetRole = await db.AspNetRoles.FindAsync(id);
//            if (aspNetRole == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetRole);
//        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] AspNetRole aspNetRole)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(aspNetRole).State = EntityState.Modified;
//                await db.SaveChangesAsync();
//                return RedirectToAction("Index");
//            }
//            return View(aspNetRole);
//        }

        // GET: Roles/Delete/5
//        public async Task<ActionResult> Delete(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetRole aspNetRole = await db.AspNetRoles.FindAsync(id);
//            if (aspNetRole == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetRole);
//        }

        // POST: Roles/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> DeleteConfirmed(string id)
//        {
//            AspNetRole aspNetRole = await db.AspNetRoles.FindAsync(id);
//            db.AspNetRoles.Remove(aspNetRole);
//            await db.SaveChangesAsync();
//            return RedirectToAction("Index");
//        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
