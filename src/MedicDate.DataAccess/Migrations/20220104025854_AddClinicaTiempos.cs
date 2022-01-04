using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicDate.DataAccess.Migrations
{
    public partial class AddClinicaTiempos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "HoraApertura",
                table: "Clinica",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "HoraCerrado",
                table: "Clinica",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoraApertura",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "HoraCerrado",
                table: "Clinica");
        }
    }
}
