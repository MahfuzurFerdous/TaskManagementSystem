using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Rejectoption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AdminRejectedAt",
                table: "TaskCards",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminRejectedBy",
                table: "TaskCards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ManagerRejectedAt",
                table: "TaskCards",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerRejectedBy",
                table: "TaskCards",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminRejectedAt",
                table: "TaskCards");

            migrationBuilder.DropColumn(
                name: "AdminRejectedBy",
                table: "TaskCards");

            migrationBuilder.DropColumn(
                name: "ManagerRejectedAt",
                table: "TaskCards");

            migrationBuilder.DropColumn(
                name: "ManagerRejectedBy",
                table: "TaskCards");
        }
    }
}
