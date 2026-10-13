namespace MesaDePartes;

/// <summary>
/// Módulos principales de la mesa de partes: registrar, mostrar, buscar, derivar y ordenar.
/// </summary>
public static class Tramites
{
    // El siguiente número de expediente es el mayor existente + 1.
    // Así no se repite aunque los trámites hayan sido reordenados.
    private static int SiguienteExpediente()
    {
        int mayor = 0;
        for (int i = 0; i < Datos.Contador; i++)
        {
            if (Datos.Expedientes[i] > mayor)
                mayor = Datos.Expedientes[i];
        }
        return mayor + 1;
    }

    // Devuelve la posición del expediente en los arreglos, o -1 si no existe.
    public static int BuscarPosicion(int numero)
    {
        for (int i = 0; i < Datos.Contador; i++)
        {
            if (Datos.Expedientes[i] == numero)
                return i;
        }
        return -1;
    }

    public static void Registrar()
    {
        Console.WriteLine("*** REGISTRO DE TRÁMITE ***");

        if (Datos.Contador >= Datos.Max)
        {
            Console.WriteLine("Capacidad máxima alcanzada.");
            return;
        }

        string solicitante = Validaciones.LeerTexto("Ingresar nombre del solicitante: ");

        string dni;
        do
        {
            Console.Write("Ingresar DNI: ");
            dni = (Console.ReadLine() ?? "").Trim();
        } while (!Validaciones.ValidarDNI(dni));

        string tipo;
        int posTipo;
        do
        {
            Console.Write("Ingresar tipo de trámite (justificación, vacante, certificado, constancia): ");
            tipo = (Console.ReadLine() ?? "").Trim();
            posTipo = Validaciones.BuscarOpcion(Datos.TiposValidos, tipo);
            if (posTipo == -1)
                Console.WriteLine("Error: tipo de trámite no válido.");
        } while (posTipo == -1);

        string encargado = Validaciones.LeerTexto("Ingresar nombre de quien recibe el documento: ");

        int pos = Datos.Contador;
        int expediente = SiguienteExpediente();
        string ahora = Validaciones.FechaActual();

        Datos.Expedientes[pos] = expediente;
        Datos.Solicitantes[pos] = solicitante;
        Datos.Dnis[pos] = dni;
        Datos.Tipos[pos] = Datos.TiposValidos[posTipo];
        Datos.Destinos[pos] = "Mesa de partes";
        Datos.Estados[pos] = "Recibido";
        Datos.Fechas[pos] = ahora;
        Datos.RecibidoPor[pos] = encargado;
        Datos.Movimientos[pos] = ahora;
        Datos.Contador++;

        Archivos.Guardar();
        Console.WriteLine("Trámite registrado. N° de expediente: " + expediente);
    }

    public static void Mostrar()
    {
        Console.WriteLine("*** LISTADO DE TRÁMITES ***");

        if (Datos.Contador == 0)
        {
            Console.WriteLine("No hay trámites registrados.");
            return;
        }

        for (int i = 0; i < Datos.Contador; i++)
        {
            Console.WriteLine(
                $"Exp. {Datos.Expedientes[i]} | {Datos.Solicitantes[i]} | {Datos.Tipos[i]} | " +
                $"Destino: {Datos.Destinos[i]} | Estado: {Datos.Estados[i]} | {Datos.Fechas[i]}");
        }
    }

    public static void Buscar()
    {
        Console.WriteLine("*** BUSCAR EXPEDIENTE ***");

        if (Datos.Contador == 0)
        {
            Console.WriteLine("No hay trámites registrados.");
            return;
        }

        int numero = Validaciones.LeerEntero("Ingresar número de expediente: ");
        int i = BuscarPosicion(numero);

        if (i == -1)
        {
            Console.WriteLine("Expediente no encontrado.");
            return;
        }

        Console.WriteLine("Solicitante: " + Datos.Solicitantes[i]);
        Console.WriteLine("DNI: " + Datos.Dnis[i]);
        Console.WriteLine("Trámite: " + Datos.Tipos[i]);
        Console.WriteLine("Recibido por: " + Datos.RecibidoPor[i]);
        Console.WriteLine("Fecha de registro: " + Datos.Fechas[i]);
        Console.WriteLine("Ubicación actual: " + Datos.Destinos[i]);
        Console.WriteLine("Estado: " + Datos.Estados[i]);
        Console.WriteLine("Último movimiento: " + Datos.Movimientos[i]);
    }

    public static void Derivar()
    {
        Console.WriteLine("*** DERIVAR TRÁMITE ***");

        if (Datos.Contador == 0)
        {
            Console.WriteLine("No hay trámites registrados.");
            return;
        }

        int numero = Validaciones.LeerEntero("Ingresar número de expediente: ");
        int i = BuscarPosicion(numero);

        if (i == -1)
        {
            Console.WriteLine("Expediente no encontrado.");
            return;
        }

        Console.WriteLine("Ubicación actual: " + Datos.Destinos[i]);

        string nuevo;
        int posDestino;
        do
        {
            Console.Write("Derivar a (Dirección, Subdirección, Profesor): ");
            nuevo = (Console.ReadLine() ?? "").Trim();
            posDestino = Validaciones.BuscarOpcion(Datos.DestinosValidos, nuevo);
            if (posDestino == -1)
                Console.WriteLine("Error: destino no válido.");
        } while (posDestino == -1);

        Datos.Destinos[i] = Datos.DestinosValidos[posDestino];
        Datos.Estados[i] = "Derivado";
        Datos.Movimientos[i] = Validaciones.FechaActual();

        Archivos.Guardar();
        Console.WriteLine("Trámite derivado a: " + Datos.Destinos[i]);
    }

    // Ordena de mayor a menor número de expediente (más recientes primero) con burbuja.
    // Al ser arreglos paralelos, cada intercambio mueve todos los datos del trámite.
    public static void Ordenar()
    {
        for (int i = 0; i < Datos.Contador - 1; i++)
        {
            for (int j = 0; j < Datos.Contador - 1 - i; j++)
            {
                if (Datos.Expedientes[j] < Datos.Expedientes[j + 1])
                {
                    (Datos.Expedientes[j], Datos.Expedientes[j + 1]) = (Datos.Expedientes[j + 1], Datos.Expedientes[j]);
                    Intercambiar(Datos.Solicitantes, j);
                    Intercambiar(Datos.Dnis, j);
                    Intercambiar(Datos.Tipos, j);
                    Intercambiar(Datos.Destinos, j);
                    Intercambiar(Datos.Estados, j);
                    Intercambiar(Datos.Fechas, j);
                    Intercambiar(Datos.RecibidoPor, j);
                    Intercambiar(Datos.Movimientos, j);
                }
            }
        }

        Archivos.Guardar();
        Console.WriteLine("Trámites ordenados por expediente (más recientes primero).");
    }

    private static void Intercambiar(string[] arreglo, int j)
    {
        string temp = arreglo[j];
        arreglo[j] = arreglo[j + 1];
        arreglo[j + 1] = temp;
    }
}
