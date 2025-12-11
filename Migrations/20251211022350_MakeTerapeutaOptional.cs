using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaludDigital.Migrations
{
    public partial class MakeTerapeutaOptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Citas_Doctors_TerapeutaId",
                table: "Citas");

            migrationBuilder.AddColumn<string>(
                name: "isDoctor",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "TerapeutaId",
                table: "Citas",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_Doctors_TerapeutaId",
                table: "Citas",
                column: "TerapeutaId",
                principalTable: "Doctors",
                principalColumn: "iDoctor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Citas_Doctors_TerapeutaId",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "isDoctor",
                table: "Users");

            migrationBuilder.AlterColumn<Guid>(
                name: "TerapeutaId",
                table: "Citas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_Doctors_TerapeutaId",
                table: "Citas",
                column: "TerapeutaId",
                principalTable: "Doctors",
                principalColumn: "iDoctor",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
