using System.ComponentModel.DataAnnotations;

namespace Vesta.Models
{
    public class Column
    {
        [StringLength(16)]
        public string ColumnStatusId { get; set; } = null!;

        [Required]
        [StringLength(16)]
        public string Title { get; set; } = null!;
        
        public string? Issues { get; set; }

        public Workspace Workspace { get; set; } = null!;

        [StringLength(64)]
        public string WorkspaceUrl { get; set; } = null!;

        [Required]
        public short Index { get; set; }

    }
}