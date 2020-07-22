using MordorFanficWeb.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
namespace MordorFanficWeb.Models
{
    public class CompositionComments : BaseEntity
    {
        [Key]
        public int CommentId { get; set; }
        public string CommentContext { get; set; }

        public int CompositionId { get; set; }
        public virtual Composition Composition { get; set; }
    }
}
