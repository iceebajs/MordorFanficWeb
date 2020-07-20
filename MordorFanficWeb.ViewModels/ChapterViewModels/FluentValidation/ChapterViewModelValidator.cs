using FluentValidation;
namespace MordorFanficWeb.ViewModels.ChapterViewModels.FluentValidation
{
    public class ChapterViewModelValidator : AbstractValidator<ChapterViewModel>
    {
        public ChapterViewModelValidator()
        {
            RuleFor(vm => vm.ChapterTitle).NotEmpty().WithMessage("Chapter title cannot be empty");
            RuleFor(vm => vm.Context).NotEmpty().WithMessage("Chapter context cannot be empty");
        }
    }
}
