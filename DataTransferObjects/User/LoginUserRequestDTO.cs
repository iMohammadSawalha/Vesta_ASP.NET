using System.ComponentModel.DataAnnotations;

namespace Vesta.DataTransferObjects.User
{
    public class LoginUserRequestDTO
    {
        [Required(ErrorMessage = "Email is required!.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address!.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required!.")]
        public string Password { get; set; } = null!;
    }
}