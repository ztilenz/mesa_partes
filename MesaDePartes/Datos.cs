namespace MesaDePartes;

/// <summary>
/// Almacena los datos del sistema en arreglos paralelos:
/// la misma posición en todos los arreglos corresponde al mismo trámite.
/// </summary>
public static class Datos
{
    public const int Max = 100;

    public static int[] Expedientes = new int[Max];
    public static string[] Solicitantes = new string[Max];
    public static string[] Dnis = new string[Max];
    public static string[] Tipos = new string[Max];
    public static string[] Destinos = new string[Max];      // cuenta de quien debe atender el trámite (ej. "prof1")
    public static string[] Estados = new string[Max];
    public static string[] Fechas = new string[Max];        // fecha y hora de registro
    public static string[] RecibidoPor = new string[Max];   // quién recibió el documento
    public static string[] Movimientos = new string[Max];   // fecha y hora del último movimiento
    public static string[] Telefonos = new string[Max];     // teléfono de contacto del solicitante
    public static string[] Parentescos = new string[Max];   // relación del solicitante con el alumno
    public static string[] Motivos = new string[Max];       // motivo (solo en justificaciones; "-" en los demás)

    public static int Contador = 0;

    public static readonly string[] ParentescosValidos =
        { "Madre", "Padre", "Tío(a)", "Abuelo(a)", "Hermano(a)", "Apoderado(a)", "Otro" };

    public static readonly string[] TiposValidos =
        { "justificacion", "vacante", "certificado", "constancia" };
}