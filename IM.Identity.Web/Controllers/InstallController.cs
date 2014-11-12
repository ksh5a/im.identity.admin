using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using IM.Identity.BI.Enums;
using IM.Identity.BI.Models;
using IM.Identity.BI.Repository.Interface;
using IM.Identity.BI.Repository.NInject;
using IM.Identity.Web.Models;
using IM.Identity.Web.Resources;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;

namespace IM.Identity.Web.Controllers
{
    public class InstallController : BaseAccountController
    {
        #region Members

        private readonly IRoleIdentityRepository<IdentityRole> _rolesRepository;
        private readonly IUserIdentityRepository<ApplicationUser> _usersRepository;

        #endregion

        public InstallController()
        {
            var kernel = new StandardKernel(new RepositoryModule());
            _rolesRepository = kernel.Get<IRoleIdentityRepository<IdentityRole>>();
            _usersRepository = kernel.Get<IUserIdentityRepository<ApplicationUser>>();
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            // Self registration is active as long as the administrator was not created yet
            var roleExists = await _rolesRepository.RoleExists(RoleConstants.SuperAdminRole);
            if (roleExists)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Index(InstallViewModel model)
        {
            // Self registration is active as long as the administrator was not created yet
            var roleExists = await _rolesRepository.RoleExists(RoleConstants.SuperAdminRole);
            if (roleExists)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var user = _usersRepository.Get().SingleOrDefault(x => x.Email == model.Email);
                if(user == null)
                {
                    user = new ApplicationUser
                    {
                        Email = model.Email,
                        UserName = model.Email,
                    };

                    var userResult = await UserManager.CreateAsync(user, model.Password);
                    if (userResult.Succeeded)
                    {
                        await SendAdminEmailConfirmation(user.Id, "Confirm your account");

                        ViewBag.Message = ViewResource.RegisterConfirmationMessage;
                        return View("Info");
                    }
                    else
                    {
                        AddErrors(userResult);
                    }
                }
                else
                {
                    await SendAdminEmailConfirmation(user.Id, "Confirm your account");

                    ViewBag.Message = ViewResource.RegisterConfirmationMessage;
                    return View("Info");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private async Task<string> SendAdminEmailConfirmation(string userId, string subject)
        {
            return await SendEmailConfirmation(userId, subject, "ConfirmEmail", "Install");
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            var user = await _usersRepository.Get(userId);

            var roleExists = await _rolesRepository.RoleExists(RoleConstants.SuperAdminRole);
            if (!roleExists)
            {
                var roleResult = await _rolesRepository.CreateAdministrationRoles();
                if (!roleResult.Succeeded)
                {
                    AddErrors(new IdentityResult("Admin roles could not be created"));
                    throw (new Exception(ViewResource.ConfirmEmailError));
                }

                var addToRoleResult = await _usersRepository.AddToRoles(user, RoleConstants.SuperAdminRole);
                if (!addToRoleResult.Succeeded)
                {
                    AddErrors(new IdentityResult("User could not be added to Admin role"));
                    throw (new Exception(ViewResource.ConfirmEmailError));
                }
            }

            var userAuthorized = await UserManager.AuthorizeAdminUser(user.Id);
            if (!userAuthorized)
            {
                throw (new Exception(ViewResource.ErrorAccessDenied));
            }

            var result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }
            else
            {
                throw (new Exception(ViewResource.ConfirmEmailError));
            }
        }
    }
}