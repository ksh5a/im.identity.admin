using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using IM.Identity.BI.Edm;
using IM.Identity.BI.Repository.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;

namespace IM.Identity.BI.Repository
{
    public class RolesRepository : BaseRepository, IIdentityRepository<IdentityRole>
    {
        [Inject]
        public RolesRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public IQueryable<IdentityRole> Get()
        {
            var roleManager = GetRoleManager(Context);
            var roles = roleManager.Roles;
                
            return roles;
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
                    result = await roleManager.CreateAsync(role);
                }

                return result;
            }
        }

        public async Task<IdentityResult> Update(IdentityRole role)
        {
            using (var context = new ApplicationDbContext())
            {
                var roleManager = GetRoleManager(context);
                var result = await roleManager.UpdateAsync(role);

                return result;
            }
        }

        public async Task<IdentityResult> Delete(IdentityRole role)
        {
            using (var context = new ApplicationDbContext())
            {
                var roleManager = GetRoleManager(context);
                var result = await roleManager.DeleteAsync(role);

                return result;
            }
        }

        private RoleManager<IdentityRole> GetRoleManager(DbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            return roleManager;
        }
    }
}
