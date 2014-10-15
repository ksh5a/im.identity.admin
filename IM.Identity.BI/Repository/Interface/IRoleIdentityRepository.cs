using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace IM.Identity.BI.Repository.Interface
{
    public interface IRoleIdentityRepository<T> : IIdentityRepository<T>
    {
        Task<bool> RoleExists(string name);
        Task<IdentityResult> Insert(string roleName);
        Task<IdentityResult> CreateAdministrationRoles();
    }
}
