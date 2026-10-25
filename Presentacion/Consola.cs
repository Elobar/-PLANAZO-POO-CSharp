namespace Planazo.Presentacion;

/// <summary>Utilidades de entrada y salida por consola.</summary>
public static class Consola
{
    public static void Exito(string mensaje) => Escribir(mensaje, ConsoleColor.Green);

    public static void Error(string mensaje) => Escribir("✘ " + mensaje, ConsoleColor.Red);

    public static void Info(string mensaje) => Escribir(mensaje, ConsoleColor.Cyan);

    public static void Separador() => Console.WriteLine(new string('-', 80));

    public static string LeerTexto(string mensaje)
    {
        Console.Write(mensaje);
        // Si la entrada se cierra (Ctrl+D / Ctrl+Z) se corta con una excepción controlada.
        return Console.ReadLine() ?? throw new EndOfStreamException();
    }

    /// <summary>Lee un entero y vuelve a preguntar mientras el usuario escriba algo que no lo sea.</summary>
    public static int LeerEntero(string mensaje)
    {
        while (true)
        {
            string texto = LeerTexto(mensaje).Trim();
            if (int.TryParse(texto, out int valor))
                return valor;

            Error("Entrada no válida: escribe un número entero.");
        }
    }

    private static void Escribir(string mensaje, ConsoleColor color)
    {
        var anterior = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(mensaje);
        Console.ForegroundColor = anterior;
    }
}
