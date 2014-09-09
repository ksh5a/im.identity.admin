using IM.Identity.BI.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;

namespace IM.Identity.BI.Edm
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        [Inject]
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
