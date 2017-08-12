using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace API.Common
{
    public static class Helpers
    {
        public static ResponseMessageResult Result404(this ApiController controller, string message)
        {
            return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(message)
            });
        }

        public static ResponseMessageResult Result403(this ApiController controller, string message)
        {
            return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                Content = new StringContent(message)
            });
        }
    }
}