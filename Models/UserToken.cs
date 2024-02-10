using System.ComponentModel.DataAnnotations;

namespace Vesta.Models
{
    public class UserToken
    {
        [Key]
        public Guid UUID { get; set; }

        [Required]
        public string RefreshToken { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User User { get; set; } = null!;

        [Required]
        [StringLength(256)]
        public string UserEmail { get; set; } = null!;
        
        public UserToken()
        {
            UUID = Guid.NewGuid();
        }
    }

}