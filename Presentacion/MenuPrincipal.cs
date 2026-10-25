using Planazo.Excepciones;
using Planazo.Modelos;
using Planazo.Servicios;

namespace Planazo.Presentacion;

/// <summary>Menu interactivo de consola. No contiene reglas de negocio: se las delega al gestor.</summary>
public class MenuPrincipal
{
    private readonly GestorReservas _gestor;

    public MenuPrincipal(GestorReservas gestor)
    {
        _gestor = gestor ?? throw new ArgumentNullException(nameof(gestor));
    }

    public void Ejecutar()
    {
        bool salir = false;

        while (!salir)
        {
            MostrarMenu();

            try
            {
                string opcion = Consola.LeerTexto("Seleccione una opción: ").Trim();
                Console.WriteLine();
                salir = ProcesarOpcion(opcion);
            }
            catch (EndOfStreamException)
            {
                salir = true; // la entrada se cerró: se sale sin error
            }
            catch (PlanazoException ex)
            {
                Consola.Error(ex.Message);
            }
            catch (ArgumentException ex)
            {
                Consola.Error(ex.Message);
            }
            catch (Exception ex)
            {
                // Último recurso: ningún error debe cerrar el programa de forma inesperada.
                Consola.Error($"Ocurrió un error inesperado: {ex.Message}");
            }
        }

        Consola.Info("Gracias por usar PLANAZO. ¡Hasta pronto!");
    }

    private static void MostrarMenu()
    {
        Console.WriteLine();
        Console.WriteLine("================================");
        Console.WriteLine("        PLANAZO - EVENTOS");
        Console.WriteLine("================================");
        Console.WriteLine();
        Console.WriteLine("1. Ver eventos disponibles");
        Console.WriteLine("2. Consultar evento");
        Console.WriteLine("3. Reservar entradas");
        Console.WriteLine("4. Ver reservas realizadas");
        Console.WriteLine("5. Cancelar una reserva");
        Console.WriteLine("6. Ver resumen de reservas");
        Console.WriteLine("7. Ver reservas de un usuario (Reto 2)");
        Console.WriteLine("8. Ejecutar casos de prueba demostrativos");
        Console.WriteLine("0. Salir");
        Console.WriteLine();
    }

    /// <returns>true si el usuario eligió salir.</returns>
    private bool ProcesarOpcion(string opcion)
    {
        switch (opcion)
        {
            case "1": VerEventos(); break;
            case "2": ConsultarEvento(); break;
            case "3": ReservarEntradas(); break;
            case "4": VerReservas(); break;
            case "5": CancelarReserva(); break;
            case "6": VerResumen(); break;
            case "7": VerReservasDeUsuario(); break;
            case "8": PruebasDemostrativas.Ejecutar(); break;
            case "0": return true;
            default: Consola.Error("Opción no válida. Elige un número del menú."); break;
        }

        return false;
    }

    // ---------- Opciones ----------

    private void VerEventos()
    {
        Consola.Info("EVENTOS DISPONIBLES");
        Consola.Separador();
        Console.WriteLine($"{"ID",-3} {"Tipo",-10} {"Nombre",-32} {"Fecha",-16} {"P. final",10} {"Cupos",9}");
        Consola.Separador();

        foreach (var evento in _gestor.Eventos)
        {
            Console.WriteLine(
                $"{evento.Id,-3} {evento.Tipo,-10} {Cortar(evento.Nombre, 32),-32} {Formato.Fecha(evento.Fecha),-16} " +
                $"{Formato.Moneda(evento.CalcularPrecioFinal()),10} {evento.CuposDisponibles + "/" + evento.CapacidadMaxima,9}");
        }
    }

    private void ConsultarEvento()
    {
        int id = Consola.LeerEntero("Identificador del evento: ");
        var evento = _gestor.ObtenerEvento(id);

        Consola.Info("DETALLE DEL EVENTO");
        Consola.Separador();
        Console.WriteLine($"ID:                {evento.Id}");
        Console.WriteLine($"Tipo:              {evento.Tipo}");
        Console.WriteLine($"Nombre:            {evento.Nombre}");
        Console.WriteLine($"Fecha:             {Formato.Fecha(evento.Fecha)}");
        Console.WriteLine($"Precio base:       {Formato.Moneda(evento.PrecioBase)}");
        Console.WriteLine($"Precio final:      {Formato.Moneda(evento.CalcularPrecioFinal())}");
        Console.WriteLine($"Cupos disponibles: {evento.CuposDisponibles} de {evento.CapacidadMaxima}");
    }

    private void ReservarEntradas()
    {
        VerEventos();
        Console.WriteLine();

        int idEvento = Consola.LeerEntero("Identificador del evento a reservar: ");
        _gestor.ObtenerEvento(idEvento); // falla pronto si el evento no existe

        string documento = Consola.LeerTexto("Documento de la persona: ");
        var usuario = _gestor.BuscarUsuario(documento);
        if (usuario == null)
        {
            string nombre = Consola.LeerTexto("Nombre de la persona: ");
            usuario = _gestor.RegistrarUsuario(documento, nombre);
        }
        else
        {
            Consola.Info($"Usuario encontrado: {usuario.Nombre}");
        }

        int cantidad = Consola.LeerEntero("Cantidad de entradas: ");
        var reserva = _gestor.Reservar(idEvento, usuario, cantidad);

        Consola.Exito("✔ Reserva confirmada.");
        MostrarReserva(reserva);
    }

    private void VerReservas()
    {
        Consola.Info("RESERVAS REALIZADAS");
        MostrarListaReservas(_gestor.Reservas);
    }

    private void CancelarReserva()
    {
        int id = Consola.LeerEntero("Identificador de la reserva a cancelar: ");
        var reserva = _gestor.CancelarReserva(id);

        Consola.Exito($"✔ Reserva #{reserva.Id} cancelada. Se devolvieron {reserva.Cantidad} cupo(s) a \"{reserva.Evento.Nombre}\".");
        Console.WriteLine($"Cupos disponibles ahora: {reserva.Evento.CuposDisponibles} de {reserva.Evento.CapacidadMaxima}");
    }

    private void VerResumen()
    {
        var resumen = _gestor.ObtenerResumen();

        Consola.Info("RESUMEN DE RESERVAS ACTIVAS");
        Consola.Separador();
        Console.WriteLine($"Reservas activas:    {resumen.ReservasActivas}");
        Console.WriteLine($"Entradas reservadas: {resumen.EntradasReservadas}");
        Console.WriteLine($"Valor total:         {Formato.Moneda(resumen.ValorTotal)}");
    }

    private void VerReservasDeUsuario()
    {
        string documento = Consola.LeerTexto("Documento del usuario: ");
        var usuario = _gestor.BuscarUsuario(documento);

        if (usuario == null)
        {
            Consola.Error("No hay ningún usuario registrado con ese documento.");
            return;
        }

        Consola.Info($"RESERVAS DE {usuario.Nombre.ToUpperInvariant()} (doc. {usuario.Documento})");
        MostrarListaReservas(_gestor.ObtenerReservasDeUsuario(documento));
    }

    // ---------- Auxiliares de impresión ----------

    private static void MostrarListaReservas(IReadOnlyList<Reserva> reservas)
    {
        Consola.Separador();
        if (reservas.Count == 0)
        {
            Console.WriteLine("No hay reservas para mostrar.");
            return;
        }

        Console.WriteLine($"{"ID",-3} {"Persona",-18} {"Evento",-28} {"Cant.",5} {"Total",12} {"Estado",-10}");
        Consola.Separador();
        foreach (var r in reservas)
        {
            Console.WriteLine(
                $"{r.Id,-3} {Cortar(r.NombrePersona, 18),-18} {Cortar(r.Evento.Nombre, 28),-28} " +
                $"{r.Cantidad,5} {Formato.Moneda(r.ValorTotal),12} {r.Estado,-10}");
        }
    }

    internal static void MostrarReserva(Reserva r)
    {
        Console.WriteLine($"  Reserva #:   {r.Id}");
        Console.WriteLine($"  Persona:     {r.NombrePersona}");
        Console.WriteLine($"  Evento:      {r.Evento.Nombre} ({r.Evento.Tipo})");
        Console.WriteLine($"  Entradas:    {r.Cantidad} x {Formato.Moneda(r.Evento.CalcularPrecioFinal())}");
        Console.WriteLine($"  Valor total: {Formato.Moneda(r.ValorTotal)}");
        Console.WriteLine($"  Estado:      {r.Estado}");
    }

    private static string Cortar(string texto, int maximo) =>
        texto.Length <= maximo ? texto : texto[..(maximo - 1)] + "…";
}
