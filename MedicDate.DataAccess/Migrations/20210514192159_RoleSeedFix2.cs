using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicDate.DataAccess.Migrations
{
    public partial class RoleSeedFix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6dea9432-502d-4e34-8b26-1fdc51db10ef");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fa0d312b-e90d-437e-ba98-c503bd38f5d3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "142b291b-4cda-4653-9f17-2739b6b1381b", "0d4b2380-1077-496e-9315-2a05a62ab6b5", "Doctor", "DOCTOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ffa792ee-be61-4f90-9733-ecdde46c092f", "d2f6d740-8963-4725-917f-8ccc7f41f58a", "Administrador", "ADMINISTRADOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "142b291b-4cda-4653-9f17-2739b6b1381b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ffa792ee-be61-4f90-9733-ecdde46c092f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fa0d312b-e90d-437e-ba98-c503bd38f5d3", "b99cf1c9-9dbe-4b8d-bb02-4462beb48aca", "Doctor", "DOCTOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6dea9432-502d-4e34-8b26-1fdc51db10ef", "efff719b-4279-40e0-b904-2ba201a8a0f0", "Administrador", "ADMINISTRADOR" });
        }
    }
}
