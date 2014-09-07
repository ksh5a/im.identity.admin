using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using IM.Identity.BI.Edm;
using IM.Identity.BI.Service.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IM.Identity.BI.Service
{
    public class RolesService : IEntityService
    {
        public async Task<List<IdentityRole>> Get()
        {
            using (var context = new ApplicationDbContext())
            {
                var roleManager = GetRoleManager(context);
                var roles = roleManager.Roles;

                return await roles.ToListAsync();
            }
        }

        public async Task<IdentityRole> Get(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                var roleManager = GetRoleManager(context);
                var role = await roleManager.FindByIdAsync(id);

                return role;
            }
        }

        public async Task<IdentityResult> Insert(IdentityRole role)
        {
            using (var context = new ApplicationDbContext())
            {
                var result = new IdentityResult();
                var roleManager = GetRoleManager(context);

                if (!roleManager.RoleExists(role.Name))
                {
                    result = await roleManager.CreateAsync(new IdentityRole(role.Name));
                }

                return result;
            }
        }

        public async Task<IdentityResult> Update(IdentityRole role)
        {
            using (var context = new ApplicationDbContext())
            {
                var roleManager = GetRoleManager(context);
                var result = await roleManager.UpdateAsync(new IdentityRole(role.Name));

                return result;
            }
        }

        public async Task<IdentityResult> Delete(IdentityRole role)
        {
            using (var context = new ApplicationDbContext())
            {
                var roleManager = GetRoleManager(context);
                var result = await roleManager.DeleteAsync(new IdentityRole(role.Name));

                return result;
            }
        }

        protected RoleManager<IdentityRole> GetRoleManager(DbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            return roleManager;
        }
    }
}
