using MordorFanficWeb.ViewModels.ChapterLikesViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MordorFanficWeb.ViewModels.ChapterViewModels
{
    public class ChapterViewModel
    {
        public int ChapterId { get; set; }
        public int ChapterNumber { get; set; }
        public string ChapterTitle { get; set; }
        public string Context { get; set; }
        public string ImgSource { get; set; }

        public int CompositionId { get; set; }
        public ICollection<ChapterLikeViewModel> ChapterLikes { get; set; }
    }
}
