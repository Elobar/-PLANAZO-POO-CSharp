namespace Planazo.Excepciones;

/// <summary>Se lanza cuando se piden más entradas que los cupos disponibles.</summary>
public class CuposInsuficientesException : PlanazoException
{
    public CuposInsuficientesException(string nombreEvento, int solicitados, int disponibles)
        : base($"No hay cupos suficientes en \"{nombreEvento}\": se pidieron {solicitados} y solo hay {disponibles} disponibles.")
    {
    }
}
