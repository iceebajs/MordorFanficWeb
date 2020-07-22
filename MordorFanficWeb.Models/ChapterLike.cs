using MordorFanficWeb.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace MordorFanficWeb.Models
{
    public class ChapterLike : BaseEntity
    {
        [Key]
        public int ChapterLikeId { get; set; }
        public int AccountId { get; set; }

        public int ChapterId { get; set; }
        public virtual Chapter Chapter { get; set; }
    }
}
