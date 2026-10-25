using Planazo.Excepciones;
using Planazo.Modelos;

namespace Planazo.Servicios;

/// <summary>
/// Gestor central: guarda los eventos, usuarios y reservas (composición) y concentra
/// las reglas de negocio. La interfaz de consola solo le delega el trabajo.
/// </summary>
public class GestorReservas
{
    private readonly List<Evento> _eventos = new();
    private readonly List<Reserva> _reservas = new();
    private readonly List<Usuario> _usuarios = new();
    private int _siguienteIdReserva = 1;
    private int _siguienteIdUsuario = 1;

    public GestorReservas(IEnumerable<Evento> eventosIniciales)
    {
        ArgumentNullException.ThrowIfNull(eventosIniciales);
        foreach (var evento in eventosIniciales)
            RegistrarEvento(evento);
    }

    // Se exponen como solo lectura para que nadie modifique las listas desde fuera.
    public IReadOnlyList<Evento> Eventos => _eventos;
    public IReadOnlyList<Reserva> Reservas => _reservas;

    // ---------- Eventos ----------

    public void RegistrarEvento(Evento evento)
    {
        ArgumentNullException.ThrowIfNull(evento);
        if (_eventos.Any(e => e.Id == evento.Id))
            throw new ArgumentException($"Ya existe un evento con el identificador {evento.Id}.");

        _eventos.Add(evento);
    }

    public Evento ObtenerEvento(int id)
    {
        return _eventos.FirstOrDefault(e => e.Id == id)
               ?? throw new EventoNoEncontradoException(id);
    }

    // ---------- Usuarios (Reto 2) ----------

    public Usuario? BuscarUsuario(string documento)
    {
        return _usuarios.FirstOrDefault(u => u.Documento == documento.Trim());
    }

    public Usuario RegistrarUsuario(string documento, string nombre)
    {
        var usuario = new Usuario(_siguienteIdUsuario, documento, nombre);
        if (BuscarUsuario(usuario.Documento) != null)
            throw new ArgumentException($"Ya existe un usuario con el documento {usuario.Documento}.");

        _usuarios.Add(usuario);
        _siguienteIdUsuario++;
        return usuario;
    }

    public IReadOnlyList<Reserva> ObtenerReservasDeUsuario(string documento)
    {
        return _reservas.Where(r => r.Usuario.Documento == documento.Trim()).ToList();
    }

    // ---------- Reservas ----------

    public Reserva Reservar(int idEvento, Usuario usuario, int cantidad)
    {
        var evento = ObtenerEvento(idEvento);

        // Primero se construye la reserva (valida datos) y luego se descuentan los cupos
        // (valida disponibilidad). Si algo falla, no se guarda nada ni se consume un ID.
        var reserva = new Reserva(_siguienteIdReserva, usuario, evento, cantidad);
        evento.ReservarCupos(cantidad);

        _reservas.Add(reserva);
        _siguienteIdReserva++;
        return reserva;
    }

    public Reserva ObtenerReserva(int id)
    {
        return _reservas.FirstOrDefault(r => r.Id == id)
               ?? throw new ReservaNoEncontradaException(id);
    }

    public Reserva CancelarReserva(int id)
    {
        var reserva = ObtenerReserva(id);
        reserva.Cancelar();
        return reserva;
    }

    public ResumenReservas ObtenerResumen()
    {
        var activas = _reservas.Where(r => r.EstaActiva).ToList();
        return new ResumenReservas(
            activas.Count,
            activas.Sum(r => r.Cantidad),
            activas.Sum(r => r.ValorTotal));
    }
}
