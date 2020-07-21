using System.Collections.Generic;
using System.Threading.Tasks;
using MordorFanficWeb.ViewModels.TagsViewModels;

namespace MordorFanficWeb.PresentationAdapters.TagsAdapter
{
    public interface ITagsAdapter
    {
        Task CreateRangeOfTags(List<TagsViewModel> tags);
        Task<List<TagsViewModel>> GetAllTags();
    }
}
