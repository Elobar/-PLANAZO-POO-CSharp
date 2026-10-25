namespace Planazo.Excepciones;

/// <summary>Se lanza cuando la cantidad de entradas es negativa o igual a cero.</summary>
public class CantidadInvalidaException : PlanazoException
{
    public CantidadInvalidaException(int cantidad)
        : base($"La cantidad de entradas debe ser mayor que cero (se recibió {cantidad}).")
    {
    }
}
