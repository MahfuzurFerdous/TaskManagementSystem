using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskCommentColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AssignedToUserId",
                table: "TaskCards",
                newName: "AssignedToUserName");

            migrationBuilder.RenameColumn(
                name: "AssignedByUserId",
                table: "TaskCards",
                newName: "AssignedByUserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AssignedToUserName",
                table: "TaskCards",
                newName: "AssignedToUserId");

            migrationBuilder.RenameColumn(
                name: "AssignedByUserName",
                table: "TaskCards",
                newName: "AssignedByUserId");
        }
    }
}
