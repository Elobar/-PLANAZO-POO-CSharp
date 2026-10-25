using Planazo.Excepciones;

namespace Planazo.Modelos;

/// <summary>
/// Asocia a un <see cref="Usuario"/> con un <see cref="Evento"/>.
/// El valor total se calcula al crear la reserva con el precio final del evento (polimorfismo).
/// </summary>
public class Reserva
{
    public int Id { get; }
    public Usuario Usuario { get; }
    public Evento Evento { get; }
    public int Cantidad { get; }
    public decimal ValorTotal { get; }
    public EstadoReserva Estado { get; private set; }

    public string NombrePersona => Usuario.Nombre;
    public bool EstaActiva => Estado == EstadoReserva.Confirmada;

    public Reserva(int id, Usuario usuario, Evento evento, int cantidad)
    {
        if (id <= 0)
            throw new ArgumentException("El identificador de la reserva debe ser mayor que cero.");
        ArgumentNullException.ThrowIfNull(usuario);
        ArgumentNullException.ThrowIfNull(evento);
        if (cantidad <= 0)
            throw new CantidadInvalidaException(cantidad);

        Id = id;
        Usuario = usuario;
        Evento = evento;
        Cantidad = cantidad;
        ValorTotal = evento.CalcularPrecioFinal() * cantidad; // cada evento aplica su propia regla
        Estado = EstadoReserva.Confirmada;
    }

    /// <summary>Cancela la reserva y devuelve los cupos al evento. No se puede cancelar dos veces.</summary>
    public void Cancelar()
    {
        if (Estado == EstadoReserva.Cancelada)
            throw new ReservaYaCanceladaException(Id);

        Evento.LiberarCupos(Cantidad);
        Estado = EstadoReserva.Cancelada;
    }
}
