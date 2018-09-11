using System;
using System.Web;
using System.IO;
using System.Drawing;
using System.Web.Mvc;
using Agatha.Models;
using Agatha.Services;

namespace Agatha.Helpers {
    public static class ImageHelper {
        public static MvcHtmlString imageHtml<T>(this HtmlHelper<T> htmlHelper, string src) {
            if ( src != "" ) {
                using ( Stream s = new FileStream(HttpContext.Current.Server.MapPath(src),
                    FileMode.Open, FileAccess.Read) ) {
                    var b = new Bitmap(s);
                    int originalWidth = b.Width;
                    int originalHeight = b.Height;

                    return new MvcHtmlString(
                        $"<img src=\"{System.Web.VirtualPathUtility.ToAbsolute(src)}\" border=\"0\" width=\"{originalWidth}\" height=\"{originalHeight}\"/>");
                }
            }
            else {
                throw new Exception();
            }
        }

        public static IPaintingsService PaintingsService<T>(this HtmlHelper<T> htmlHelper) {
            return ( IPaintingsService ) htmlHelper.ViewBag.PaintingService;
        }


        public static string PaintingImageUrl<T>(this HtmlHelper<T> htmlHelper, Painting painting,
            string size) {
            int maxDimension;
            switch ( size ) {
                case "small":
                    maxDimension = 175;
                    break;
                case "medium":
                    maxDimension = 500;
                    break;
                case "large":
                    maxDimension = 640;
                    break;
                default:
                    throw new Exception($"Unknown size \"{size}\"");
            }
            return $"/painting/{painting.Id}?maxwidth={maxDimension}&maxheight={maxDimension}";
        }
    }
}