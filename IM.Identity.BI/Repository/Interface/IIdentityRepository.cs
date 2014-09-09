using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace IM.Identity.BI.Repository.Interface
{
    public interface IIdentityRepository<T> : IDisposable
    {
        IQueryable<T> Get();

        Task<T> Get(string id);

        Task<IdentityResult> Insert(T entity);

        Task<IdentityResult> Update(T entity);

        Task<IdentityResult> Delete(T entity);
    }
}
