namespace Agatha.Models
{
    public class PaintingDetailsModel
        {
        public PaintingDetailsModel( Painting painting )
            {
            this.Painting = painting;
            }

        public string PageTitle => $"Agatha Belaya - {Painting.Name}";

        public string Name => this.Painting.Name;
        public string Description => this.Painting.Description;
        public string Comment => this.Painting.Comment;
        public Painting Painting { get; set; }
        };
}