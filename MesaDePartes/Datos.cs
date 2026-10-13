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
    public static string[] Destinos = new string[Max];
    public static string[] Estados = new string[Max];
    public static string[] Fechas = new string[Max];        // fecha y hora de registro
    public static string[] RecibidoPor = new string[Max];   // quién recibió el documento
    public static string[] Movimientos = new string[Max];   // fecha y hora del último movimiento

    public static int Contador = 0;

    public static readonly string[] TiposValidos =
        { "justificacion", "vacante", "certificado", "constancia" };

    public static readonly string[] DestinosValidos =
        { "Direccion", "Subdireccion", "Profesor" };
}
