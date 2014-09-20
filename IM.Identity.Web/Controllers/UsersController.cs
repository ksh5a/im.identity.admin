using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IM.Identity.BI.Models;
using IM.Identity.BI.Repository.Interface;
using IM.Identity.BI.Repository.NInject;
using IM.Identity.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Ninject;

namespace IM.Identity.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserIdentityRepository<ApplicationUser> _usersRepository;
        private readonly IIdentityRepository<IdentityRole> _rolesRepository;
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public UsersController()
        {
            var kernel = new StandardKernel(new RepositoryModule());
            _usersRepository = kernel.Get<IUserIdentityRepository<ApplicationUser>>();
            _rolesRepository = kernel.Get<IIdentityRepository<IdentityRole>>();
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

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEndDateUtc = user.LockoutEndDateUtc,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                AccessFailedCount = user.AccessFailedCount,
                RoleViewModels = GetRoleViewModels(user)
            };

            return View(userViewModel);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            var userViewModel = new UserViewModel
            {
                RoleViewModels = GetRoleViewModels()
            };

            return View(userViewModel);
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
                    LockoutEnabled = userViewModel.LockoutEnabled
                };

                var result = await _usersRepository.Insert(user);
                if (result.Succeeded)
                {
                    result = await _usersRepository.AddToRoles(user.Id, userViewModel.RoleViewModels.Where(x => x.HasRole).Select(x => x.RoleName));
                    if (!result.Succeeded)
                    {
                        AddErrors(new IdentityResult("User could not be added to roles"));
                        return View(userViewModel);
                    }
                }
                else 
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View(userViewModel);
                }

                var callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account");

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

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                LockoutEnabled = user.LockoutEnabled,
                RoleViewModels = GetRoleViewModels(user)
            };

            return View(userViewModel);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserName,Email,PhoneNumber,LockoutEnabled,RoleViewModels")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _usersRepository.Get(userViewModel.Id);
                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email;
                user.PhoneNumber = userViewModel.PhoneNumber;
                user.LockoutEnabled = userViewModel.LockoutEnabled;

                var result = await _usersRepository.Update(user);
                if (result.Succeeded)
                {
                    result = await _usersRepository.AddToRoles(user.Id, userViewModel.RoleViewModels.Where(x => x.HasRole).Select(x => x.RoleName));
                    if (!result.Succeeded)
                    {
                        AddErrors(new IdentityResult("User could not be added to roles"));
                        return View(userViewModel);
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View(userViewModel);
                }
            }

            return View(userViewModel);
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

        private IList<IdentityRoleViewModel> GetRoleViewModels(ApplicationUser user = null)
        {
            IList<IdentityRoleViewModel> roleViewModels;

            if (user == null)
            {
                var allRoles = _rolesRepository.Get();
                roleViewModels = (
                    from IdentityRole identityRole in allRoles
                    select new IdentityRoleViewModel
                    {
                        RoleId = identityRole.Id,
                        RoleName = identityRole.Name,
                    }).ToList();
            }
            else
            {
                var allRoles = _rolesRepository.Get();
                var userRoleIds = user.Roles.Select(x => x.RoleId);
                roleViewModels = (
                    from IdentityRole identityRole in allRoles
                    select new IdentityRoleViewModel
                    {
                        RoleId = identityRole.Id,
                        RoleName = identityRole.Name,
                        HasRole = userRoleIds.Contains(identityRole.Id)
                    }).ToList();
            }

            return roleViewModels;
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public async Task<string> SendEmailConfirmationTokenAsync(string userId, string subject)
        {
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(userId);

            var callbackUrl = Url.Action("ConfirmEmail", "Account",
               new { userId = userId, code = code }, protocol: Request.Url.Scheme);

            await UserManager.SendEmailAsync(userId, subject,
               "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            return callbackUrl;
        }

        #endregion

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            _usersRepository.Dispose();

            base.Dispose(disposing);
        }

        #endregion
    }
}
