using System.Text;
using Planazo.Presentacion;
using Planazo.Servicios;

// Permite mostrar tildes, ñ y símbolos correctamente en la consola.
Console.OutputEncoding = Encoding.UTF8;

var gestor = new GestorReservas(DatosIniciales.CrearEventos());
var menu = new MenuPrincipal(gestor);
menu.Ejecutar();
