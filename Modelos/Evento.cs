using Planazo.Excepciones;

namespace Planazo.Modelos;

/// <summary>
/// Clase abstracta que reúne lo común a cualquier evento de PLANAZO.
/// Cada tipo de evento define su propia regla de precio en <see cref="CalcularPrecioFinal"/>.
/// </summary>
public abstract class Evento
{
    // Los datos solo se asignan en el constructor (get-only) y los cupos
    // únicamente cambian a través de ReservarCupos / LiberarCupos.
    public int Id { get; }
    public string Nombre { get; }
    public DateTime Fecha { get; }
    public decimal PrecioBase { get; }
    public int CapacidadMaxima { get; }
    public int CuposDisponibles { get; private set; }

    /// <summary>Nombre del tipo de evento (Cine, Concierto, Taller...).</summary>
    public abstract string Tipo { get; }

    protected Evento(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima)
    {
        if (id <= 0)
            throw new ArgumentException("El identificador del evento debe ser mayor que cero.");
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del evento no puede estar vacío.");
        if (precioBase <= 0)
            throw new ArgumentException("El precio base debe ser mayor que cero.");
        if (capacidadMaxima <= 0)
            throw new ArgumentException("La capacidad máxima debe ser mayor que cero.");

        Id = id;
        Nombre = nombre.Trim();
        Fecha = fecha;
        PrecioBase = precioBase;
        CapacidadMaxima = capacidadMaxima;
        CuposDisponibles = capacidadMaxima; // al crearse, todos los cupos están libres
    }

    /// <summary>Calcula el precio de una entrada según la regla de cada tipo de evento.</summary>
    public abstract decimal CalcularPrecioFinal();

    /// <summary>Descuenta cupos. Valida la cantidad y la disponibilidad antes de modificar nada.</summary>
    public void ReservarCupos(int cantidad)
    {
        if (cantidad <= 0)
            throw new CantidadInvalidaException(cantidad);
        if (cantidad > CuposDisponibles)
            throw new CuposInsuficientesException(Nombre, cantidad, CuposDisponibles);

        CuposDisponibles -= cantidad;
    }

    /// <summary>Devuelve cupos al cancelar una reserva, sin superar nunca la capacidad máxima.</summary>
    public void LiberarCupos(int cantidad)
    {
        if (cantidad <= 0)
            throw new CantidadInvalidaException(cantidad);
        if (CuposDisponibles + cantidad > CapacidadMaxima)
            throw new InvalidOperationException("No se pueden liberar más cupos que la capacidad máxima del evento.");

        CuposDisponibles += cantidad;
    }
}
