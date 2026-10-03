using System;
using System.Web;
using Sentry;
using Sentry.AspNet;

namespace SentryPlayground
{
    public class Global : HttpApplication
    {
        private static IDisposable _sentry;

        protected void Application_Start()
        {
            // An empty/missing DSN makes the SDK a no-op, so the app runs without Sentry configured.
            _sentry = SentrySdk.Init(o =>
            {
                o.AddAspNet();
                o.Dsn = AppConfig.Get("Sentry.Backend.Dsn") ?? ""; // "" disables the SDK; null would throw
                o.Environment = AppConfig.Get("Sentry.Environment");
                o.SendDefaultPii = false;
                o.DefaultTags["app"] = "backend";
            });
        }

        // Single capture point for unhandled request exceptions (no separate logging integration).
        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            if (ex is HttpUnhandledException && ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            if (ex != null)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        protected void Application_End()
        {
            _sentry?.Dispose();
        }
    }
}
