using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicDate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddConfirmationChecksToCita : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailDayBeforeConfirm",
                table: "Cita",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EmailHoursBeforeConfirm",
                table: "Cita",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailDayBeforeConfirm",
                table: "Cita");

            migrationBuilder.DropColumn(
                name: "EmailHoursBeforeConfirm",
                table: "Cita");
        }
    }
}
