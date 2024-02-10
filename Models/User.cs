using System.ComponentModel.DataAnnotations;

namespace Vesta.Models
{
    public class User
    {
        [Key]
        [StringLength(256)]
        public string Email { get; set; } = null!;
        public string? Image { get; set; }

        public List<Issue>? Issues { get; set; }

        [Required]
        public string HashedPassword { get; set; } = null!;

        public ICollection<WorkspaceUser>? WorkspaceUsers { get; set; }

        [StringLength(64)]
        public string? DefaultWorkspaceUrl { get; set; }
        public Workspace Workspace { get; set; } = null!;

        public ICollection<UserToken>? UserTokens { get; set; }
    }
}