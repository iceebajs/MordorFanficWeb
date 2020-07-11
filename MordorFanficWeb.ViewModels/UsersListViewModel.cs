using System;
using System.Collections.Generic;
using System.Text;

namespace MordorFanficWeb.ViewModels
{
    public class UsersListViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string CreationDate { get; set; }
        public string LastVisit { get; set; }
        public bool AccountStatus { get; set; }
    }
}
