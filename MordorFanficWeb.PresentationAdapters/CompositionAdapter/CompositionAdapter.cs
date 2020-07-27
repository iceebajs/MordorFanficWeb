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

        public async Task<int> CreateComposition(CompositionViewModel composition)
        {
            return await compositionService.CreateComposition(mapper.Map<Composition>(composition)).ConfigureAwait(false);
        }

        public async Task DeleteComposition(int id)
        {
            var composition = await compositionService.GetAsync<Composition>(x => x.CompositionId == id).ConfigureAwait(false);
            await compositionService.DeleteAsync(composition).ConfigureAwait(false);
        }

        public async Task<List<CompositionViewModel>> GetAllCompositions()
        {
            return mapper.Map<List<CompositionViewModel>>(await compositionService.GetAllAsync<Composition>().ConfigureAwait(false));
        }

        public async Task<CompositionViewModel> GetComposition(int id)
        {
            return mapper.Map<CompositionViewModel>(await compositionService.GetCompositionById(id).ConfigureAwait(false));
        }

        public async Task UpdateComposition(CompositionViewModel composition)
        {
            var updatedComposition = mapper.Map<Composition>(composition);
            await compositionService.UpdateAsync(updatedComposition).ConfigureAwait(false);
        }

        public async Task<List<CompositionViewModel>> GetAllCompositionsOfAccount(int id)
        {
            return mapper.Map<List<CompositionViewModel>>(await compositionService.GetAllCompositionsOfAccount(id).ConfigureAwait(false));
        }

        public async Task<List<CompositionViewModel>> GetLastAdded()
        {
            return mapper.Map<List<CompositionViewModel>>(await compositionService.GetLastAdded().ConfigureAwait(false));
        }

        public async Task<List<CompositionViewModel>> FindInCompositions(string keyword)
        {
            return mapper.Map<List<CompositionViewModel>>(await compositionService.FindInCompositions(keyword).ConfigureAwait(false));
        }
    }
}
