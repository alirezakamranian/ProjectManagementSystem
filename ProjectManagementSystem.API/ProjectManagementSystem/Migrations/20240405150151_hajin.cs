using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class hajin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectMemberTasks");

            migrationBuilder.DropTable(
                name: "ProjectTaskExecutiveAgent");

            migrationBuilder.DropTable(
                name: "ProjectMemberTaskLists");

            migrationBuilder.AddColumn<string>(
                name: "LeaderId",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaderId",
                table: "Projects");

            migrationBuilder.CreateTable(
                name: "ProjectMemberTaskLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMemberTaskLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectMemberTaskLists_ProjectMembers_ProjectMemberId",
                        column: x => x.ProjectMemberId,
                        principalTable: "ProjectMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTaskExecutiveAgent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTaskExecutiveAgent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTaskExecutiveAgent_ProjectMembers_ProjectMemberId",
                        column: x => x.ProjectMemberId,
                        principalTable: "ProjectMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTaskExecutiveAgent_ProjectTasks_ProjectTaskId",
                        column: x => x.ProjectTaskId,
                        principalTable: "ProjectTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMemberTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberTaskListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(700)", maxLength: 700, nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMemberTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectMemberTasks_ProjectMemberTaskLists_MemberTaskListId",
                        column: x => x.MemberTaskListId,
                        principalTable: "ProjectMemberTaskLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMemberTaskLists_ProjectMemberId",
                table: "ProjectMemberTaskLists",
                column: "ProjectMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMemberTasks_MemberTaskListId",
                table: "ProjectMemberTasks",
                column: "MemberTaskListId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTaskExecutiveAgent_ProjectMemberId",
                table: "ProjectTaskExecutiveAgent",
                column: "ProjectMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTaskExecutiveAgent_ProjectTaskId",
                table: "ProjectTaskExecutiveAgent",
                column: "ProjectTaskId");
        }
    }
}
