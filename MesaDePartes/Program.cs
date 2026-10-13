using MesaDePartes;

// Algoritmo principal: carga los datos guardados y repite el menú hasta elegir salir.
Archivos.Cargar();
Console.WriteLine("Presione Enter para continuar...");
Console.ReadLine();

int opcion;
do
{
    MostrarMenu();
    opcion = Validaciones.LeerEntero("");
    Console.WriteLine();

    switch (opcion)
    {
        case 1:
            Tramites.Registrar();
            break;
        case 2:
            Tramites.Mostrar();
            break;
        case 3:
            Tramites.Buscar();
            break;
        case 4:
            Tramites.Derivar();
            break;
        case 5:
            Tramites.Ordenar();
            Tramites.Mostrar();
            break;
        case 6:
            Console.WriteLine("Gracias por usar el sistema.");
            break;
        default:
            Console.WriteLine("Opción incorrecta.");
            break;
    }

    if (opcion != 6)
    {
        Console.WriteLine();
        Console.WriteLine("Presione Enter para continuar...");
        Console.ReadLine();
    }
} while (opcion != 6);

static void MostrarMenu()
{
    Console.Clear();
    Console.WriteLine("***** MESA DE PARTES DIGITAL *****");
    Console.WriteLine("I.E. Darío Arrús Cuestas N.° 5011");
    Console.WriteLine();
    Console.WriteLine("[1] Registrar trámite");
    Console.WriteLine("[2] Mostrar trámites");
    Console.WriteLine("[3] Buscar expediente");
    Console.WriteLine("[4] Derivar trámite");
    Console.WriteLine("[5] Ordenar por expediente (más recientes primero)");
    Console.WriteLine("[6] Salir");
    Console.Write("Ingresar opción: ");
}
