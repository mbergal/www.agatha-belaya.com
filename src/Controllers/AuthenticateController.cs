using System.Web.Mvc;
using Agatha.Services;
using FlickrNet;

namespace Agatha.Controllers
    {
    [RoutePrefix("authentication")]
    public class AuthenticationController : Controller
        {
        private readonly Flickr _flickr;
        private readonly FlickrAccessTokenRepository _flickrAccessTokenRepository;

        public AuthenticationController( Flickr flickr, FlickrAccessTokenRepository flickrAccessTokenRepository )
            {
            this._flickr = flickr;
            this._flickrAccessTokenRepository = flickrAccessTokenRepository;
            }

        public ActionResult Index()
            {
            return View();
            }

        [Route("authenticate")]
        [HttpPost]
        public ActionResult Authenticate()
            {
            var requestToken = _flickr.OAuthGetRequestToken("oob");

            string url = _flickr.OAuthCalculateAuthorizationUrl(requestToken.Token, AuthLevel.Write);

            Session["requestToken"] = requestToken;

            return Redirect( url );
            }

        [Route("save-code")]
        [HttpPost]
        public ActionResult SaveCode( string code )
            {
            var accessToken = _flickr.OAuthGetAccessToken( (OAuthRequestToken)Session["requestToken"], code );
            _flickrAccessTokenRepository.SaveAccessToken( accessToken );

            ViewBag.Message = "Successfully authenticated as " + accessToken.FullName;
            return View( "Index" );
            }
        }
    }
