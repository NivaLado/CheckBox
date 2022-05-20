using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckBoxWebApi.Models
{
    public class AppUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(256)]
        public string VendorId { get; set; }

        [Required]
        public ushort AuthorizationMethod { get; set; }

        [MaxLength(24)]
        [Required]
        public string Email { get; set; }

        [MaxLength(24)]
        public string Password { get; set; }

        [MaxLength(24)]
        public string Name { get; set; }

        [MaxLength(24)]
        public string Surname { get; set; }

        public DateTime AccountCreated { get; set; }

        public DateTime LastLogin { get; set; }

        public string Picture { get; set; }
    }
}
