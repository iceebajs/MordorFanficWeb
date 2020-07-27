using MordorFanficWeb.BusinessLogic.Helpers;
using MordorFanficWeb.ViewModels.CompositionCommentsViewModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace MordorFanficWeb.BusinessLogic.Services
{
    public class WebSocketService
    {
        private ConcurrentDictionary<string, WebSocket> users = new ConcurrentDictionary<string, WebSocket>();

        public async Task AddComment(WebSocket socket)
        {
            try
            {
                var name = GenerateName();
                var userAddedSuccessfully = users.TryAdd(name, socket);
                while (!userAddedSuccessfully)
                {
                    name = GenerateName();
                    userAddedSuccessfully = users.TryAdd(name, socket);
                }

                if(users.Values.Count > 50)
                {
                    var closedConnections = users.Where(c => c.Value.State == WebSocketState.Closed);
                    var abortedConnections = users.Where(c => c.Value.State == WebSocketState.Aborted);

                    foreach (var con in closedConnections)
                        users.TryRemove(con.Key, out var i);
                    foreach (var con in abortedConnections)
                        users.TryRemove(con.Key, out var i);
                }

                while (socket.State == WebSocketState.Open)
                {
                    var buffer = new byte[1024 * 4];
                    WebSocketReceiveResult socketResponse;
                    var package = new List<byte>();
                    do
                    {
                        socketResponse = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).ConfigureAwait(false);
                        package.AddRange(new ArraySegment<byte>(buffer, 0, socketResponse.Count));
                    } while (!socketResponse.EndOfMessage);
                    var bufferAsString = System.Text.Encoding.ASCII.GetString(package.ToArray());
                    if (!string.IsNullOrEmpty(bufferAsString))
                    {
                        var changeRequest = AddCommentRequest.FromJson(bufferAsString);
                        await HandleAddComentRequest(changeRequest).ConfigureAwait(false);
                    }
                }
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None).ConfigureAwait(false);
            }
            catch
            { }
        }

        private string GenerateName()
        {
            var prefix = "MordorClient";
            Random ran = new Random();
            var name = prefix + ran.Next(1, 1000);
            while (users.ContainsKey(name))
            {
                name = prefix + ran.Next(1, 1000);
            }
            return name;
        }

        private async Task HandleAddComentRequest(AddCommentRequest request)
        {
            List<CompositionCommentsViewModel> list = new List<CompositionCommentsViewModel>();
            list.Add(new CompositionCommentsViewModel 
            {
                UserName = request.UserName,
                CommentContext = request.CommentContext, 
                CompositionId = request.CompositionId,
                CommentId = 0
            });
            var message = new SocketMessage<List<CompositionCommentsViewModel>>()
            {
                MessageType = "newComment",
                Payload = list
            };
            await SendAll(message.ToJson()).ConfigureAwait(false);
        }

        private async Task SendAll(string message)
        {
            await Send(message, users.Values.ToArray()).ConfigureAwait(false);
        }

        private async Task Send(string message, params WebSocket[] usersToSendComment)
        {
            var sockets = usersToSendComment.Where(s => s.State == WebSocketState.Open);

            foreach (var socket in sockets)
            {
                var stringAsBytes = System.Text.Encoding.ASCII.GetBytes(message);
                var byteArraySegment = new ArraySegment<byte>(stringAsBytes, 0, stringAsBytes.Length);
                await socket.SendAsync(byteArraySegment, WebSocketMessageType.Text, true, CancellationToken.None).ConfigureAwait(false);
            }
        }
    }
}
