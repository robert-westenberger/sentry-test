using System;
using System.Web;

namespace SentryPlayground
{
    /// <summary>Deliberately throws so the backend Sentry integration can be verified.</summary>
    public class TestErrorHandler : IHttpHandler
    {
        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context)
        {
            throw new InvalidOperationException("Synthetic backend test error");
        }
    }
}
