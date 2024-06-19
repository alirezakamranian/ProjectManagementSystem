using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class sgegw : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectTaskId",
                table: "TaskAssignment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignment_ProjectTaskId",
                table: "TaskAssignment",
                column: "ProjectTaskId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignment_ProjectTasks_ProjectTaskId",
                table: "TaskAssignment",
                column: "ProjectTaskId",
                principalTable: "ProjectTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignment_ProjectTasks_ProjectTaskId",
                table: "TaskAssignment");

            migrationBuilder.DropIndex(
                name: "IX_TaskAssignment_ProjectTaskId",
                table: "TaskAssignment");

            migrationBuilder.DropColumn(
                name: "ProjectTaskId",
                table: "TaskAssignment");
        }
    }
}
