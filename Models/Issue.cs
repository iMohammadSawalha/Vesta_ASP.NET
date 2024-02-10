using System.ComponentModel.DataAnnotations;

namespace Vesta.Models
{
    public class Issue
    {
        public int IssueId { get; set; }

        [Required]
        [StringLength(64)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }
        
        public int? Parent { get; set; }

        [StringLength(256)]
        public string? AssigneeEmail { get; set; }
        public User? Assignee { get; set; }

        [Required]
        [StringLength(16)]
        public string Status { get; set; } = null!;

        [StringLength(64)]
        public string WorkspaceUrl { get; set; } = null!;
        public Workspace Workspace { get; set; } = null!;
    }
}