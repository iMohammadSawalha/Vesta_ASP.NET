using System.ComponentModel.DataAnnotations;

namespace Vesta.Models
{

    public enum UserRole
    {
    Admin,
    Member
    }
    public class WorkspaceUser
    {
        [StringLength(256)]
        public string UserEmail { get; set; } = null!;

        [StringLength(64)]
        public string WorkspaceUrl { get; set; } = null!;
        
        [Required]
        public UserRole UserRole { get; set; }

        public User User { get; set; } = null!;
        public Workspace Workspace { get; set; } = null!;
    }
}