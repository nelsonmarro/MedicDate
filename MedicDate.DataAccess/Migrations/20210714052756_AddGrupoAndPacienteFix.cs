using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicDate.DataAccess.Migrations
{
    public partial class AddGrupoAndPacienteFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ciudad",
                table: "Paciente");

            migrationBuilder.RenameColumn(
                name: "Edad",
                table: "Paciente",
                newName: "NumHistoria");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Paciente",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sexo",
                table: "Paciente",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ArchivoPaciente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RutaArchivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PacienteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchivoPaciente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArchivoPaciente_Paciente_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Paciente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grupo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GrupoPaciente",
                columns: table => new
                {
                    PacienteId = table.Column<int>(type: "int", nullable: false),
                    GrupoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoPaciente", x => new { x.GrupoId, x.PacienteId });
                    table.ForeignKey(
                        name: "FK_GrupoPaciente_Grupo_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrupoPaciente_Paciente_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Paciente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArchivoPaciente_PacienteId",
                table: "ArchivoPaciente",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_GrupoPaciente_PacienteId",
                table: "GrupoPaciente",
                column: "PacienteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArchivoPaciente");

            migrationBuilder.DropTable(
                name: "GrupoPaciente");

            migrationBuilder.DropTable(
                name: "Grupo");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Paciente");

            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "Paciente");

            migrationBuilder.RenameColumn(
                name: "NumHistoria",
                table: "Paciente",
                newName: "Edad");

            migrationBuilder.AddColumn<string>(
                name: "Ciudad",
                table: "Paciente",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);
        }
    }
}
