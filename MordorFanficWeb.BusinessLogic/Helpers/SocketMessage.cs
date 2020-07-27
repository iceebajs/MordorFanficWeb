using Newtonsoft.Json;

namespace MordorFanficWeb.BusinessLogic.Helpers
{
    public class SocketMessage<T>
    {
        public string MessageType { get; set; }
        public T Payload { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
