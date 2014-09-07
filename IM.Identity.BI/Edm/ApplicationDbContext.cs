using IM.Identity.BI.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IM.Identity.BI.Edm
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
