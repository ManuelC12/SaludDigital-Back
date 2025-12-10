using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaludDigital.Migrations
{
    public partial class AddCitasAndRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "PasswordResetToken",
            //    table: "Users");

            //migrationBuilder.DropColumn(
            //    name: "ResetTokenExpires",
            //    table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpires",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }
    }
}
