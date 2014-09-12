using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using IM.Identity.BI.Models;
using IM.Identity.BI.Repository.Interface;
using IM.Identity.BI.Repository.NInject;
using IM.Identity.Web.Models;
using Ninject;

namespace IM.Identity.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserIdentityRepository<ApplicationUser> _usersRepository;

        public UsersController()
        {
            var kernel = new StandardKernel(new RepositoryModule());
            _usersRepository = kernel.Get<IUserIdentityRepository<ApplicationUser>>();
        }

        // GET: Users
        //[Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var users = _usersRepository.Get();

            return View(users);
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await _usersRepository.Get(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = userViewModel.UserName,
                    Email = userViewModel.Email,
                    PhoneNumber = userViewModel.PhoneNumber,
                    TwoFactorEnabled = userViewModel.TwoFactorEnabled,
                    LockoutEnabled = userViewModel.LockoutEnabled
                };

                var result = await _usersRepository.Insert(user, userViewModel.Password);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }

                return RedirectToAction("Index");
            }

            return View(userViewModel);
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await _usersRepository.Get(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserName,Email,PhoneNumber,TwoFactorEnabled,LockoutEnabled")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                var user = await _usersRepository.Get(applicationUser.Id);
                user.UserName = applicationUser.UserName;
                user.Email = applicationUser.Email;
                user.PhoneNumber = applicationUser.PhoneNumber;
                user.TwoFactorEnabled = applicationUser.TwoFactorEnabled;
                user.LockoutEnabled = applicationUser.LockoutEnabled;

                var result = await _usersRepository.Update(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(applicationUser);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await _usersRepository.Get(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var result = await _usersRepository.Delete(id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _usersRepository.Dispose();

            base.Dispose(disposing);
        }
    }
}
