using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class dgfhgfs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignment_ProjectMembers_ProjectMemberId",
                table: "TaskAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignment_ProjectTasks_ProjectTaskId",
                table: "TaskAssignment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskAssignment",
                table: "TaskAssignment");

            migrationBuilder.DropIndex(
                name: "IX_TaskAssignment_ProjectMemberId",
                table: "TaskAssignment");

            migrationBuilder.RenameTable(
                name: "TaskAssignment",
                newName: "TaskAssignments");

            migrationBuilder.RenameIndex(
                name: "IX_TaskAssignment_ProjectTaskId",
                table: "TaskAssignments",
                newName: "IX_TaskAssignments_ProjectTaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskAssignments",
                table: "TaskAssignments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_ProjectMemberId",
                table: "TaskAssignments",
                column: "ProjectMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_ProjectMembers_ProjectMemberId",
                table: "TaskAssignments",
                column: "ProjectMemberId",
                principalTable: "ProjectMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_ProjectTasks_ProjectTaskId",
                table: "TaskAssignments",
                column: "ProjectTaskId",
                principalTable: "ProjectTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_ProjectMembers_ProjectMemberId",
                table: "TaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_ProjectTasks_ProjectTaskId",
                table: "TaskAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskAssignments",
                table: "TaskAssignments");

            migrationBuilder.DropIndex(
                name: "IX_TaskAssignments_ProjectMemberId",
                table: "TaskAssignments");

            migrationBuilder.RenameTable(
                name: "TaskAssignments",
                newName: "TaskAssignment");

            migrationBuilder.RenameIndex(
                name: "IX_TaskAssignments_ProjectTaskId",
                table: "TaskAssignment",
                newName: "IX_TaskAssignment_ProjectTaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskAssignment",
                table: "TaskAssignment",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignment_ProjectMemberId",
                table: "TaskAssignment",
                column: "ProjectMemberId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignment_ProjectMembers_ProjectMemberId",
                table: "TaskAssignment",
                column: "ProjectMemberId",
                principalTable: "ProjectMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignment_ProjectTasks_ProjectTaskId",
                table: "TaskAssignment",
                column: "ProjectTaskId",
                principalTable: "ProjectTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
