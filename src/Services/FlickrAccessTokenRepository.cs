using System.IO;
using System.Web.Hosting;
using FlickrNet;
using Newtonsoft.Json;

namespace Agatha.Services
    {
    public class FlickrAccessTokenRepository
        {
        private OAuthAccessToken _code = null;

        public FlickrAccessTokenRepository()
            {
                
            }

        private string CodeFilePath()
            {
            return HostingEnvironment.MapPath("~/App_Data/flickr.code.txt");
            }

        public void SaveAccessToken( OAuthAccessToken accessToken )
            {
            File.WriteAllText( CodeFilePath(), JsonConvert.SerializeObject(accessToken) );
            _code = accessToken;
            }

        public OAuthAccessToken GetAccessToken()
            {
            if ( _code == null )
                _code = JsonConvert.DeserializeObject<OAuthAccessToken>( File.ReadAllText( CodeFilePath() ) );
            return _code;
            }
        }
    }
