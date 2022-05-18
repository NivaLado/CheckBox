using System.ComponentModel.DataAnnotations;

namespace CheckBoxWebApi.Models
{
    public class Album
    { 
        [Key]
        public long Id { get; set; }

        public long UserId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Title { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        [Required]
        public string ThumbnailUrl { get; set; }

        [Required]
        public string FolderName { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime EditTime { get; set; }
    }
}
