using Agatha.Models;

namespace Agatha.Services {
    public interface IPaintingsService {
        Painting[] GetGallery(string name);
        Painting GetPainting(string id);
    }
}