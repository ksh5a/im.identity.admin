using System.Linq;
using System.Threading.Tasks;
using IM.Identity.BI.Edm;
using IM.Identity.BI.Enums;
using IM.Identity.BI.Repository.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;

namespace IM.Identity.BI.Repository
{
    public class RolesRepository : BaseRepository, IRoleIdentityRepository<IdentityRole>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        [Inject]
        public RolesRepository(ApplicationDbContext context)
            : base(context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            _roleManager = new RoleManager<IdentityRole>(roleStore);
        }

        #region IIdentityRepository

        public IQueryable<IdentityRole> Get()
        {
            var roles = _roleManager.Roles;
                
            return roles;
        }

        public async Task<IdentityRole> Get(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            return role;
        }

        public async Task<IdentityResult> Insert(IdentityRole role)
        {
            var result = new IdentityResult();

            if (!_roleManager.RoleExists(role.Name))
            {
                result = await _roleManager.CreateAsync(role);
            }

            return result;
        }

        public async Task<IdentityResult> Insert(string roleName)
        {
            var result = await Insert(new IdentityRole(roleName));

            return result;
        }

        public async Task<IdentityResult> Update(IdentityRole role)
        {
            var result = await _roleManager.UpdateAsync(role);

            return result;
        }

        public async Task<IdentityResult> Delete(string id)
        {
            var role = await Get(id);
            var result = await _roleManager.DeleteAsync(role);

            return result;
        }

        #endregion

        #region IRoleIdentityRepository

        public async Task<bool> RoleExists(string roleName)
        {
            var result = await _roleManager.RoleExistsAsync(roleName);

            return result;
        }

        public async Task<IdentityResult> CreateAdministrationRoles()
        {
            var result = await Insert(RoleConstants.SuperAdminRole);

            if (result.Succeeded)
            {
                result = await Insert(RoleConstants.AdminRole);
            }

            return result;
        }

        #endregion
    }
}
