namespace MedicDate.Utility;

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

   public const string AZ_STORAGE_CONTAINER_PACIENTE = "pacientes";

   public static List<string> ListadoEstadosCita = new List<string>
  {
    "Completada",
    "Por Confirmar",
    "Anulada",
    "Confirmada",
    "No asistió paciente",
    "Cancelada"
  };

   public static readonly DateTime Januay = new DateTime(DateTime.Now.Year, 1, 1);
   public static readonly DateTime February = new DateTime(DateTime.Now.Year, 2, 1);
   public static readonly DateTime March = new DateTime(DateTime.Now.Year, 3, 1);
   public static readonly DateTime April = new DateTime(DateTime.Now.Year, 4, 1);
   public static readonly DateTime May = new DateTime(DateTime.Now.Year, 5, 1);
   public static readonly DateTime June = new DateTime(DateTime.Now.Year, 6, 1);
   public static readonly DateTime July = new DateTime(DateTime.Now.Year, 7, 1);
   public static readonly DateTime August = new DateTime(DateTime.Now.Year, 8, 1);
   public static readonly DateTime September = new DateTime(DateTime.Now.Year, 9, 1);
   public static readonly DateTime October = new DateTime(DateTime.Now.Year, 10, 1);
   public static readonly DateTime November = new DateTime(DateTime.Now.Year, 11, 1);
   public static readonly DateTime December = new DateTime(DateTime.Now.Year, 12, 1);
}