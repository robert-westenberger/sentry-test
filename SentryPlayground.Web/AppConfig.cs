using System.Configuration;
using System.Web.Script.Serialization;

namespace SentryPlayground
{
    public static class AppConfig
    {
        public static string Get(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        /// <summary>Inline script defining window.__SENTRY_CONFIG__ for one frontend app.</summary>
        public static string FrontendConfigScript(string appKey)
        {
            var json = new JavaScriptSerializer().Serialize(new
            {
                dsn = Get("Sentry." + appKey + ".Dsn"),
                environment = Get("Sentry.Environment")
            });
            // Keep the JSON inert inside a <script> element.
            json = json.Replace("<", "\u003c");
            return "<script>window.__SENTRY_CONFIG__ = " + json + ";</script>";
        }
    }
}
