using Planazo.Excepciones;
using Planazo.Modelos;
using Planazo.Servicios;

namespace Planazo.Presentacion;

/// <summary>
/// Ejecuta los 7 casos de prueba del enunciado sobre un gestor nuevo (con datos iniciales),
/// así no altera las reservas que el usuario haya hecho en el menú. Ideal para tomar capturas.
/// </summary>
public static class PruebasDemostrativas
{
    public static void Ejecutar()
    {
        var gestor = new GestorReservas(DatosIniciales.CrearEventos());
        var usuario = gestor.RegistrarUsuario("1001", "Ana Pérez");
        Reserva? reservaCaso1 = null;

        Consola.Info("CASOS DE PRUEBA DEMOSTRATIVOS (sobre un gestor nuevo)");

        Caso(1, "Registrar una reserva válida", () =>
        {
            var evento = gestor.ObtenerEvento(3);
            Console.WriteLine($"Cupos antes: {evento.CuposDisponibles}");
            reservaCaso1 = gestor.Reservar(3, usuario, 2);
            MenuPrincipal.MostrarReserva(reservaCaso1);
            Console.WriteLine($"Cupos después: {evento.CuposDisponibles}");
            Consola.Exito("✔ Reserva registrada correctamente.");
        });

        Caso(2, "Reservar más entradas que los cupos disponibles", () =>
        {
            var evento = gestor.ObtenerEvento(6);
            Console.WriteLine($"Intentando reservar 20 entradas en \"{evento.Nombre}\" (cupos: {evento.CuposDisponibles}).");
            gestor.Reservar(6, usuario, 20);
        });

        Caso(3, "Reservar con una cantidad negativa", () =>
        {
            Console.WriteLine("Intentando reservar -3 entradas.");
            gestor.Reservar(1, usuario, -3);
        });

        Caso(4, "Cancelar una reserva y comprobar que se devuelven los cupos", () =>
        {
            var evento = reservaCaso1!.Evento;
            Console.WriteLine($"Cupos antes de cancelar: {evento.CuposDisponibles}");
            gestor.CancelarReserva(reservaCaso1.Id);
            Console.WriteLine($"Cupos después de cancelar: {evento.CuposDisponibles} (capacidad {evento.CapacidadMaxima})");
            Consola.Exito($"✔ Reserva #{reservaCaso1.Id} cancelada y cupos devueltos.");
        });

        Caso(5, "Cancelar dos veces la misma reserva", () =>
        {
            Console.WriteLine($"Intentando cancelar de nuevo la reserva #{reservaCaso1!.Id}.");
            gestor.CancelarReserva(reservaCaso1.Id);
        });

        Caso(6, "Consultar un evento inexistente", () =>
        {
            Console.WriteLine("Consultando el evento 99.");
            gestor.ObtenerEvento(99);
        });

        Caso(7, "Comprobar el precio final de cada tipo de evento", () =>
        {
            // (id del evento, precio final esperado calculado a mano)
            var esperados = new (int Id, decimal Esperado)[]
            {
                (1, 20_000m),   // Cine: mantiene el precio base
                (3, 57_500m),   // Concierto: 50.000 + 15 %
                (5, 72_000m),   // Taller: 80.000 - 10 %
                (7, 130_000m)   // Festival: 100.000 + 20 % + 10.000
            };

            foreach (var (id, esperado) in esperados)
            {
                var e = gestor.ObtenerEvento(id);
                decimal obtenido = e.CalcularPrecioFinal();
                string resultado = obtenido == esperado ? "OK" : "FALLÓ";
                Console.WriteLine(
                    $"{e.Tipo,-9} base {Formato.Moneda(e.PrecioBase),9} -> esperado {Formato.Moneda(esperado),9} | " +
                    $"obtenido {Formato.Moneda(obtenido),9} | {resultado}");
            }
        });

        Console.WriteLine();
        Consola.Info("Fin de los casos de prueba.");
    }

    private static void Caso(int numero, string titulo, Action accion)
    {
        Console.WriteLine();
        Consola.Separador();
        Console.WriteLine($"CASO {numero}: {titulo}");
        Consola.Separador();

        try
        {
            accion();
        }
        catch (PlanazoException ex)
        {
            // Error esperado: se informa y el programa continúa.
            Consola.Error($"Error controlado: {ex.Message}");
        }
    }
}
