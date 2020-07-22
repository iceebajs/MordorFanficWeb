using MordorFanficWeb.ViewModels.CompositionRatingsViewModels;
using System.Threading.Tasks;

namespace MordorFanficWeb.PresentationAdapters.CompositionRatingsAdapter
{
    public interface ICompositionRatingsAdapter
    {
        Task AddRating(CompositionRatingViewModel rating);
    }
}
