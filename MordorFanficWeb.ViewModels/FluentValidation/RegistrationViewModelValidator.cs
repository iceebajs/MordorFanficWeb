using FluentValidation;
using System.Data;

namespace MordorFanficWeb.ViewModels.FluentValidation
{
    public class RegistrationViewModelValidator : AbstractValidator<RegistrationViewModel>
    {
        public RegistrationViewModelValidator()
        {
            RuleFor(vm => vm.UserName).NotEmpty().WithMessage("User name cannot be empty");
            RuleFor(vm => vm.Email).NotEmpty().WithMessage("Email cannot be empty");
            RuleFor(vm => vm.EmailConfirm).Equal(vm => vm.Email).WithMessage("The email addresses do not match");
            RuleFor(vm => vm.Password).NotEmpty().WithMessage("Password cannot be empty");
            RuleFor(vm => vm.PasswordConfirm).Equal(vm => vm.Password).WithMessage("The passwords do not match");
            RuleFor(vm => vm.FirstName).NotEmpty().WithMessage("First name cannot be empty");
            RuleFor(vm => vm.LastName).NotEmpty().WithMessage("Last name cannot be empty");
        }
    }
}
