using System.Threading.Tasks;
using AutoMapper;
using MordorFanficWeb.BusinessLogic.Interfaces;
using MordorFanficWeb.Models;
using MordorFanficWeb.ViewModels.CompositionCommentsViewModels;

namespace MordorFanficWeb.PresentationAdapters.CommentsAdapter
{
    public class CommentsAdapter : ICommentsAdapter
    {
        private readonly IMapper mapper;
        private readonly ICommentsService commentsService;

        public CommentsAdapter(IMapper mapper, ICommentsService commentsService)
        {
            this.mapper = mapper;
            this.commentsService = commentsService;
        }

        public async Task AddComent(CompositionCommentsViewModel comment)
        {
            await commentsService.CreateAsync(mapper.Map<CompositionComments>(comment)).ConfigureAwait(false);
        }
    }
}
