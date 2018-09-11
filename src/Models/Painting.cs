using System;
using System.Linq;
using FlickrNet;

namespace Agatha.Models {
    public class Painting {
        public Painting() {
        }


        public Painting(Photo photo) {
            this.Id = photo.PhotoId;
            this.Name = photo.Title;
            this.SmallUrl = photo.SmallUrl;
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Comment { get; set; }

        public string LargeUrl { get; set; }


        //public string                       width;
        //public string                       height;
        public string SmallUrl { get; set; }

        public override string ToString() {
            return string.Format("Name {1}, src {2}", Name, LargeUrl);
        }
    }
}