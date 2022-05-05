using System.ComponentModel.DataAnnotations;

namespace CheckBoxWebApi.Models
{
    public class AppUser
    {
        public long Id { get; set; }

        [Key]
        [MaxLength(256)]
        public string UserId { get; set; }

        [MaxLength(256)]
        public string Email { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Surname { get; set; }

        public DateTime AccountCreated { get; set; }

        public DateTime LastLogin { get; set; }
    }
}
