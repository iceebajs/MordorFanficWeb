using System.Collections.Generic;
using System.Threading.Tasks;
using MordorFanficWeb.Models;
using MordorFanficWeb.ViewModels.CompositionViewModels;

namespace MordorFanficWeb.PresentationAdapters.CompositionAdapter
{
    public interface ICompositionAdapter
    {
        Task<CompositionViewModel> GetComposition(int id);
        Task DeleteComposition(int id);
        Task UpdateComposition(CompositionViewModel composition);
        Task<int> CreateComposition(CompositionViewModel composition);
        Task<List<CompositionViewModel>> GetAllCompositions();
        Task<List<CompositionViewModel>> GetAllCompositionsOfAccount(int id);
    }
}
