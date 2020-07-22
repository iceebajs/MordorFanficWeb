using System.ComponentModel.DataAnnotations;
using MordorFanficWeb.Models.BaseModels;

namespace MordorFanficWeb.Models
{
    public class CompositionRatings : BaseEntity
    {
        [Key]
        public int CompositionRatingId { get; set; }
        public int AccountId { get; set; }
        public int Rating { get; set; }

        public int CompositionId { get; set; }
        public virtual Composition Composition { get; set; }
    }
}
