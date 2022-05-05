using System.ComponentModel.DataAnnotations;

namespace CheckBoxWebApi.Models
{
    public class Check
    {
        public long Id { get; set; }

        [Key]
        [MaxLength(256)]
        public string AlbumId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime EditTime { get; set; }
    }
}
