using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace IM.Identity.EDM.Entities
{
    public partial class AspNetRole
    {
        public AspNetRole()
        {
            AspNetUsers = new HashSet<AspNetUser>();
        }

        public string Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}
