using System.Collections.Generic;
using System.Threading.Tasks;
using MordorFanficWeb.ViewModels.CompositionTagsViewModel;

namespace MordorFanficWeb.PresentationAdapters.CompositionTagsAdapter
{
    public interface ICompositionTagsAdapter
    {
        Task CreateRangeOfTags(List<CompositionTagsViewModel> tags);
    }
}
