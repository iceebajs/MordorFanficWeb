using FluentValidation;
using System.Data;

namespace MordorFanficWeb.ViewModels.FluentValidation
{
    public class CredentialsViewModelValidator : AbstractValidator<CredentialsViewModel>
    {
        public CredentialsViewModelValidator()
        {
            RuleFor(vm => vm.Email).NotEmpty().WithMessage("Email valuse is empty");
            RuleFor(vm => vm.Password).NotEmpty().WithMessage("Password value is empty");
        }
    }
}
