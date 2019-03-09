namespace Wallspace.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LoginCredentials
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}