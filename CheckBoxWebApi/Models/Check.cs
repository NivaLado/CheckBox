using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckBoxWebApi.Models
{
    public class Check
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int AlbumId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime EditTime { get; set; }

        [MaxLength(256)]
        [Required]
        public string Url { get; set; }
    }
}
