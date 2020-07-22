using System.Threading.Tasks;
using MordorFanficWeb.ViewModels.CompositionCommentsViewModels;

namespace MordorFanficWeb.PresentationAdapters.CommentsAdapter
{
    public interface ICommentsAdapter
    {
        Task AddComent(CompositionCommentsViewModel comment);
    }
}
