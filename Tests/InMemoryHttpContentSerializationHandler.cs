using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests
{
    public class InMemoryHttpContentSerializationHandler : DelegatingHandler
    {
        public InMemoryHttpContentSerializationHandler()
        {
        }

        public InMemoryHttpContentSerializationHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Replace the original content with a StreamContent before the request
            // passes through upper layers in the stack
            request.Content = ConvertToStreamContent(request.Content);

            return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>((responseTask) =>
            {
                HttpResponseMessage response = responseTask.Result;

                // Replace the original content with a StreamContent before the response
                // passes through lower layers in the stack
                response.Content = ConvertToStreamContent(response.Content);

                return response;
            });
        }

        private StreamContent ConvertToStreamContent(HttpContent originalContent)
        {
            if (originalContent == null)
            {
                return null;
            }

            StreamContent streamContent = originalContent as StreamContent;

            if (streamContent != null)
            {
                return streamContent;
            }

            MemoryStream ms = new MemoryStream();

            // **** NOTE: ideally you should NOT be doing calling Wait() as its going to block this thread ****
            // if the original content is an ObjectContent, then this particular CopyToAsync() call would cause the MediaTypeFormatters to 
            // take part in Serialization of the ObjectContent and the result of this serialization is stored in the provided target memory stream.
            originalContent.CopyToAsync(ms).Wait();

            // Reset the stream position back to 0 as in the previous CopyToAsync() call,
            // a formatter for example, could have made the position to be at the end after serialization
            ms.Position = 0;

            streamContent = new StreamContent(ms);

            // copy headers from the original content
            foreach (KeyValuePair<string, IEnumerable<string>> header in originalContent.Headers)
            {
                streamContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return streamContent;
        }
    }
}
