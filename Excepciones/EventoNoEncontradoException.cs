namespace Planazo.Excepciones;

/// <summary>Se lanza cuando se consulta un evento que no existe.</summary>
public class EventoNoEncontradoException : PlanazoException
{
    public EventoNoEncontradoException(int id)
        : base($"No existe ningún evento con el identificador {id}.")
    {
    }
}
