namespace Planazo.Modelos;

/// <summary>Reto 2: persona que realiza reservas. Se identifica por su documento.</summary>
public class Usuario
{
    public int Id { get; }
    public string Documento { get; }
    public string Nombre { get; }

    public Usuario(int id, string documento, string nombre)
    {
        if (id <= 0)
            throw new ArgumentException("El identificador del usuario debe ser mayor que cero.");
        if (string.IsNullOrWhiteSpace(documento))
            throw new ArgumentException("El documento del usuario no puede estar vacío.");
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del usuario no puede estar vacío.");

        Id = id;
        Documento = documento.Trim();
        Nombre = nombre.Trim();
    }
}
