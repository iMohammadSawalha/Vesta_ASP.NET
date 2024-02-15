using System.ComponentModel.DataAnnotations;

namespace Vesta.DataTransferObjects.User
{
    public class ConfirmationEmailRequestDTO
    {
        [Required(ErrorMessage = "Email is required!.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address!.")]
        public string Email { get; set; } = null!;
    }
}