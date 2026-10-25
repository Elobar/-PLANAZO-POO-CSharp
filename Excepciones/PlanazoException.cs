namespace Planazo.Excepciones;

/// <summary>
/// Excepción base de todas las reglas de negocio de PLANAZO.
/// Permite capturar cualquier error de dominio con un solo catch.
/// </summary>
public class PlanazoException : Exception
{
    public PlanazoException(string mensaje) : base(mensaje)
    {
    }
}
