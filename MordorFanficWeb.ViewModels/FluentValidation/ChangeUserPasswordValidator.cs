using FluentValidation;
using System.Data;

namespace MordorFanficWeb.ViewModels.FluentValidation
{
    public class ChangeUserPasswordValidator : AbstractValidator<ChangeUserPasswordViewModel>
    {
        public ChangeUserPasswordValidator()
        {
            RuleFor(vm => vm.OldPassword).NotEmpty().WithMessage("Old password filed is empty");
            RuleFor(vm => vm.NewPassword).NotEmpty().WithMessage("Password cannot be empty");
            RuleFor(vm => vm.NewPasswordConfirm).Equal(vm => vm.NewPassword).WithMessage("The passwords do not match");
        }
    }
}
