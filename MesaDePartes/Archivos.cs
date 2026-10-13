namespace MesaDePartes;

/// <summary>
/// Persistencia de datos: guarda y carga los trámites en un archivo de texto.
/// Cada línea es un trámite con los campos separados por '|'.
/// </summary>
public static class Archivos
{
    private static readonly string Carpeta =
        Path.Combine(AppContext.BaseDirectory, "datos");

    private static readonly string Ruta =
        Path.Combine(Carpeta, "tramites.txt");

    public static void Guardar()
    {
        try
        {
            Directory.CreateDirectory(Carpeta);

            string[] lineas = new string[Datos.Contador];
            for (int i = 0; i < Datos.Contador; i++)
            {
                lineas[i] = string.Join("|",
                    Datos.Expedientes[i],
                    Datos.Solicitantes[i],
                    Datos.Dnis[i],
                    Datos.Tipos[i],
                    Datos.Destinos[i],
                    Datos.Estados[i],
                    Datos.Fechas[i],
                    Datos.RecibidoPor[i],
                    Datos.Movimientos[i],
                    Datos.Telefonos[i],
                    Datos.Parentescos[i],
                    Datos.Motivos[i]);
            }
            File.WriteAllLines(Ruta, lineas);
        }
        catch (IOException ex)
        {
            Console.WriteLine("Error al guardar el archivo: " + ex.Message);
        }
    }

    public static void Cargar()
    {
        Datos.Contador = 0;

        if (!File.Exists(Ruta))
            return;

        try
        {
            string[] lineas = File.ReadAllLines(Ruta);
            for (int i = 0; i < lineas.Length && Datos.Contador < Datos.Max; i++)
            {
                string[] campos = lineas[i].Split('|');
                if (campos.Length != 12 || !int.TryParse(campos[0], out int exp))
                    continue; // línea dañada: se ignora

                if (Usuarios.BuscarCuenta(campos[4]) == -1)
                    continue; // destinatario inexistente (datos de una versión anterior)

                int pos = Datos.Contador;
                Datos.Expedientes[pos] = exp;
                Datos.Solicitantes[pos] = campos[1];
                Datos.Dnis[pos] = campos[2];
                Datos.Tipos[pos] = campos[3];
                Datos.Destinos[pos] = campos[4];
                Datos.Estados[pos] = campos[5];
                Datos.Fechas[pos] = campos[6];
                Datos.RecibidoPor[pos] = campos[7];
                Datos.Movimientos[pos] = campos[8];
                Datos.Telefonos[pos] = campos[9];
                Datos.Parentescos[pos] = campos[10];
                Datos.Motivos[pos] = campos[11];
                Datos.Contador++;
            }
            Console.WriteLine($"Se cargaron {Datos.Contador} trámite(s) desde el archivo.");
        }
        catch (IOException ex)
        {
            Console.WriteLine("Error al leer el archivo: " + ex.Message);
        }
    }

    public static string RutaArchivo()
    {
        return Ruta;
    }
}