namespace MedicDate.Shared.Models.Medico
{
    public class MedicoCitaResponseDto
    {
        public string? Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public string? Cedula { get; set; }
        public string? FullInfo => $"{Nombre} {Apellidos} ({Cedula})";
    }
}