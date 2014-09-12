using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace IM.Identity.BI.Repository.Interface
{
    public interface IUserIdentityRepository<T> : IIdentityRepository<T>
    {
        Task<IdentityResult> Insert(T user, string password);
        Task<IdentityResult> AddToRole(string userId, string roleName);
    }
}
