using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicDate.DataAccess.Migrations
{
    public partial class FixPaginaDropDownRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagina_PaginaDropDown_PaginaDropDownId",
                table: "Pagina");

            migrationBuilder.DropIndex(
                name: "IX_Pagina_PaginaDropDownId",
                table: "Pagina");

            migrationBuilder.DropColumn(
                name: "PaginaDropDownId",
                table: "Pagina");

            migrationBuilder.CreateTable(
                name: "PaginaPaginaDropDown",
                columns: table => new
                {
                    PaginaDropDownsId = table.Column<int>(type: "int", nullable: false),
                    PaginasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaginaPaginaDropDown", x => new { x.PaginaDropDownsId, x.PaginasId });
                    table.ForeignKey(
                        name: "FK_PaginaPaginaDropDown_Pagina_PaginasId",
                        column: x => x.PaginasId,
                        principalTable: "Pagina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaginaPaginaDropDown_PaginaDropDown_PaginaDropDownsId",
                        column: x => x.PaginaDropDownsId,
                        principalTable: "PaginaDropDown",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "142b291b-4cda-4653-9f17-2739b6b1381b",
                column: "ConcurrencyStamp",
                value: "e9f8b250-015e-4b5f-964e-b115909a2365");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ffa792ee-be61-4f90-9733-ecdde46c092f",
                column: "ConcurrencyStamp",
                value: "6c0d817d-87a4-47e7-b0a0-c64c8c596cab");

            migrationBuilder.CreateIndex(
                name: "IX_PaginaPaginaDropDown_PaginasId",
                table: "PaginaPaginaDropDown",
                column: "PaginasId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaginaPaginaDropDown");

            migrationBuilder.AddColumn<int>(
                name: "PaginaDropDownId",
                table: "Pagina",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "142b291b-4cda-4653-9f17-2739b6b1381b",
                column: "ConcurrencyStamp",
                value: "770873fb-0a86-482c-927b-9b4bb33ef81e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ffa792ee-be61-4f90-9733-ecdde46c092f",
                column: "ConcurrencyStamp",
                value: "04433a0a-3586-4543-8550-3237c42ad625");

            migrationBuilder.CreateIndex(
                name: "IX_Pagina_PaginaDropDownId",
                table: "Pagina",
                column: "PaginaDropDownId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagina_PaginaDropDown_PaginaDropDownId",
                table: "Pagina",
                column: "PaginaDropDownId",
                principalTable: "PaginaDropDown",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
