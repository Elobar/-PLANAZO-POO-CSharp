namespace Planazo.Modelos;

/// <summary>
/// Reto 1: nuevo tipo de evento. Regla propia: recargo del 20 % sobre el precio base
/// más un cargo fijo de $10.000 por servicios del festival (zonas de comida, seguridad, etc.).
/// Se integra al sistema sin modificar Cine, Concierto, Taller ni el gestor.
/// </summary>
public class Festival : Evento
{
    private const decimal Recargo = 0.20m;
    private const decimal CargoServicios = 10_000m;

    public Festival(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima)
        : base(id, nombre, fecha, precioBase, capacidadMaxima)
    {
    }

    public override string Tipo => "Festival";

    public override decimal CalcularPrecioFinal() => PrecioBase * (1 + Recargo) + CargoServicios;
}
