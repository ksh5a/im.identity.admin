using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using IM.Identity.BI.Edm;
using IM.Identity.BI.Models;
using IM.Identity.BI.Repository.Interface;
using IM.Identity.BI.Repository.NInject;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;

namespace IM.Identity.BI.Repository
{
    public class UsersRepository : BaseRepository, IUserIdentityRepository<ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        [Inject]
        public UsersRepository(ApplicationDbContext context)
            : base(context)
        {
            var roleStore = new UserStore<ApplicationUser>(context);
            _userManager = new UserManager<ApplicationUser>(roleStore);
        }

        public IQueryable<ApplicationUser> Get()
        {
            var users = _userManager.Users;
            foreach (var user in users)
            {
                user.UserRoles = GetUserRoles(user.Id);
            }

            return users;
        }

        public async Task<ApplicationUser> Get(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.UserRoles = GetUserRoles(user.Id);

            return user;
        }

        public async Task<IdentityResult> Insert(ApplicationUser user)
        {
            var result = await _userManager.CreateAsync(user);

            return result;
        }

        public async Task<IdentityResult> Insert(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            return result;
        }

        public async Task<IdentityResult> Update(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);

            return result;
        }

        public async Task<IdentityResult> Delete(string id)
        {
            var user = await Get(id);
            var result = await _userManager.DeleteAsync(user);

            return result;
        }

        public async Task<IdentityResult> AddToRole(string userId, string roleName)
        {
            var result = await _userManager.AddToRolesAsync(userId, roleName);

            return result;
        }

        public async Task<IdentityResult> AddToRoles(string userId, IEnumerable<string> roleNames)
        {
            var result = await _userManager.AddToRolesAsync(userId, roleNames.ToArray());

            return result;
        }

        private IEnumerable<IdentityRole> GetUserRoles(string userId)
        {
            var kernel = new StandardKernel(new RepositoryModule());
            var rolesRepository = kernel.Get<IIdentityRepository<IdentityRole>>();
            var roles = rolesRepository.Get().Where(x => x.Users.Select(y => y.UserId).Contains(userId));

            return roles;
        }
    }
}
