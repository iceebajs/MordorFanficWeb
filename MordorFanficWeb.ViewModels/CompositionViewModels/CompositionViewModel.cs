using System.Collections.Generic;
using MordorFanficWeb.ViewModels.ChapterViewModels;
using MordorFanficWeb.ViewModels.CompositionTagsViewModels;

namespace MordorFanficWeb.ViewModels.CompositionViewModels
{
    public class CompositionViewModel
    {
        public int CompositionId { get; set; }
        public string Title { get; set; }
        public string PreviewContext { get; set; }
        public string Genre { get; set; }

        public int UserId { get; set; }
        public ICollection<ChapterViewModel> Chapters { get; set; }
        public ICollection<CompositionTagsViewModel> CompositionTags { get; set; }
    }
}
