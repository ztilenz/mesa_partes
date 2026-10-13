namespace MesaDePartes;

/// <summary>
/// Cuentas del sistema (director, coordinador y profesores) e inicio de sesión.
/// Arreglos paralelos: la misma posición en todos los arreglos es la misma persona.
/// </summary>
public static class Usuarios
{
    public static readonly string[] Cuentas =
        { "director", "coordinador", "prof1", "prof2", "prof3", "prof4", "prof5" };

    public static readonly string[] Claves =
        { "dir2026", "coord2026", "prof1234", "prof2345", "prof3456", "prof4567", "prof5678" };

    public static readonly string[] Cargos =
        { "Director", "Coordinador", "Profesor", "Profesor", "Profesor", "Profesor", "Profesor" };

    public static readonly string[] Nombres =
        { "Luis Salazar", "Marta Vega", "Carmen Ruiz", "Jorge Paredes",
          "Elena Castro", "Daniel Rojas", "Sofía León" };

    // Posición del usuario que inició sesión (-1 = nadie).
    public static int Actual = -1;

    // Devuelve la posición de la cuenta o -1 si no existe.
    public static int BuscarCuenta(string cuenta)
    {
        return Validaciones.BuscarOpcion(Cuentas, cuenta);
    }

    // Ejemplo: "Profesor Carmen Ruiz"
    public static string Etiqueta(int pos)
    {
        return Cargos[pos] + " " + Nombres[pos];
    }

    // Convierte el nombre de cuenta guardado en el archivo a texto legible.
    public static string EtiquetaDeCuenta(string cuenta)
    {
        int pos = BuscarCuenta(cuenta);
        if (pos == -1)
            return cuenta;
        return Etiqueta(pos);
    }

    // El director y el coordinador pueden ver todos los trámites.
    public static bool EsDirectivo()
    {
        return Actual != -1 && Cargos[Actual] != "Profesor";
    }

    // Pide usuario y contraseña (máximo 3 intentos). Devuelve true si entró.
// Si deja el usuario vacío, cancela y vuelve al menú.
public static bool IniciarSesion()
{
    Console.WriteLine("*** INICIAR SESIÓN ***");
    Console.WriteLine("(Deje el usuario vacío y presione Enter para cancelar)");

    for (int intento = 1; intento <= 3; intento++)
    {
        Console.Write("Usuario: ");
        string cuenta = (Console.ReadLine() ?? "").Trim();

        if (cuenta == "")
        {
            Console.WriteLine("Inicio de sesión cancelado.");
            return false;
        }

        string clave = Validaciones.LeerClave("Contraseña: ");

        int pos = BuscarCuenta(cuenta);
        if (pos != -1 && Claves[pos] == clave)
        {
            Actual = pos;
            Console.WriteLine("Bienvenido(a), " + Etiqueta(pos) + ".");
            return true;
        }

        Console.WriteLine($"Usuario o contraseña incorrectos. Intento {intento} de 3.");
    }

    Console.WriteLine("Demasiados intentos fallidos.");
    return false;
}
    public static void CerrarSesion()
    {
        Actual = -1;
    }

    public static void MostrarLista()
    {
        for (int i = 0; i < Cuentas.Length; i++)
        {
            Console.WriteLine($"[{i + 1}] {Etiqueta(i)}");
        }
    }

    // Muestra la lista y pide elegir un destinatario. Devuelve su posición (0 a 6).
    public static int ElegirDestinatario(string mensaje)
    {
        MostrarLista();
        int numero;
        do
        {
            numero = Validaciones.LeerEntero(mensaje);
            if (numero < 1 || numero > Cuentas.Length)
                Console.WriteLine("Error: elija un número de la lista.");
        } while (numero < 1 || numero > Cuentas.Length);
        return numero - 1;
    }
}