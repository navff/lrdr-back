using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using Microsoft.Web.WebSockets;

namespace API.Controllers
{
    public class ChatController : ApiController
    {
        public HttpResponseMessage Get(string username)
        {
            HttpContext.Current.AcceptWebSocketRequest(new ChatWebSocketHandler(username));
            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }

        class ChatWebSocketHandler : WebSocketHandler
        {
            private static WebSocketCollection _chatClients = new WebSocketCollection();
            private string _username;

            public ChatWebSocketHandler(string username)
            {
                _username = username;
            }

            public override void OnOpen()
            {
                base.OnOpen();
                _chatClients.Add(this);
            }

            public override void OnMessage(string message)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var msg = serializer.Deserialize<SomeMessage>(message);
                _chatClients.Broadcast(_username + " ^ " + message);
            }
        }

        class SomeMessage
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }
    }
}
