using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class Rname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskLabelAttachment_ProjectTasks_TaskId",
                table: "TaskLabelAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskLabelAttachment_TaskLables_LabelId",
                table: "TaskLabelAttachment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskLabelAttachment",
                table: "TaskLabelAttachment");

            migrationBuilder.RenameTable(
                name: "TaskLabelAttachment",
                newName: "TaskLabelAttachments");

            migrationBuilder.RenameIndex(
                name: "IX_TaskLabelAttachment_TaskId",
                table: "TaskLabelAttachments",
                newName: "IX_TaskLabelAttachments_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskLabelAttachment_LabelId",
                table: "TaskLabelAttachments",
                newName: "IX_TaskLabelAttachments_LabelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskLabelAttachments",
                table: "TaskLabelAttachments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLabelAttachments_ProjectTasks_TaskId",
                table: "TaskLabelAttachments",
                column: "TaskId",
                principalTable: "ProjectTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLabelAttachments_TaskLables_LabelId",
                table: "TaskLabelAttachments",
                column: "LabelId",
                principalTable: "TaskLables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskLabelAttachments_ProjectTasks_TaskId",
                table: "TaskLabelAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskLabelAttachments_TaskLables_LabelId",
                table: "TaskLabelAttachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskLabelAttachments",
                table: "TaskLabelAttachments");

            migrationBuilder.RenameTable(
                name: "TaskLabelAttachments",
                newName: "TaskLabelAttachment");

            migrationBuilder.RenameIndex(
                name: "IX_TaskLabelAttachments_TaskId",
                table: "TaskLabelAttachment",
                newName: "IX_TaskLabelAttachment_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskLabelAttachments_LabelId",
                table: "TaskLabelAttachment",
                newName: "IX_TaskLabelAttachment_LabelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskLabelAttachment",
                table: "TaskLabelAttachment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLabelAttachment_ProjectTasks_TaskId",
                table: "TaskLabelAttachment",
                column: "TaskId",
                principalTable: "ProjectTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLabelAttachment_TaskLables_LabelId",
                table: "TaskLabelAttachment",
                column: "LabelId",
                principalTable: "TaskLables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
