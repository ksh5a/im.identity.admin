using IM.Identity.EDM.Entities;

namespace IM.Identity.EDM.Repository
{
    public class AspNetRoleRepository : GenericRepository<AspNetRole>
    {
        public AspNetRoleRepository(IdentityDbContext context) : base(context)
        {
        }
    }
}
