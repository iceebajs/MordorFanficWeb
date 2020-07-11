using Castle.Components.DictionaryAdapter;
using Microsoft.AspNetCore.Identity;

namespace MordorFanficWeb.Models
{
    public class AppUserModel : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreationDate { get; set; }
        public string LastVisit { get; set; }
        public bool AccountStatus { get; set; }
        public bool IsMasterAdmin { get; set; }

    }
}
