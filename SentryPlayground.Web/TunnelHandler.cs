using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace SentryPlayground
{
    /// <summary>
    /// Sentry tunnel: the browser SDKs POST their envelopes here (the `tunnel` option) and the
    /// server relays them to Sentry. Only envelopes addressed to the configured frontend DSN are forwarded.
    /// </summary>
    public class TunnelHandler : HttpTaskAsyncHandler
    {
        private const int MaxEnvelopeBytes = 1024 * 1024;
        private static readonly HttpClient Client = new HttpClient();

        public override async Task ProcessRequestAsync(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;

            if (!string.Equals(request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
            {
                response.StatusCode = 405;
                return;
            }

            if (!Uri.TryCreate(AppConfig.Get("Sentry.Frontend.Dsn"), UriKind.Absolute, out var dsn))
            {
                // No DSN configured: Sentry is disabled, so drop the event.
                response.StatusCode = 204;
                return;
            }

            if (request.ContentLength > MaxEnvelopeBytes)
            {
                response.StatusCode = 413;
                return;
            }

            byte[] body;
            using (var buffer = new MemoryStream())
            {
                await request.InputStream.CopyToAsync(buffer);
                body = buffer.ToArray();
            }
            if (body.Length > MaxEnvelopeBytes)
            {
                response.StatusCode = 413;
                return;
            }

            // The envelope's first line is a JSON header that names the target DSN.
            var newline = Array.IndexOf(body, (byte)'\n');
            var headerJson = Encoding.UTF8.GetString(body, 0, newline < 0 ? body.Length : newline);
            string envelopeDsn;
            try
            {
                var header = new JavaScriptSerializer().Deserialize<System.Collections.Generic.Dictionary<string, object>>(headerJson);
                envelopeDsn = header != null && header.TryGetValue("dsn", out var value) ? value as string : null;
            }
            catch (ArgumentException)
            {
                envelopeDsn = null;
            }

            if (!Uri.TryCreate(envelopeDsn, UriKind.Absolute, out var target)
                || target.Host != dsn.Host
                || target.AbsolutePath != dsn.AbsolutePath)
            {
                response.StatusCode = 400;
                return;
            }

            var projectId = dsn.AbsolutePath.Trim('/');
            var upstream = dsn.Scheme + "://" + dsn.Authority + "/api/" + projectId + "/envelope/";
            using (var content = new ByteArrayContent(body))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-sentry-envelope");
                using (var result = await Client.PostAsync(upstream, content))
                {
                    response.StatusCode = (int)result.StatusCode;
                }
            }
        }
    }
}
