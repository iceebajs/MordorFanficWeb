using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MordorFanficWeb.Models
{
    public class AppUserModel : IdentityUser
    {
        public AppUserModel()
        {
            Compositions = new HashSet<CompositionModel>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreationDate { get; set; }
        public string LastVisit { get; set; }
        public bool AccountStatus { get; set; }
        public bool IsMasterAdmin { get; set; }
        public virtual ICollection<CompositionModel> Compositions { get; set; }
    }
}
