using System;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Agatha.Models;
using FlickrNet;
using MbCache.Configuration;
using MbCache.Core.Events;
using Newtonsoft.Json;
using Serilog;


namespace Agatha.Services {
    public class PaintingsService : IPaintingsService {
        private readonly Flickr _flickr;

        public PaintingsService(Flickr flickr) {
            _flickr = flickr;
        }

        public Painting[] GetGallery(string name) {
            Log.Logger.Information($"GetGallery: name={name}");
            var sets = _flickr.PhotosetsGetList();

            var gallery = sets.FirstOrDefault(x => x.Title.ToLower() == name.ToLower());
            if ( gallery != null ) {
                var photos =
                    _flickr.PhotosetsGetPhotos(gallery.PhotosetId, PhotoSearchExtras.Description);
                return photos.Select(x => new Painting(x)).ToArray();
            }
            else
                throw new Exception($"Could not find set with name \"{name}\".");
        }

        public Painting GetPainting(string id) {
            Log.Logger.Information($"GetPainting: id={id}");
            return this.PhotoInfoToPainting(_flickr.PhotosGetInfo(id));
        }

        private Painting PhotoInfoToPainting(PhotoInfo photoInfo) {
            
            var descriptionParts = photoInfo.Description.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            
            return new Painting {
                Id = photoInfo.PhotoId,
                Name = photoInfo.Title,
                Description = descriptionParts.Length > 0 ? descriptionParts[0]: "",
                Comment =  descriptionParts.Length > 1 ? string.Join(" ", descriptionParts.Skip(1).ToArray()) : "",
                LargeUrl = photoInfo.LargeUrl,
                SmallUrl = photoInfo.SmallUrl,
            };
        }

    }
}

public class FileCache : ICache {
    private readonly string _directory;

    public FileCache(string directory) {
        if ( !Directory.Exists(directory) ) {
            throw new Exception($"Directory \"{directory}\" does not exist.");
        }
        this._directory = directory;
    }

    public void Clear() {
        var di = new DirectoryInfo(this._directory);
        foreach ( var file in di.GetFiles() ) {
            file.Delete();
        }
        foreach ( var dir in di.GetDirectories() ) {
            dir.Delete(true);
        }
    }

    public void Delete(string cacheKey) {
        throw new NotImplementedException();
    }

    public object GetAndPutIfNonExisting(KeyAndItsDependingKeys keyAndItsDependingKeys,
        CachedMethodInformation cachedMethodInformation, Func<object> originalMethod) {
        string cacheKey = keyAndItsDependingKeys.Key.Replace("|", "/");
        var cacheFile = Path.Combine(this._directory, $"{cacheKey}.json");
        if ( File.Exists(cacheFile) ) {
            return
                JsonConvert.DeserializeObject(
                    File.ReadAllText(cacheFile),
                    cachedMethodInformation.Method.ReturnType);
        }
        else {
            var valueToCache = originalMethod();
            if ( !Directory.Exists(Path.GetDirectoryName(cacheFile)) ) {
                Directory.CreateDirectory(Path.GetDirectoryName(cacheFile));
            }
            File.WriteAllText(cacheFile, JsonConvert.SerializeObject(
                valueToCache));

            return valueToCache;
        }
    }


    public void Initialize(EventListenersCallback eventListenersCallback) {
    }
}