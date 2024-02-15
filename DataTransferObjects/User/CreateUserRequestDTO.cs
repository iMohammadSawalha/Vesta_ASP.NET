using System.ComponentModel.DataAnnotations;

namespace Vesta.DataTransferObjects.User
{
    public class CreateUserRequestDTO
    {
        [Required(ErrorMessage = "Email is required!.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address!.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required!.")]
        [RegularExpression
        ("^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*\\W*).*$",
        ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number."
        )]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "An Email Confirmation Code is required!.")]
        public string EmailConfirmationCode { get; set; } = null!;
        public string? Image { get; set; }
    }
}