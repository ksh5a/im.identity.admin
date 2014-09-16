using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IM.Identity.BI.Repository.Interface
{
    public interface IUserIdentityRepository<T> : IIdentityRepository<T>
    {
        Task<IdentityResult> Insert(T user, string password);
        Task<IdentityResult> AddToRole(string userId, string roleName);
        Task<IdentityResult> AddToRoles(string userId, IEnumerable<string> roleNames);
    }
}
