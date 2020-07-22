using MordorFanficWeb.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace MordorFanficWeb.Models
{
    public class Tags : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Tag { get; set; }
    }
}
