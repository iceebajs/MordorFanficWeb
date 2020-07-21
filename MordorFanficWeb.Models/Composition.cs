using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MordorFanficWeb.Models.BaseModels;

namespace MordorFanficWeb.Models
{
    public class Composition : BaseEntity
    {
        public Composition()
        {
            Chapters = new HashSet<Chapter>();
            CompositionTags = new HashSet<CompositionTags>();
        }
        [Key]
        public int CompositionId { get; set; }
        public string Title { get; set; }
        public string PreviewContext { get; set; }
        public string Genre { get; set; }
        public string Tags { get; set; }

        public int UserId { get; set; }
        public virtual AccountUser User { get; set; }

        public virtual ICollection<Chapter> Chapters { get; set; }
        public virtual ICollection<CompositionTags> CompositionTags { get; set; }
    }
}
