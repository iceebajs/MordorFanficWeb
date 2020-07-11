using FluentValidation;
using System.Data;

namespace MordorFanficWeb.ViewModels.FluentValidation
{
    public class UpdateUserViewModelValidator : AbstractValidator<UpdateUserViewModel>
    {
        public UpdateUserViewModelValidator()
        {
            RuleFor(vm => vm.UserName).NotEmpty().WithMessage("User name cannot be empty");
            RuleFor(vm => vm.FirstName).NotEmpty().WithMessage("First name cannot be empty");
            RuleFor(vm => vm.LastName).NotEmpty().WithMessage("Last name cannot be empty");
        }
    }
}
