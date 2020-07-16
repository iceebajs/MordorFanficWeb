using System;
using System.Collections.Generic;
using System.Text;

namespace MordorFanficWeb.ViewModels
{
    public class ChangeUserPasswordViewModel
    {
        public string Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirm { get; set; }
    }
}
