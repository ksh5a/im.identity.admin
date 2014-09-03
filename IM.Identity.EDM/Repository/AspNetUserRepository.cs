using IM.Identity.EDM.Entities;

namespace IM.Identity.EDM.Repository
{
    public class AspNetUserRepository : GenericRepository<AspNetUser>
    {
        public AspNetUserRepository(IdentityDbContext context) : base(context)
        {
        }
    }
}
