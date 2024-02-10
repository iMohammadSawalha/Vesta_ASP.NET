using System.ComponentModel.DataAnnotations;

namespace Vesta.Models
{
    public class Workspace
    {
        [Key]
        [StringLength(64)]
        public string Url { get; set; } = null!;

        [Required]
        [StringLength(64)]
        public string Name { get; set; } = null!;

        [Required]
        public string Image { get; set; } = null!;

        [Required]
        [StringLength(4)]
        public string Symbol { get; set; } = null!;
        public int IdCounter { get; set; } = 1;

        public ICollection<Issue>? Issues { get; set; }

        public ICollection<Column>? Columns { get; set; }

        public ICollection<WorkspaceUser> WorkspaceUsers { get; set; } = null!;

        public void CreateDefaultColumns()
            {
                Columns = new List<Column>
                {
                    new() { WorkspaceUrl = Url, ColumnStatusId = "backlog", Title = "Backlog", Index =  0 },
                    new() { WorkspaceUrl = Url, ColumnStatusId = "todo", Title = "Todo", Index =  1 },
                    new() { WorkspaceUrl = Url, ColumnStatusId = "inprogress", Title = "In Progress", Index =  2 },
                    new() { WorkspaceUrl = Url, ColumnStatusId = "done", Title = "Done", Index =  3 }
                };
            }
    }
}