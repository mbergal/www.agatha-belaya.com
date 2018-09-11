using System.IO;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using ImageResizer;
using Agatha.Models;
using Agatha.Services;

namespace Agatha.Controllers {
    public class PaintingController : Controller {
        private readonly IPaintingsService _paintingsService;

        public PaintingController(IPaintingsService paintingsService) {
            _paintingsService = paintingsService;
        }

        public FileResult Show(string id) {
            var imageUrl = _paintingsService.GetPainting(id).LargeUrl;
            var cached =
                HostingEnvironment.MapPath($"~/.cache/Urls/{HttpUtility.UrlEncode(imageUrl)}");
            if ( !System.IO.File.Exists(cached) ) {
                var request = ( HttpWebRequest ) WebRequest.Create(imageUrl);
                request.Method = "GET";
                var response = ( HttpWebResponse ) request.GetResponse();

                if ( response.StatusCode.ToString().ToLower() == "ok" ) {
                    string contentType = response.ContentType;
                    var content = response.GetResponseStream();
                    Directory.CreateDirectory(Path.GetDirectoryName(cached));
                    using ( var fileStream = System.IO.File.Create(cached) ) {
                        content.CopyTo(fileStream);
                    }
                }
            }

            if ( System.IO.File.Exists(cached) ) {
                var resizedFlickrResult = new MemoryStream();
                ImageBuilder.Current.Build(System.IO.File.ReadAllBytes(cached), resizedFlickrResult,
                    new ResizeSettings(Request.QueryString));
                resizedFlickrResult.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(resizedFlickrResult, "image/jpeg");
            }
            else
                return null;
        }

        public ActionResult Details(string id) {
            return View(new PaintingDetailsModel(_paintingsService.GetPainting(id)));
        }
    }
}