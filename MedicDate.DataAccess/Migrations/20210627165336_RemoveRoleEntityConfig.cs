using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicDate.DataAccess.Migrations
{
    public partial class RemoveRoleEntityConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "142b291b-4cda-4653-9f17-2739b6b1381b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ffa792ee-be61-4f90-9733-ecdde46c092f");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "142b291b-4cda-4653-9f17-2739b6b1381b", "e9f8b250-015e-4b5f-964e-b115909a2365", "Doctor", "DOCTOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ffa792ee-be61-4f90-9733-ecdde46c092f", "6c0d817d-87a4-47e7-b0a0-c64c8c596cab", "Administrador", "ADMINISTRADOR" });
        }
    }
}
