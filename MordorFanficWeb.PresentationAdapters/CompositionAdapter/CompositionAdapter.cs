using AutoMapper;
using MordorFanficWeb.BusinessLogic.Interfaces;
using MordorFanficWeb.Models;
using MordorFanficWeb.ViewModels.CompositionViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MordorFanficWeb.PresentationAdapters.CompositionAdapter
{
    public class CompositionAdapter : ICompositionAdapter
    {
        private readonly ICompositionService compositionService;
        private readonly IMapper mapper;

        public CompositionAdapter(ICompositionService compositionService, IMapper mapper)
        {
            this.compositionService = compositionService;
            this.mapper = mapper;
        }

        public async Task CreateComposition(CompositionViewModel composition)
        {
            await compositionService.CreateAsync(mapper.Map<CompositionModel>(composition)).ConfigureAwait(false);
        }

        public async Task DeleteComposition(int id)
        {
            var composition = await compositionService.GetAsync<CompositionModel>(x => x.CompositionId == id).ConfigureAwait(false);
            await compositionService.DeleteAsync(composition).ConfigureAwait(false);
        }

        public async Task<List<CompositionViewModel>> GetAllCompositions()
        {
            return mapper.Map<List<CompositionViewModel>>(await compositionService.GetAllAsync<CompositionModel>().ConfigureAwait(false));
        }

        public async Task<CompositionViewModel> GetComposition(int id)
        {
            return mapper.Map<CompositionViewModel>(await compositionService.GetAsync<CompositionModel>(x => x.CompositionId == id).ConfigureAwait(false));
        }

        public async Task UpdateComposition(CompositionViewModel composition)
        {
            var updatedComposition = mapper.Map<CompositionModel>(composition);
            await compositionService.UpdateAsync(updatedComposition).ConfigureAwait(false);
        }
    }
}
