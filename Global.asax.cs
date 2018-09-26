using System;
using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Agatha.Services;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FlickrNet;
using Serilog;

namespace Agatha {
    public class WindsorInstaller : IWindsorInstaller {
        public void Install(IWindsorContainer container, IConfigurationStore store) {
            container.Register(
                Component.For<FlickrAccessTokenRepository>().LifestylePerWebRequest(),
                Component.For<PaintingsService>().LifestylePerWebRequest(),
                Component.For<Flickr>().UsingFactoryMethod(
                    c => {
                        Flickr.CacheDisabled = true;

                        var f = new Flickr("235ab182f4630ecc8025029aef841c6e", "5b6b2e2b0945d067");
                        var accessToken = c.Resolve<FlickrAccessTokenRepository>().GetAccessToken();
                        f.OAuthAccessToken = accessToken.Token;
                        f.OAuthAccessTokenSecret = accessToken.TokenSecret;
                        return f;
                    }).LifestylePerWebRequest()
            );
        }
    }

    public class Global : System.Web.HttpApplication {
        protected void Application_Start(object sender, EventArgs e) {
            RazorConfig.Register();
            RouteConfig.Register(RouteTable.Routes);
            FilterConfig.Register(GlobalFilters.Filters);

            var logPath = Path.Combine(HostingEnvironment.MapPath("~/App_Data"), "log.txt");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(logPath)
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("Starting...");
        }

        protected void Session_Start(object sender, EventArgs e) {
        }

        protected void Application_BeginRequest(object sender, EventArgs e) {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e) {
        }

        protected void Application_Error(object sender, EventArgs e) {
            var ex = Server.GetLastError();
            Log.Logger.Error(ex, ex.Message);
        }

        protected void Session_End(object sender, EventArgs e) {
        }

        protected void Application_End(object sender, EventArgs e) {
        }
    }
}