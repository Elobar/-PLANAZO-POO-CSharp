namespace Planazo.Excepciones;

/// <summary>Se lanza cuando se intenta cancelar una reserva que ya estaba cancelada.</summary>
public class ReservaYaCanceladaException : PlanazoException
{
    public ReservaYaCanceladaException(int id)
        : base($"La reserva #{id} ya fue cancelada anteriormente; no se puede cancelar dos veces.")
    {
    }
}
