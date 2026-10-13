namespace MesaDePartes;

/// <summary>
/// Módulos principales de la mesa de partes: registrar, mostrar, buscar, derivar y ordenar.
/// Cada trámite está dirigido a un usuario (director, coordinador o profesor).
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

    // Un usuario ve un trámite si está dirigido a él; el director y el coordinador ven todos.
    private static bool PuedeVer(int i)
    {
        if (Usuarios.EsDirectivo())
            return true;
        return Datos.Destinos[i] == Usuarios.Cuentas[Usuarios.Actual];
    }

    // Muestra la lista de parentescos y devuelve el elegido.
    // Si elige "Otro", pide que lo escriba.
    private static string ElegirParentesco()
    {
        for (int i = 0; i < Datos.ParentescosValidos.Length; i++)
        {
            Console.WriteLine($"[{i + 1}] {Datos.ParentescosValidos[i]}");
        }

        int numero;
        do
        {
            numero = Validaciones.LeerEntero("Elegir parentesco: ");
            if (numero < 1 || numero > Datos.ParentescosValidos.Length)
                Console.WriteLine("Error: elija un número de la lista.");
        } while (numero < 1 || numero > Datos.ParentescosValidos.Length);

        string parentesco = Datos.ParentescosValidos[numero - 1];
        if (parentesco == "Otro")
            parentesco = Validaciones.LeerTexto("Especifique el parentesco: ");

        return parentesco;
    }

    // Registro hecho en mesa de partes (no requiere iniciar sesión).
    public static void Registrar()
    {
        Console.WriteLine("*** REGISTRO DE TRÁMITE ***");

        if (Datos.Contador >= Datos.Max)
        {
            Console.WriteLine("Capacidad máxima alcanzada.");
            return;
        }

        string solicitante = Validaciones.LeerTexto("Ingresar nombre del solicitante: ");

        Console.WriteLine();
        Console.WriteLine("¿Qué es el solicitante del alumno?");
        string parentesco = ElegirParentesco();
        Console.WriteLine();

        string dni;
        do
        {
            Console.Write("Ingresar DNI: ");
            dni = (Console.ReadLine() ?? "").Trim();
        } while (!Validaciones.ValidarDNI(dni));

        string telefono;
        do
        {
            Console.Write("Ingresar teléfono (9 dígitos): ");
            telefono = (Console.ReadLine() ?? "").Trim();
        } while (!Validaciones.ValidarTelefono(telefono));

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

        // Solo la justificación lleva motivo.
        string motivo = "-";
        if (Datos.TiposValidos[posTipo] == "justificacion")
            motivo = Validaciones.LeerTexto("Ingresar motivo de la justificación: ");

        Console.WriteLine();
        Console.WriteLine("¿A quién va dirigido el trámite?");
        int posDestino = Usuarios.ElegirDestinatario("Elegir destinatario: ");
        Console.WriteLine();

        string encargado = Validaciones.LeerTexto("Ingresar nombre de quien recibe el documento: ");

        int pos = Datos.Contador;
        int expediente = SiguienteExpediente();
        string ahora = Validaciones.FechaActual();

        Datos.Expedientes[pos] = expediente;
        Datos.Solicitantes[pos] = solicitante;
        Datos.Dnis[pos] = dni;
        Datos.Tipos[pos] = Datos.TiposValidos[posTipo];
        Datos.Destinos[pos] = Usuarios.Cuentas[posDestino];
        Datos.Estados[pos] = "Recibido";
        Datos.Fechas[pos] = ahora;
        Datos.RecibidoPor[pos] = encargado;
        Datos.Movimientos[pos] = ahora;
        Datos.Telefonos[pos] = telefono;
        Datos.Parentescos[pos] = parentesco;
        Datos.Motivos[pos] = motivo;
        Datos.Contador++;

        Archivos.Guardar();
        Console.WriteLine("Trámite registrado. N° de expediente: " + expediente);
        Console.WriteLine("Dirigido a: " + Usuarios.Etiqueta(posDestino));
    }

    // todos = true muestra todos los trámites (solo director y coordinador);
    // todos = false muestra únicamente los dirigidos al usuario actual.
    public static void Mostrar(bool todos)
    {
        if (todos)
            Console.WriteLine("*** TODOS LOS TRÁMITES ***");
        else
            Console.WriteLine("*** MIS TRÁMITES ***");

        int mostrados = 0;
        for (int i = 0; i < Datos.Contador; i++)
        {
            bool mio = Datos.Destinos[i] == Usuarios.Cuentas[Usuarios.Actual];
            if (!todos && !mio)
                continue;

            Console.WriteLine(
                $"Exp. {Datos.Expedientes[i]} | {Datos.Solicitantes[i]} | {Datos.Tipos[i]} | " +
                $"Dirigido a: {Usuarios.EtiquetaDeCuenta(Datos.Destinos[i])} | " +
                $"Estado: {Datos.Estados[i]} | {Datos.Fechas[i]}");

            // Las justificaciones muestran además su motivo.
            if (Datos.Tipos[i] == "justificacion")
                Console.WriteLine("   Motivo: " + Datos.Motivos[i]);

            mostrados++;
        }

        if (mostrados == 0)
            Console.WriteLine("No hay trámites para mostrar.");
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

        if (!PuedeVer(i))
        {
            Console.WriteLine("Este expediente no está dirigido a usted.");
            return;
        }

        Console.WriteLine("Solicitante: " + Datos.Solicitantes[i] + " (" + Datos.Parentescos[i] + " del alumno)");
        Console.WriteLine("DNI: " + Datos.Dnis[i]);
        Console.WriteLine("Teléfono: " + Datos.Telefonos[i]);
        Console.WriteLine("Trámite: " + Datos.Tipos[i]);
        if (Datos.Tipos[i] == "justificacion")
            Console.WriteLine("Motivo: " + Datos.Motivos[i]);
        Console.WriteLine("Recibido por: " + Datos.RecibidoPor[i]);
        Console.WriteLine("Fecha de registro: " + Datos.Fechas[i]);
        Console.WriteLine("Dirigido a: " + Usuarios.EtiquetaDeCuenta(Datos.Destinos[i]));
        Console.WriteLine("Estado: " + Datos.Estados[i]);
        Console.WriteLine("Último movimiento: " + Datos.Movimientos[i]);
    }

    // Solo se puede derivar un trámite que esté dirigido al usuario actual.
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

        if (Datos.Destinos[i] != Usuarios.Cuentas[Usuarios.Actual])
        {
            Console.WriteLine("Solo puede derivar trámites dirigidos a usted.");
            return;
        }

        Console.WriteLine("Dirigido actualmente a: " + Usuarios.EtiquetaDeCuenta(Datos.Destinos[i]));
        Console.WriteLine();
        Console.WriteLine("¿A quién desea derivarlo?");

        int posDestino;
        do
        {
            posDestino = Usuarios.ElegirDestinatario("Derivar a: ");
            if (posDestino == Usuarios.Actual)
                Console.WriteLine("Error: el trámite ya está dirigido a usted.");
        } while (posDestino == Usuarios.Actual);

        Datos.Destinos[i] = Usuarios.Cuentas[posDestino];
        Datos.Estados[i] = "Derivado";
        Datos.Movimientos[i] = Validaciones.FechaActual();

        Archivos.Guardar();
        Console.WriteLine("Trámite derivado a: " + Usuarios.Etiqueta(posDestino));
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
                    Intercambiar(Datos.Telefonos, j);
                    Intercambiar(Datos.Parentescos, j);
                    Intercambiar(Datos.Motivos, j);
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