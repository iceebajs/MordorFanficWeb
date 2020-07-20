using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MordorFanficWeb.Models
{
    public class AccountUser
    {
        public AccountUser()
        {
            Compositions = new HashSet<Composition>();
        }

        [Key]
        public int AccountUserId { get; set; }
        public string IdentityId { get; set; }
        public virtual AppUser Identity { get; set; }

        public virtual ICollection<Composition> Compositions { get; set; }

    }
}
