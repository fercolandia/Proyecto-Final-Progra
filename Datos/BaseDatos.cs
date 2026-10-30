namespace Parqueo.Datos;
// La conexión se conserva en un archivo para poder usar otra instancia de SQL Server.
public static class BaseDatos
{
    public static string Usuario { get; set; }="Instalación";
    public static string Cadena=>File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"conexion.txt")).Trim();
}
