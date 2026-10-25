namespace Planazo.Excepciones;

/// <summary>Se lanza cuando se busca una reserva que no existe.</summary>
public class ReservaNoEncontradaException : PlanazoException
{
    public ReservaNoEncontradaException(int id)
        : base($"No existe ninguna reserva con el identificador {id}.")
    {
    }
}
