

namespace MordorFanficWeb.ViewModels
{
    public class RegistrationViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string EmailConfirm { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreationDate { get; set; }
        public string LastVisit { get; set; }
        public bool AccountStatus { get; set; }
        public bool IsMasterAdmin { get; set; }

    }
}
