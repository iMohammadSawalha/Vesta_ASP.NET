using Microsoft.EntityFrameworkCore;
using Vesta.Models;

namespace Vesta.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Workspace> Workspaces { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserToken> UserTokens { get; set; } = null!;
        public DbSet<Issue> Issues { get; set; } = null!;
        public DbSet<Column> Columns { get; set; } = null!;
        public DbSet<WorkspaceUser> WorkspaceUsers { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Compund key decleration
            modelBuilder.Entity<WorkspaceUser>()
                .HasKey(wu => new {wu.UserEmail, wu.WorkspaceUrl});
            
            modelBuilder.Entity<Issue>()
                .HasKey(i => new {i.WorkspaceUrl, i.IssueId});

            modelBuilder.Entity<Column>()
                .HasKey(c => new {c.WorkspaceUrl, c.ColumnStatusId});


            //- Many to Many -//

            // (1) Wrokspace to (Many) User(s)
            modelBuilder.Entity<WorkspaceUser>()
                .HasOne(w => w.Workspace)
                .WithMany(wu => wu.WorkspaceUsers)
                .HasForeignKey(u => u.WorkspaceUrl);
            // (Many) Wrokspace(s) to (1) User
            modelBuilder.Entity<WorkspaceUser>()
                .HasOne(w => w.User)
                .WithMany(wu => wu.WorkspaceUsers)
                .HasForeignKey(u => u.UserEmail);



            // (Many) Issue(s) to (1) User
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Assignee)
                .WithMany(u => u.Issues)
                .HasForeignKey(i => i.AssigneeEmail);



            // (Many) Issue(s) to (1) Workspace
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Workspace)
                .WithMany(c => c.Issues)
                .HasForeignKey(i => i.WorkspaceUrl);



            // (Many) Column(s) to (1) Workspace
            modelBuilder.Entity<Column>()
                .HasOne(c => c.Workspace)
                .WithMany(w => w.Columns)
                .HasForeignKey(c => c.WorkspaceUrl);



            // (Many) Token(s) to (1) User
            modelBuilder.Entity<UserToken>()
                .HasOne(t => t.User)
                .WithMany(u => u.UserTokens)
                .HasForeignKey(t => t.UserEmail);

        }
    }
}