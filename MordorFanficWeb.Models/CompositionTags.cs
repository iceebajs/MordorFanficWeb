using MordorFanficWeb.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace MordorFanficWeb.Models
{
    public class CompositionTags : BaseEntity
    {
        [Key]
        public int CompositionTagsId { get; set; }
        public int CompositionId { get; set; }
        public virtual Composition Composition { get; set; }

        public int TagId { get; set; }
        public virtual Tags Tag { get; set; }
    }
}
