using AutoMapper;
using MordorFanficWeb.BusinessLogic.Interfaces;
using MordorFanficWeb.ViewModels.CompositionRatingsViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using MordorFanficWeb.Models;

namespace MordorFanficWeb.PresentationAdapters.CompositionRatingsAdapter
{
    public class CompositionRatingsAdapter : ICompositionRatingsAdapter
    {
        private readonly IMapper mapper;
        private readonly ICompositionRatingsService ratingService;

        public CompositionRatingsAdapter(IMapper mapper, ICompositionRatingsService ratingService)
        {
            this.mapper = mapper;
            this.ratingService = ratingService;
        }

        public async Task AddRating(CompositionRatingViewModel rating)
        {
            await ratingService.CreateAsync(mapper.Map<CompositionRatings>(rating)).ConfigureAwait(false);
        }
    }
}
