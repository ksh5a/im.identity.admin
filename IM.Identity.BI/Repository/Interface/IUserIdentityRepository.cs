using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IM.Identity.BI.Repository.Interface
{
    public interface IUserIdentityRepository<T> : IIdentityRepository<T>
    {
        Task<IdentityResult> Insert(T user, string password);
        Task<IdentityResult> AddToRole(T user, string roleName);
        Task<IdentityResult> AddToRoles(T user, params string[] roleNames);
        Task<IdentityResult> RemoveFromRole(T user, string roleName);
        Task<IdentityResult> RemoveFromRoles(T user, params string[] roleNames);
        IEnumerable<IdentityRole> GetUserRoles(string userId);
    }
}
