namespace MesaDePartes;

/// <summary>
/// Funciones reutilizables para validar y leer datos de teclado.
/// </summary>
public static class Validaciones
{
    // El DNI debe tener exactamente 8 dígitos numéricos.
    public static bool ValidarDNI(string dni)
    {
        if (dni.Length != 8)
        {
            Console.WriteLine("Error: el DNI debe tener 8 dígitos.");
            return false;
        }

        for (int i = 0; i < dni.Length; i++)
        {
            if (!char.IsDigit(dni[i]))
            {
                Console.WriteLine("Error: el DNI solo debe contener números.");
                return false;
            }
        }
        return true;
    }

    // Busca un texto dentro de una lista de opciones (sin importar mayúsculas ni tildes simples).
    // Devuelve la posición o -1 si no existe.
    public static int BuscarOpcion(string[] opciones, string texto)
    {
        string buscado = Normalizar(texto);
        for (int i = 0; i < opciones.Length; i++)
        {
            if (Normalizar(opciones[i]) == buscado)
                return i;
        }
        return -1;
    }

    // Quita espacios, pasa a minúsculas y elimina tildes.
    public static string Normalizar(string texto)
    {
        string t = texto.Trim().ToLower();
        t = t.Replace('á', 'a').Replace('é', 'e').Replace('í', 'i')
             .Replace('ó', 'o').Replace('ú', 'u');
        return t;
    }

    // Evita que el texto rompa el formato del archivo (el separador es '|').
    public static string LimpiarTexto(string texto)
    {
        return texto.Replace('|', ' ').Trim();
    }

    // Lee un texto que no puede quedar vacío.
    public static string LeerTexto(string mensaje)
    {
        string valor;
        do
        {
            Console.Write(mensaje);
            valor = LimpiarTexto(Console.ReadLine() ?? "");
            if (valor == "")
                Console.WriteLine("Error: este campo no puede estar vacío.");
        } while (valor == "");
        return valor;
    }

    // Lee un número entero, repite hasta que sea válido.
    public static int LeerEntero(string mensaje)
    {
        int numero;
        Console.Write(mensaje);
        while (!int.TryParse(Console.ReadLine(), out numero))
        {
            Console.WriteLine("Error: ingrese un número válido.");
            Console.Write(mensaje);
        }
        return numero;
    }

    public static string FechaActual()
    {
        return DateTime.Now.ToString("dd/MM/yyyy HH:mm");
    }
}
