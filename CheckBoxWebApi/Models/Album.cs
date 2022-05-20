using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckBoxWebApi.Models
{
    public class Album
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

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
