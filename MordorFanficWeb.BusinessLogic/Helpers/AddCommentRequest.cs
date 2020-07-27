using Newtonsoft.Json;

namespace MordorFanficWeb.BusinessLogic.Helpers
{
    public class AddCommentRequest
    {
        public int CommentId { get; set; }
        public string UserName { get; set; }
        public string CommentContext { get; set; }
        public int CompositionId { get; set; }

        public static AddCommentRequest FromJson(string json)
        {
            return JsonConvert.DeserializeObject<AddCommentRequest>(json);
        }
    }
}
