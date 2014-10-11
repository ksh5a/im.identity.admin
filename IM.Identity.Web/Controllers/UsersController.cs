using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using IM.Identity.BI.Enums;
using IM.Identity.BI.Errors;
using IM.Identity.BI.Models;
using IM.Identity.BI.Repository.Interface;
using IM.Identity.BI.Repository.NInject;
using IM.Identity.Web.Code.Managers;
using IM.Identity.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;

namespace IM.Identity.Web.Controllers
{
    [Authorize(Roles = RoleConstants.AdminRoles)]
    public class UsersController : BaseAccountController
    {
        private readonly IUserIdentityRepository<ApplicationUser> _usersRepository;
        private readonly IIdentityRepository<IdentityRole> _rolesRepository;

        public UsersController()
        {
            var kernel = new StandardKernel(new RepositoryModule());
            _usersRepository = kernel.Get<IUserIdentityRepository<ApplicationUser>>();
            _rolesRepository = kernel.Get<IIdentityRepository<IdentityRole>>();
        }

        public ActionResult Index()
        {
            var users = _usersRepository.Get();

            return View(users);
        }

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
                FirstName = user.FirstName,
                LastName = user.LastName,
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
                    UserName = userViewModel.Email,
                    Email = userViewModel.Email,
                    FirstName = userViewModel.FirstName,
                    LastName = userViewModel.LastName,
                    PhoneNumber = userViewModel.PhoneNumber,
                    LockoutEnabled = userViewModel.LockoutEnabled
                };

                var result = await _usersRepository.Insert(user);
                if (result.Succeeded)
                {
                    if (!await UpdateUserRoles(userViewModel, user))
                    {
                        return View(userViewModel);
                    }
                }
                else
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View(userViewModel);
                }

                await SendEmailConfirmation(user.Id, "Confirm your account");

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
                FirstName = user.FirstName,
                LastName = user.LastName,
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
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserName,Email,FirstName,LastName,PhoneNumber,LockoutEnabled,RoleViewModels")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _usersRepository.Get(userViewModel.Id);
                user.UserName = userViewModel.Email;
                user.Email = userViewModel.Email;
                user.FirstName = userViewModel.FirstName;
                user.LastName = userViewModel.LastName;
                user.PhoneNumber = userViewModel.PhoneNumber;
                user.LockoutEnabled = userViewModel.LockoutEnabled;

                var result = await _usersRepository.Update(user);
                if (result.Succeeded)
                {
                    if (!await UpdateUserRoles(userViewModel, user))
                    {
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
                var allRoles = _rolesRepository.Get().ToList();
                var userRoles = _usersRepository.GetUserRoles(user.Id).ToList();

                roleViewModels = (
                    from IdentityRole identityRole in allRoles
                    select new IdentityRoleViewModel
                    {
                        RoleId = identityRole.Id,
                        RoleName = identityRole.Name,
                        HasRole = userRoles.Select(x => x.Id).Contains(identityRole.Id)
                    }).ToList();
            }

            return roleViewModels;
        }

        private async Task<bool> UpdateUserRoles(UserViewModel userViewModel, ApplicationUser user)
        {
            var addRoles = userViewModel.RoleViewModels.Where(x => x.HasRole).Select(x => x.RoleName).ToArray();

            foreach (var role in addRoles)
            {
                var result = await _usersRepository.AddToRole(user, role);
                var userAlreadyInRoleErrors = result.Errors.Any() &&
                                              result.Errors.Select(x => x == RoleErrors.UserAlreadyInRole).Count() ==
                                              result.Errors.Count();
                if (!result.Succeeded && !userAlreadyInRoleErrors)
                {
                    AddErrors(new IdentityResult(result.Errors));
                    return false;
                }
            }

            var removeRoles = userViewModel.RoleViewModels.Where(x => !x.HasRole).Select(x => x.RoleName).ToArray();
            foreach (var role in removeRoles)
            {

                var result = await _usersRepository.RemoveFromRole(user, role);
                var userIsNotInRoleErrors = result.Errors.Any() &&
                                            result.Errors.Select(x => x == RoleErrors.UserIsNotInRole).Count() ==
                                            result.Errors.Count();
                if (!result.Succeeded && !userIsNotInRoleErrors)
                {
                    AddErrors(new IdentityResult(result.Errors));
                    return false;
                }
            }

            return true;
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private async Task<string> SendEmailConfirmation(string userId, string subject)
        {
            var emailManager = new EmailManager(UserManager);
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = Url.Action("ConfirmEmail", "Account",
               new { userId = userId, code = code }, protocol: Request.Url.Scheme);

            return await emailManager.SendConfirmationEmail(userId, subject, callbackUrl);
        }

        #endregion

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _usersRepository.Dispose();
                _rolesRepository.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
