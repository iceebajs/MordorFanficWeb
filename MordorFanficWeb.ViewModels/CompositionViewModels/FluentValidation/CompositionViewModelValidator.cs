using FluentValidation;

namespace MordorFanficWeb.ViewModels.CompositionViewModels.FluentValidation
{
    public class CompositionViewModelValidator : AbstractValidator<CompositionViewModel>
    {
        public CompositionViewModelValidator()
        {
            RuleFor(vm => vm.Genre).NotEmpty().WithMessage("Genre cannot be empty");
            RuleFor(vm => vm.PreviewContext).NotEmpty().WithMessage("Preview context cannot be empty");
            RuleFor(vm => vm.Title).NotEmpty().WithMessage("Title cannot be empty");
            RuleFor(vm => vm.CompositionTags).NotEmpty().WithMessage("Tags field cannot be empty");
        }
    }
}
