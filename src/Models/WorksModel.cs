using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Agatha.Services;

namespace Agatha.Models {
    public class Gallery {
        public Gallery(string name, string id) {
            Name = name;
            Id = id;
        }

        public string Name { get; set; }
        public string Id { get; set; }
    }

    public class Galleries {
        public static readonly Gallery[] List = {
            new Gallery("Recent Works", "recent_works"),
//            new Gallery("Cats", "cats"),
            new Gallery("Serenity", "serenity"),
            new Gallery("Fog", "fog"),
            new Gallery("Graphics", "graphics"),
//            new Gallery("Silence", "silence"),
            new Gallery("Flowers", "flowers"),
            new Gallery("Chess", "chess"),
            new Gallery("Artist comments", "comments")
        };
    };

    public class GalleryMenuItem {
        public GalleryMenuItem(string name, string id, int count) {
            Name = name;
            Id = id;
            Count = count;
        }

        public string Name;
        public string Id;
        public int Count;
    };

    public class WorksModel {
        private readonly IPaintingsService _paintingService;

        public WorksModel(IPaintingsService paintingsService) {
            _paintingService = paintingsService;
        }

        public IEnumerable<GalleryMenuItem> Galleries {
            get {
                return Models.Galleries.List
                    .Select(
                        menuItem =>
                            new GalleryMenuItem(menuItem.Name, menuItem.Id,
                                this._paintingService.GetGallery(menuItem.Name).Length));
            }
        }

        public IEnumerable<PaintingAndGallery> Paintings {
            get {
                return Models.Galleries.List.SelectMany(gallery => _paintingService
                    .GetGallery(gallery.Name)
                    .Select(painting =>
                        new PaintingAndGallery {Gallery = gallery, Painting = painting}));
            }
        }
    }
}