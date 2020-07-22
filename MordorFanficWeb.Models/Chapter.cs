using MordorFanficWeb.Models.BaseModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MordorFanficWeb.Models
{
    public class Chapter : BaseEntity
    {
        public Chapter()
        {
            ChapterLikes = new HashSet<ChapterLike>();
        }

        [Key]
        public int ChapterId { get; set; }
        public int ChapterNumber { get; set; }
        public string ChapterTitle { get; set; }
        public string Context { get; set; }
        public string ImgSource { get; set; }

        public int CompositionId { get; set; }
        public virtual Composition Composition { get; set; }
        public ICollection<ChapterLike> ChapterLikes { get; set; }
    }
}
