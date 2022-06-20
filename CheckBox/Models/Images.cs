namespace CheckBox.Models
{
    public class Images
    {
        public int Id { get; set; }

        public int AlbumId { get; set; }

        public string ImageName { get; set; }

        public string ImagePath { get; set; }

        public bool IsThumbnail { get; set; }
    }
}
