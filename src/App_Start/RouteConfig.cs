using System.Web.Mvc;
using System.Web.Routing;

namespace Agatha
    {
    public class RouteConfig
        {
        public static void Register( RouteCollection routes )
            {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                "PaintingDetails", // Route name
                "painting/details/{id}", // URL with parameters
                new { controller = "Painting", action = "Details" } // Parameter defaults
                );

            routes.MapRoute(
                "Painting", // Route name
                "painting/{id}", // URL with parameters
                new { controller = "Painting", action = "Show" } // Parameter defaults
                );

            routes.MapRoute(
                "ArtistComments", // Route name
                "gallery/comments", // URL with parameters
                new { controller = "Home", action = "ArtistComments" } // Parameter defaults
                );
 
            routes.MapRoute(
                "Gallery", // Route name
                "gallery/{gallery}", // URL with parameters
                new { controller = "Home", action = "OnlineGallery", gallery = UrlParameter.Optional } // Parameter defaults
                );

            routes.MapRoute(
                "About", // Route name
                "about", // URL with parameters
                new { controller = "Home", action = "About" } // Parameter defaults
                );

            routes.MapRoute(
                "Authenticate", // Route name
                "authenticate", // URL with parameters
                new { controller = "Authentication", action = "Index" } // Parameter defaults
                );


            routes.MapRoute(
                "Default", // Route name
                "{action}", // URL with parameters
                new { controller = "Home", action = "Index" } // Parameter defaults
                );
            }
        };
    }