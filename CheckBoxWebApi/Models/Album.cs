using System.ComponentModel.DataAnnotations;

namespace CheckBoxWebApi.Models
{
    public class Album
    {
        public long Id { get; set; }

        [MaxLength(256)]
        public string UserId { get; set; }

        [Key]
        [MaxLength(256)]
        public string AlbumId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Title { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime EditTime { get; set; }
    }
}
