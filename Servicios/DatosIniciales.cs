using Planazo.Modelos;

namespace Planazo.Servicios;

/// <summary>Eventos default con  los que arranca el programa (2 de cine, 2 conciertos, 2 talleres y 1 festival).</summary>
public static class DatosIniciales
{
    public static List<Evento> CrearEventos()
    {
        return new List<Evento>
        {
            new Cine(1, "Buscando a Edinso", new DateTime(2026, 11, 7, 19, 0, 0), 20_000m, 40),
            new Cine(2, "Star Wars: El despertar de Edinso", new DateTime(2026, 11, 8, 20, 0, 0), 18_000m, 30),
            new Concierto(3, "Noche de Salsa en Barranquilla", new DateTime(2026, 11, 14, 20, 0, 0), 50_000m, 100),
            new Concierto(4, "Concierto Sinfónico de Edinso", new DateTime(2026, 12, 12, 19, 30, 0), 60_000m, 80),
            new Taller(5, "Taller de Fotografía de Edinso", new DateTime(2026, 11, 21, 9, 0, 0), 80_000m, 20),
            new Taller(6, "Taller de Cocina con Edinso", new DateTime(2026, 11, 28, 10, 0, 0), 100_000m, 15),
            new Festival(7, "Festival del edinso", new DateTime(2026, 12, 5, 16, 0, 0), 100_000m, 200)
        };
    }
}
