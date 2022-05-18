using System.ComponentModel.DataAnnotations;

namespace CheckBoxWebApi.Models
{
    public class Check
    {
        [Key]
        public long Id { get; set; }

        public long AlbumId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime EditTime { get; set; }

        [MaxLength(256)]
        [Required]
        public string Url { get; set; }
    }
}
