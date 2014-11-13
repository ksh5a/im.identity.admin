using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IM.Identity.Web.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Lockout")]
        public bool LockoutEnabled { get; set; }

        [Display(Name = "Email Confirmed")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Phone Number Confirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        [Display(Name = "Lockout End Date")]
        public DateTime? LockoutEndDateUtc { get; set; }

        [Display(Name = "Access Failed Count")]
        public int AccessFailedCount { get; set; }

        public IList<IdentityRoleViewModel> RoleViewModels { get; set; }
    }
}