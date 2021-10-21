namespace MedicDate.Shared.Models.Paciente
{
    public class PacienteCitaResponseDto
    {
        public string? Id { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Cedula { get; set; }
        public string? NumHistoria { get; set; }
        public string? FullInfo => $"{Nombres} {Apellidos} - {Cedula} ({NumHistoria})";
    }
}