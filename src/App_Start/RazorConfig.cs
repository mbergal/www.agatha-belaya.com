using System.Linq;
using System.Web.Mvc;

namespace Agatha {
    public class RazorConfig {
        public static void Register() {
            System.Web.Mvc.RazorViewEngine rve = ( RazorViewEngine )ViewEngines.Engines
                .FirstOrDefault(e => e.GetType() == typeof(RazorViewEngine));

            string[] additionalPartialViewLocations = {
                "~/src/Views/{1}/{0}.cshtml",
                "~/src/Views/{1}/{0}.vbhtml",
                "~/src/Views/Shared/{0}.cshtml",
                "~/src/Views/Shared/{0}.vbhtml"
            };


            if ( rve != null )
            {
                rve.ViewLocationFormats = rve.ViewLocationFormats
                    .Union(additionalPartialViewLocations)
                    .ToArray();


            }
        }
    }
}