using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicDate.DataAccess.Migrations
{
    public partial class AddUniqueConstrains : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Paciente_Cedula",
                table: "Paciente",
                column: "Cedula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_NumHistoria",
                table: "Paciente",
                column: "NumHistoria",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medico_Cedula",
                table: "Medico",
                column: "Cedula",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Paciente_Cedula",
                table: "Paciente");

            migrationBuilder.DropIndex(
                name: "IX_Paciente_NumHistoria",
                table: "Paciente");

            migrationBuilder.DropIndex(
                name: "IX_Medico_Cedula",
                table: "Medico");
        }
    }
}
