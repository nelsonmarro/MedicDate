using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicDate.DataAccess.Migrations
{
    public partial class RoleSeedFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eedd1c1f-644a-407e-8c44-0196ce66ffaf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2771c1a-6c51-4a78-833b-3aaa1caab65e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fa0d312b-e90d-437e-ba98-c503bd38f5d3", "b99cf1c9-9dbe-4b8d-bb02-4462beb48aca", "Doctor", "DOCTOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6dea9432-502d-4e34-8b26-1fdc51db10ef", "efff719b-4279-40e0-b904-2ba201a8a0f0", "Administrador", "ADMINISTRADOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { "eedd1c1f-644a-407e-8c44-0196ce66ffaf", "d8f5781f-80f6-4205-acd6-93ff19c209e7", "Doctor", "DOCTOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f2771c1a-6c51-4a78-833b-3aaa1caab65e", "c23ec67e-6ec0-4b5e-b593-a6006379c136", "Administrador", "ADMINISTRADOR" });
        }
    }
}
