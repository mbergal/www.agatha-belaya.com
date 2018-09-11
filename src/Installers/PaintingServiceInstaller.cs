using System.Web.Hosting;
using Agatha.Services;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FlickrNet;
using MbCache.Configuration;
using MbCache.ProxyImpl.LinFu;


namespace Agatha.Installers {   
    public class PaintingServiceInstaller : IWindsorInstaller {
        public void Install(IWindsorContainer container, IConfigurationStore store) {
            var persistentCache = new FileCache(HostingEnvironment.MapPath("~/.cache"));
            var cacheBuilder =
                new CacheBuilder(new LinFuProxyFactory()).SetCache(persistentCache);
            cacheBuilder.For<PaintingsService>()
                .CacheMethod(c => c.GetGallery(""))
                .CacheMethod(c => c.GetPainting(""))
                .As<IPaintingsService>();
            var factory = cacheBuilder.BuildFactory();

            container.Register(Component.For<IPaintingsService>().UsingFactoryMethod(
                (kernel) => factory.Create<IPaintingsService>(kernel.Resolve<Flickr>())
            ));
        }
    }
}