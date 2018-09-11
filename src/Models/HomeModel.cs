using System.Collections.Generic;
using System.Linq;
using Agatha.Services;

namespace Agatha.Models {
    public class PaintingAndGallery {
        public Gallery Gallery { get; set; }
        public Painting Painting { get; set; }
    }

    public class IndexModel {
        public IndexModel(IPaintingsService paintingsService) {
            this.PaintingService = paintingsService;
        }

        public IPaintingsService PaintingService { get; set; }

        public IEnumerable<GalleryMenuItem> Galleries {
            get {
                return Models.Galleries.List
                    .Select(menuItem => new GalleryMenuItem(menuItem.Name, menuItem.Id,
                        PaintingService.GetGallery(menuItem.Name).Length));
            }
        }

        public IEnumerable<PaintingAndGallery> Paintings {
            get {
                return Models.Galleries.List.SelectMany(gallery => PaintingService
                    .GetGallery(gallery.Name)
                    .Select(painting =>
                        new PaintingAndGallery {Gallery = gallery, Painting = painting}));
            }
        }
    }
}