using System.Data;
using Parqueo.Datos;
using Parqueo.Entidades;
namespace Parqueo.Negocio;
public record ResumenDiario(DateTime Fecha,int IngresadosHoy,int AtendidosHoy,int VehiculosDentro,decimal IngresosHoy);
public static class Consultas
{
    public static DataTable EmpresaTabla(){using var db=new ParqueoContext();return Tablas.Crear(db.Empresa.ToList());}
    public static Dictionary<string,string> Textos(){using var db=new ParqueoContext();return db.TextosInterfaz.ToDictionary(x=>x.Clave,x=>x.Texto);}
    public static bool SinUsuarios(){using var db=new ParqueoContext();return !db.Usuarios.Any();}
    public static DataTable Opciones(string tabla,bool vigentes=false)
    {
        using var db=new ParqueoContext();var datos=Catalogos.Objetos(db,tabla);var resultado=new DataTable();resultado.Columns.Add("Id",typeof(int));resultado.Columns.Add("Nombre");
        foreach(var o in datos){var t=o.GetType();if(vigentes){var inicio=(DateTime)t.GetProperty("Inicio")!.GetValue(o)!;var fin=(DateTime)t.GetProperty("Fin")!.GetValue(o)!;var activo=(bool)t.GetProperty("Activo")!.GetValue(o)!;if(!ParqueoServicio.Vigente(inicio,fin,activo,DateTime.Now))continue;}resultado.Rows.Add(t.GetProperty("Id")!.GetValue(o),t.GetProperty(tabla=="Espacios"?"Codigo":"Nombre")!.GetValue(o));}return resultado;
    }
    public static DataTable Activos(string busqueda="",DateTime? momento=null)
    {
        Seguridad.Exigir("Operación");using var db=new ParqueoContext();DateTime ahora=momento??DateTime.Now;string periodo=ahora.ToString("yyyy-MM");
        var cuotasPagadas=db.Mensualidades.Where(m=>m.Periodo==periodo).Select(m=>m.ConvenioId).ToHashSet();
        var vehiculos=(from t in db.Tickets join e in db.Espacios on t.EspacioId equals e.Id join tarifa in db.Tarifas on t.TarifaId equals tarifa.Id where t.Salida==null orderby t.Entrada select new{Ticket=t,Espacio=e.Codigo,Tarifa=tarifa.Nombre}).ToList();
        var tabla=Tablas.Crear(vehiculos.Select(v=>new{v.Ticket.Id,v.Ticket.Placa,v.Espacio,v.Ticket.Entrada,Tiempo=ParqueoServicio.Tiempo(v.Ticket.Entrada,ahora),Estimado=ParqueoServicio.TotalEstimado(v.Ticket,ahora,v.Ticket.ConvenioId.HasValue && cuotasPagadas.Contains(v.Ticket.ConvenioId.Value)),v.Tarifa}));
        return BuscarActivos(tabla,busqueda);
    }
    // Filtrar en memoria permite buscar mientras se escribe sin consultar SQL en cada tecla.
    public static DataTable BuscarActivos(DataTable tabla,string busqueda)
    {
        string texto=busqueda.Trim().ToUpperInvariant();bool soloTicket=texto.StartsWith('#');string numero=texto.TrimStart('#');bool esNumero=int.TryParse(numero,out int ticket);
        var resultado=tabla.Clone();
        foreach(DataRow fila in tabla.Rows){bool coincide=texto.Length==0 || (esNumero && (int)fila["Id"]==ticket) || (!soloTicket && ((string)fila["Placa"]).Contains(texto,StringComparison.OrdinalIgnoreCase));if(coincide)resultado.ImportRow(fila);}
        return resultado;
    }
    public static DataTable FiltrarEspacios(DataTable tabla,int zona,int tipo,string estado)
    {
        var resultado=tabla.Clone();foreach(DataRow fila in tabla.Rows)
            if((zona==0 || (int)fila["ZonaId"]==zona) && (tipo==0 || (int)fila["TipoEspacioId"]==tipo) && (string.IsNullOrWhiteSpace(estado) || (string)fila["Estado"]==estado))resultado.ImportRow(fila);
        return resultado;
    }
    public static DataTable Historial(string placa)
    {
        Seguridad.Exigir("Operación");string busqueda=placa.Trim().ToUpperInvariant();if(busqueda.Length<3 || busqueda.Length>15)throw new Exception("Escriba la placa completa (entre 3 y 15 caracteres).");
        using var db=new ParqueoContext();DateTime ahora=DateTime.Now;
        var visitas=(from t in db.Tickets join e in db.Espacios on t.EspacioId equals e.Id where t.Placa==busqueda orderby t.Entrada descending,t.Id descending select new{Ticket=t,Espacio=e.Codigo}).ToList();
        return Tablas.Crear(visitas.Select(v=>new{v.Ticket.Id,v.Ticket.Placa,v.Espacio,v.Ticket.Entrada,v.Ticket.Salida,Tiempo=ParqueoServicio.Tiempo(v.Ticket.Entrada,v.Ticket.Salida??ahora),Estado=v.Ticket.Salida==null?"Dentro":"Finalizado",Total=v.Ticket.Total}));
    }
    public static ResumenDiario ResumenDia(DateTime? momento=null)
    {
        Seguridad.Exigir("Reportes");DateTime ahora=momento??DateTime.Now;DateTime inicio=ahora.Date;
        using var db=new ParqueoContext();
        var tickets=db.Tickets.Where(t=>t.Entrada<=ahora && (t.Salida==null || t.Salida>=inicio)).Select(t=>new{t.Entrada,t.Salida,t.Total}).ToList();
        var cobrados=tickets.Where(t=>t.Salida!=null && t.Salida<=ahora).ToList();
        return new ResumenDiario(inicio,tickets.Count(t=>t.Entrada>=inicio),cobrados.Count,tickets.Count(t=>t.Salida==null || t.Salida>ahora),cobrados.Sum(t=>t.Total??0));
    }
    public static DataTable Bitacora(DateTime desde,DateTime hasta,string usuario,string accion){Seguridad.Exigir("Bitácora");using var db=new ParqueoContext();return Tablas.Crear(db.Bitacora.Where(b=>b.Fecha>=desde && b.Fecha<hasta && b.Usuario.Contains(usuario) && (accion=="Todas" || b.Accion==accion)).OrderByDescending(b=>b.Id).ToList());}
    public static DataTable Reporte(int opcion,DateTime desde,DateTime hasta,int tipo)
    {
        Seguridad.Exigir("Reportes");if(opcion==1)return ParqueoServicio.ResumenOcupacion(tipo);using var db=new ParqueoContext();
        var query=from t in db.Tickets join e in db.Espacios on t.EspacioId equals e.Id join te in db.TiposEspacio on e.TipoEspacioId equals te.Id where tipo==0 || te.Id==tipo select new{t.Id,t.Placa,e.Codigo,Tipo=te.Nombre,t.Entrada,t.Salida,t.Subtotal,t.Total};
        if(opcion==2)return Tablas.Crear(query.Where(t=>t.Entrada<hasta && (t.Salida==null || t.Salida>=desde)).OrderBy(t=>t.Entrada).Select(t=>new{t.Id,t.Placa,t.Codigo,t.Tipo,t.Entrada,t.Salida}).ToList());
        var cobrados=query.Where(t=>t.Salida>=desde && t.Salida<hasta).ToList();if(opcion==0)return Tablas.Crear(cobrados.GroupBy(t=>t.Tipo).Select(g=>new{Tipo=g.Key,Vehiculos=g.Count(),Subtotal=g.Sum(t=>t.Subtotal),Impuesto=g.Sum(t=>t.Total-t.Subtotal),Ingresos=g.Sum(t=>t.Total)}));
        return Tablas.Crear(cobrados.OrderByDescending(t=>t.Salida).Select(t=>new{t.Id,t.Placa,t.Codigo,t.Tipo,t.Entrada,t.Salida,t.Total}));
    }
}
