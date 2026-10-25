namespace Planazo.Modelos;

/// <summary>Función de cine: el precio final es igual al precio base.</summary>
public class Cine : Evento
{
    public Cine(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima)
        : base(id, nombre, fecha, precioBase, capacidadMaxima)
    {
    }

    public override string Tipo => "Cine";

    public override decimal CalcularPrecioFinal() => PrecioBase;
}
