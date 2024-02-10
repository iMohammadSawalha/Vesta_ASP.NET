using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vesta.Migrations
{
    public partial class InitialSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workspaces",
                columns: table => new
                {
                    Url = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    IdCounter = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workspaces", x => x.Url);
                });

            migrationBuilder.CreateTable(
                name: "Columns",
                columns: table => new
                {
                    ColumnStatusId = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    WorkspaceUrl = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Issues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Index = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Columns", x => new { x.WorkspaceUrl, x.ColumnStatusId });
                    table.ForeignKey(
                        name: "FK_Columns_Workspaces_WorkspaceUrl",
                        column: x => x.WorkspaceUrl,
                        principalTable: "Workspaces",
                        principalColumn: "Url",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultWorkspaceUrl = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Email);
                    table.ForeignKey(
                        name: "FK_Users_Workspaces_DefaultWorkspaceUrl",
                        column: x => x.DefaultWorkspaceUrl,
                        principalTable: "Workspaces",
                        principalColumn: "Url");
                });

            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    IssueId = table.Column<int>(type: "int", nullable: false),
                    WorkspaceUrl = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Parent = table.Column<int>(type: "int", nullable: true),
                    AssigneeEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => new { x.WorkspaceUrl, x.IssueId });
                    table.ForeignKey(
                        name: "FK_Issues_Users_AssigneeEmail",
                        column: x => x.AssigneeEmail,
                        principalTable: "Users",
                        principalColumn: "Email");
                    table.ForeignKey(
                        name: "FK_Issues_Workspaces_WorkspaceUrl",
                        column: x => x.WorkspaceUrl,
                        principalTable: "Workspaces",
                        principalColumn: "Url",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.UUID);
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkspaceUsers",
                columns: table => new
                {
                    UserEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    WorkspaceUrl = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    UserRole = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspaceUsers", x => new { x.UserEmail, x.WorkspaceUrl });
                    table.ForeignKey(
                        name: "FK_WorkspaceUsers_Users_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkspaceUsers_Workspaces_WorkspaceUrl",
                        column: x => x.WorkspaceUrl,
                        principalTable: "Workspaces",
                        principalColumn: "Url",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Issues_AssigneeEmail",
                table: "Issues",
                column: "AssigneeEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DefaultWorkspaceUrl",
                table: "Users",
                column: "DefaultWorkspaceUrl",
                unique: true,
                filter: "[DefaultWorkspaceUrl] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_UserEmail",
                table: "UserTokens",
                column: "UserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceUsers_WorkspaceUrl",
                table: "WorkspaceUsers",
                column: "WorkspaceUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Columns");

            migrationBuilder.DropTable(
                name: "Issues");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "WorkspaceUsers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Workspaces");
        }
    }
}
