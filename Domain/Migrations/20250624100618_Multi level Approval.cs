using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MultilevelApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AdminApprovedAt",
                table: "TaskCards",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminApprovedBy",
                table: "TaskCards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdminApproved",
                table: "TaskCards",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsManagerApproved",
                table: "TaskCards",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ManagerApprovedAt",
                table: "TaskCards",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerApprovedBy",
                table: "TaskCards",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminApprovedAt",
                table: "TaskCards");

            migrationBuilder.DropColumn(
                name: "AdminApprovedBy",
                table: "TaskCards");

            migrationBuilder.DropColumn(
                name: "IsAdminApproved",
                table: "TaskCards");

            migrationBuilder.DropColumn(
                name: "IsManagerApproved",
                table: "TaskCards");

            migrationBuilder.DropColumn(
                name: "ManagerApprovedAt",
                table: "TaskCards");

            migrationBuilder.DropColumn(
                name: "ManagerApprovedBy",
                table: "TaskCards");
        }
    }
}
