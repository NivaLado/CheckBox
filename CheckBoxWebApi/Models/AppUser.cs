using System.ComponentModel.DataAnnotations;

namespace CheckBoxWebApi.Models
{
    public class AppUser
    {
        [Key]
        public long Id { get; set; }

        [MaxLength(256)]
        public string AuthorizationMethod { get; set; }

        [MaxLength(256)]
        [Required]
        public string Email { get; set; }

        [MaxLength(256)]
        public string Password { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Surname { get; set; }

        public DateTime AccountCreated { get; set; }

        public DateTime LastLogin { get; set; }
    }
}
