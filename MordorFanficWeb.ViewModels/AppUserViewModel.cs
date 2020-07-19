using MordorFanficWeb.ViewModels.CompositionViewModels;
using System.Collections.Generic;

namespace MordorFanficWeb.ViewModels
{
    public class AppUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool AccountStatus { get; set; }
        public string CreationDate { get; set; }
        public string LastVisit { get; set; }

        public ICollection<CompositionViewModel> Compositions { get; set; }
    }
}
