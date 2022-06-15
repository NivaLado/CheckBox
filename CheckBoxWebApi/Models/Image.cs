using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckBoxWebApi.Models
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Album))]
        public int AlbumId { get; set; }

        [MaxLength(40)]
        public string ImageName { get; set; }
    }
}
