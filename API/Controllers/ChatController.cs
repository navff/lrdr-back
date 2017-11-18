using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using API.Common;
using API.Operations;
using Microsoft.Web.WebSockets;
using Models.Operations;

namespace API.Controllers
{
    public class ChatController : ApiController
    {
        private UserOperations _userOperations;
        private OrderOperations _orderOperations;

        public ChatController(UserOperations userOperations, OrderOperations orderOperations)
        {
            _userOperations = userOperations;
            _orderOperations = orderOperations;
        }

        //[RESTAuthorize]
        public async Task<HttpResponseMessage> Get(int orderId, string token)
        {
            var user = await _userOperations.GetUserByTokenAsync(token);
            if (user == null)
            {
                return new HttpResponseMessage(HttpStatusCode.Forbidden);
            }

            var order = await _orderOperations.GetAsync(orderId);
            if (order == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            if (user.Id != order.OwnerUserId && user.Id != order.CustomerUserId)
            {
                return new HttpResponseMessage(HttpStatusCode.Forbidden);
            }


            HttpContext.Current.AcceptWebSocketRequest(new ChatWebSocketHandler(orderId, User.Identity.Name));
            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }

        class ChatWebSocketHandler : WebSocketHandler
        {
            private static Dictionary<int, WebSocketCollection> _chats = new Dictionary<int, WebSocketCollection>();
            private WebSocketCollection _currentChat;
            private string _userEmail;

            public ChatWebSocketHandler(int orderId, string userEmail)
            {
                _userEmail = userEmail;
                _currentChat = new WebSocketCollection();
                if (!_chats.ContainsKey(orderId))
                {
                    _chats.Add(orderId, _currentChat);
                }
                
            }

            public override void OnOpen()
            {
                base.OnOpen();
                _currentChat.Add(this);
            }

            public override void OnMessage(string message)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                SomeMessage msg = new SomeMessage();
                try
                {
                    msg = serializer.Deserialize<SomeMessage>(message);
                }
                catch (Exception e)
                {
                    // Интересно, что делать, если клиент прислал говно?
                    // пока что просто глушим
                    Console.WriteLine(e);
                    return;
                }

                _currentChat.Broadcast(_userEmail + " ^ " + msg.Text);
            }

        }

        class SomeMessage
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }
    }
}
