using System.Globalization;

namespace Planazo.Presentacion;

/// <summary>Formatos de salida (no depende de la cultura instalada en el equipo).</summary>
public static class Formato
{
    private static readonly NumberFormatInfo Numeros = new()
    {
        NumberGroupSeparator = ".",
        NumberDecimalSeparator = ","
    };

    public static string Moneda(decimal valor) => "$" + valor.ToString("N0", Numeros);

    public static string Fecha(DateTime fecha) => fecha.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
}
