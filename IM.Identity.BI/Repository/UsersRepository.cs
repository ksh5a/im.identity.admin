using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using IM.Identity.BI.Edm;
using IM.Identity.BI.Models;
using IM.Identity.BI.Repository.Interface;
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

            return users;
        }

        public async Task<ApplicationUser> Get(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return user;
        }

        public async Task<IdentityResult> Insert(ApplicationUser user)
        {
            throw new NotImplementedException();
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
    }
}
