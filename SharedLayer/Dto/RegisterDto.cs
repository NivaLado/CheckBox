namespace SharedLayer.Dto
{
    public class RegisterDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public ushort AuthorizationMethod { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string VendorId { get; set; }
    }
}
