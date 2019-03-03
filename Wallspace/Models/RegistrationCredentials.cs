namespace Wallspace.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RegistrationCredentials
    {
        [Required]
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }
    }
}