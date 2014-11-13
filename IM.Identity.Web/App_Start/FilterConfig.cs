using System.Globalization;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace IM.Identity.Web
{
    public class FilterConfig
    {
        private bool _useHttps;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            #region Https filter

            bool useHttps;
            var useHttpsString = WebConfigurationManager.AppSettings["UseHttps"] != null
                ? WebConfigurationManager.AppSettings["UseHttps"].ToString(CultureInfo.InvariantCulture)
                : string.Empty;
            bool.TryParse(useHttpsString, out useHttps);

            if (useHttps)
            {
                filters.Add(new RequireHttpsAttribute());
            }

            #endregion
        }
    }
}
