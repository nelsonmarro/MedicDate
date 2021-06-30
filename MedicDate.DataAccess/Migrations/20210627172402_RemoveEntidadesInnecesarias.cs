using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicDate.DataAccess.Migrations
{
    public partial class RemoveEntidadesInnecesarias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaginaPaginaDropDown");

            migrationBuilder.DropTable(
                name: "PaginaUsuarioRolBoton");

            migrationBuilder.DropTable(
                name: "PaginaDropDown");

            migrationBuilder.DropTable(
                name: "Boton");

            migrationBuilder.DropTable(
                name: "PaginaUsuarioRol");

            migrationBuilder.DropTable(
                name: "Pagina");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "Pagina",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EsVisible = table.Column<bool>(type: "bit", nullable: false),
                    Icono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ruta = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagina", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaginaDropDown",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Icono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaginaDropDown", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaginaUsuarioRol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdIdentityRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdPagina = table.Column<int>(type: "int", nullable: false),
                    IdentityRoleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PaginaId = table.Column<int>(type: "int", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "PaginaUsuarioRolBoton",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BotonId = table.Column<int>(type: "int", nullable: true),
                    IdBoton = table.Column<int>(type: "int", nullable: false),
                    IdPaginaUsuarioRol = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_PaginaPaginaDropDown_PaginasId",
                table: "PaginaPaginaDropDown",
                column: "PaginasId");

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
    }
}
