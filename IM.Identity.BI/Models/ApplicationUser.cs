﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IM.Identity.BI.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IM.Identity.BI.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, 
    // please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        #region Extra column mapping

        public string FirstName { get; set; }
        public string LastName { get; set; }

        #endregion

        #region Properties

        public IEnumerable<IdentityRole> UserRoles { get; set; }

        #endregion

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here

            return userIdentity;
        }

        public bool IsSuperAdmin()
        {
            return UserRoles != null && UserRoles.Select(role => role.Name).Contains(RoleConstants.SuperAdminRole);
        }
    }
}
