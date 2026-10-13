using MesaDePartes;

// Algoritmo principal: carga los datos guardados y repite el menú hasta elegir salir.
Archivos.Cargar();
Pausa();

int opcion;
do
{
    MostrarMenuPrincipal();
    opcion = Validaciones.LeerEntero("");
    Console.WriteLine();

    switch (opcion)
    {
        case 1:
            Tramites.Registrar();
            Pausa();
            break;
        case 2:
            if (Usuarios.IniciarSesion())
            {
                Pausa();
                MenuUsuario();
            }
            else
            {
                Pausa();
            }
            break;
        case 3:
            Console.WriteLine("Gracias por usar el sistema.");
            break;
        default:
            Console.WriteLine("Opción incorrecta.");
            Pausa();
            break;
    }
} while (opcion != 3);

// Menú que ve cada usuario después de iniciar sesión.
static void MenuUsuario()
{
    int opcion;
    do
    {
        Console.Clear();
        Console.WriteLine("Sesión: " + Usuarios.Etiqueta(Usuarios.Actual));
        Console.WriteLine();
        Console.WriteLine("[1] Mis trámites");
        Console.WriteLine("[2] Buscar expediente");
        Console.WriteLine("[3] Derivar trámite");
        Console.WriteLine("[4] Ordenar por expediente (más recientes primero)");
        if (Usuarios.EsDirectivo())
            Console.WriteLine("[5] Ver todos los trámites");
        Console.WriteLine("[6] Cerrar sesión");
        Console.Write("Ingresar opción: ");

        opcion = Validaciones.LeerEntero("");
        Console.WriteLine();

        switch (opcion)
        {
            case 1:
                Tramites.Mostrar(false);
                break;
            case 2:
                Tramites.Buscar();
                break;
            case 3:
                Tramites.Derivar();
                break;
            case 4:
                Tramites.Ordenar();
                Tramites.Mostrar(false);
                break;
            case 5:
                if (Usuarios.EsDirectivo())
                    Tramites.Mostrar(true);
                else
                    Console.WriteLine("Opción incorrecta.");
                break;
            case 6:
                Usuarios.CerrarSesion();
                Console.WriteLine("Sesión cerrada.");
                break;
            default:
                Console.WriteLine("Opción incorrecta.");
                break;
        }

        Pausa();
    } while (opcion != 6);
}

static void MostrarMenuPrincipal()
{
    Console.Clear();
    Console.WriteLine("***** MESA DE PARTES DIGITAL *****");
    Console.WriteLine("I.E. Darío Arrús Cuestas N.° 5011");
    Console.WriteLine();
    Console.WriteLine("[1] Registrar trámite (mesa de partes)");
    Console.WriteLine("[2] Iniciar sesión (director, coordinador, profesores)");
    Console.WriteLine("[3] Salir");
    Console.Write("Ingresar opción: ");
}

static void Pausa()
{
    Console.WriteLine();
    Console.WriteLine("Presione Enter para continuar...");
    Console.ReadLine();
}