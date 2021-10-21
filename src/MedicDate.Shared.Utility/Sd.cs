namespace MedicDate.Utility
{
    public static class Sd
    {
        public const string ROLE_ADMIN = "Administrador";
        public const string ROLE_DOCTOR = "Doctor";
        public const string TOKEN_ACCESS = "authToken";
        public const string TOKEN_REFRESH = "refreshToken";
        public const string AUTH_TYPE_JWT = "jwtAuthType";

        public const string ESTADO_CITA_PORCONFIRMAR = "Por Confirmar";
        public const string ESTADO_CITA_ANULADA = "Anulada";
        public const string ESTADO_CITA_COMPLETADA = "Completada";
        public const string ESTADO_CITA_CONFIRMADA = "Confirmada";
        public const string ESTADO_CITA_NOASISTIOPACIENTE = "No asistió paciente";
        public const string ESTADO_CITA_CANCELADA = "Cancelada";

        public static IEnumerable<string> ListadoEstadosCita = new List<string>()
        {
            "Por Confirmar",
            "Anulada",
            "Confirmada",
            "No asistió paciente",
            "Cancelada",
         };
    }
}