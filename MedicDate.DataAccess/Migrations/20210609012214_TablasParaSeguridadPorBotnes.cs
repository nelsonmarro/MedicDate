using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicDate.DataAccess.Migrations
{
    public partial class TablasParaSeguridadPorBotnes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boton",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boton", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaginaDropDown",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icono = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaginaDropDown", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pagina",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ruta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EsVisible = table.Column<bool>(type: "bit", nullable: false),
                    PaginaDropDownId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagina", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagina_PaginaDropDown_PaginaDropDownId",
                        column: x => x.PaginaDropDownId,
                        principalTable: "PaginaDropDown",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaginaUsuarioRol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPagina = table.Column<int>(type: "int", nullable: false),
                    IdIdentityRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaginaId = table.Column<int>(type: "int", nullable: true),
                    IdentityRoleId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaginaUsuarioRol", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaginaUsuarioRol_AspNetRoles_IdentityRoleId",
                        column: x => x.IdentityRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaginaUsuarioRol_Pagina_PaginaId",
                        column: x => x.PaginaId,
                        principalTable: "Pagina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaginaUsuarioRolBoton",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPaginaUsuarioRol = table.Column<int>(type: "int", nullable: false),
                    IdBoton = table.Column<int>(type: "int", nullable: false),
                    BotonId = table.Column<int>(type: "int", nullable: true),
                    PaginaUsuarioRolId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaginaUsuarioRolBoton", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaginaUsuarioRolBoton_Boton_BotonId",
                        column: x => x.BotonId,
                        principalTable: "Boton",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaginaUsuarioRolBoton_PaginaUsuarioRol_PaginaUsuarioRolId",
                        column: x => x.PaginaUsuarioRolId,
                        principalTable: "PaginaUsuarioRol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_PaginaUsuarioRol_IdentityRoleId",
                table: "PaginaUsuarioRol",
                column: "IdentityRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PaginaUsuarioRol_PaginaId",
                table: "PaginaUsuarioRol",
                column: "PaginaId");

            migrationBuilder.CreateIndex(
                name: "IX_PaginaUsuarioRolBoton_BotonId",
                table: "PaginaUsuarioRolBoton",
                column: "BotonId");

            migrationBuilder.CreateIndex(
                name: "IX_PaginaUsuarioRolBoton_PaginaUsuarioRolId",
                table: "PaginaUsuarioRolBoton",
                column: "PaginaUsuarioRolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaginaUsuarioRolBoton");

            migrationBuilder.DropTable(
                name: "Boton");

            migrationBuilder.DropTable(
                name: "PaginaUsuarioRol");

            migrationBuilder.DropTable(
                name: "Pagina");

            migrationBuilder.DropTable(
                name: "PaginaDropDown");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "142b291b-4cda-4653-9f17-2739b6b1381b",
                column: "ConcurrencyStamp",
                value: "3d7b9152-0256-4a2c-ac7a-297bfd2899a4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ffa792ee-be61-4f90-9733-ecdde46c092f",
                column: "ConcurrencyStamp",
                value: "ec5c409d-2843-464d-9c83-be618218fbf9");
        }
    }
}
