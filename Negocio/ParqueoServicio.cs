using System.Data;
using Microsoft.EntityFrameworkCore;
using Parqueo.Datos;
using Parqueo.Entidades;
namespace Parqueo.Negocio;
public record EstadoEspacio(int Id,string Codigo,string Zona,string Tipo,string Estado,string? Placa,DateTime? Entrada,int TipoEspacioId,int ZonaId);
public static class ParqueoServicio
{
    internal static bool Vigente(DateTime inicio,DateTime fin,bool activo,DateTime ahora)=>activo && inicio<=ahora && ahora<fin;
    internal static bool ReservaVigente(Reserva r,List<Convenio> convenios,List<Evento> eventos,DateTime ahora)
    {
        if(r.ConvenioId!=null){var c=convenios.Single(x=>x.Id==r.ConvenioId);return Vigente(c.Inicio,c.Fin,c.Activo,ahora);}
        var e=eventos.Single(x=>x.Id==r.EventoId);return Vigente(e.Inicio,e.Fin,e.Activo,ahora);
    }
    private static List<EstadoEspacio> Estados(ParqueoContext db)
    {
        var zonas=db.Zonas.ToList();var tipos=db.TiposEspacio.ToList();var espacios=db.Espacios.ToList();var tickets=db.Tickets.Where(t=>t.Salida==null).ToList();var reservas=db.Reservas.ToList();var convenios=db.Convenios.ToList();var eventos=db.Eventos.ToList();var resultado=new List<EstadoEspacio>();
        foreach(var e in espacios){var t=tickets.FirstOrDefault(t=>t.EspacioId==e.Id);bool reservado=reservas.Any(r=>r.EspacioId==e.Id && ReservaVigente(r,convenios,eventos,DateTime.Now));string estado=t!=null?"Ocupado":e.FueraServicio?"Fuera de servicio":reservado?"Reservado":"Disponible";resultado.Add(new(e.Id,e.Codigo,zonas.Single(z=>z.Id==e.ZonaId).Nombre,tipos.Single(x=>x.Id==e.TipoEspacioId).Nombre,estado,t?.Placa,t?.Entrada,e.TipoEspacioId,e.ZonaId));}
        return resultado.OrderBy(x=>x.Zona).ThenBy(x=>x.Codigo).ToList();
    }
    public static DataTable Disponibilidad(){Seguridad.Exigir("Operación");using var db=new ParqueoContext();return Tablas.Crear(Estados(db));}
    public static DataTable ResumenOcupacion(int tipo){Seguridad.Exigir("Reportes");using var db=new ParqueoContext();return Tablas.Crear(Estados(db).Where(e=>tipo==0 || e.TipoEspacioId==tipo).GroupBy(e=>new{e.Zona,e.Tipo,e.Estado}).Select(g=>new{g.Key.Zona,g.Key.Tipo,g.Key.Estado,Cantidad=g.Count()}));}
    public static int Ingresar(string placa,int vehiculo,int tipo,int? convenio,int? evento)
    {
        Seguridad.Exigir("Operación",true);placa=placa.Trim().ToUpperInvariant();if(placa.Length<3 || placa.Length>15)throw new Exception("La placa debe tener entre 3 y 15 caracteres.");if(convenio!=null && evento!=null)throw new Exception("Seleccione convenio o evento, no ambos.");
        if (!System.Text.RegularExpressions.Regex.IsMatch(placa, @"^[A-Z0-9]+$")) throw new Exception("La placa solo puede contener letras de la A a la Z y números del 0 al 9.");
        using var db=new ParqueoContext();using var transaccion=db.Database.BeginTransaction(IsolationLevel.Serializable);DateTime ahora=DateTime.Now;
        if(db.Tickets.Any(t=>t.Placa==placa && t.Salida==null))throw new Exception("El vehículo ya está dentro.");
        if(!db.TiposVehiculo.Any(v=>v.Id==vehiculo))throw new Exception("El tipo de vehículo no existe.");
        var nombreVehiculo = db.TiposVehiculo
        .Where(v => v.Id == vehiculo)
        .Select(v => v.Nombre)
        .Single();
        var nombreEspacio = db.TiposEspacio
        .Where(e => e.Id == tipo)
        .Select(e => e.Nombre)
        .Single();
        bool compatible =
            (nombreVehiculo == "Automóvil" &&
                (nombreEspacio == "Normal" || nombreEspacio == "Discapacitados" || nombreEspacio == "VIP")) ||
            (nombreVehiculo == "Camioneta" &&
                (nombreEspacio == "Normal" || nombreEspacio == "Discapacitados" || nombreEspacio == "VIP")) ||
            (nombreVehiculo == "Motocicleta" &&
                (nombreEspacio == "Motocicleta" || nombreEspacio == "VIP"));
        if (!compatible)
            throw new Exception("El espacio seleccionado no es compatible con el tipo de vehículo.");
        var convenios=db.Convenios.ToList();var eventos=db.Eventos.ToList();Tarifa? tarifa;
        if(convenio!=null){var c=convenios.Single(x=>x.Id==convenio);if(!Vigente(c.Inicio,c.Fin,c.Activo,ahora))throw new Exception("El convenio no está vigente.");tarifa=db.Tarifas.Find(c.TarifaId);}
        else if(evento!=null){var e=eventos.Single(x=>x.Id==evento);if(!Vigente(e.Inicio,e.Fin,e.Activo,ahora))throw new Exception("El evento no está vigente.");tarifa=db.Tarifas.Find(e.TarifaId);}
        else{var regulares=db.Tarifas.Where(t=>t.TipoEspacioId==tipo && t.Activa && t.Regular && !t.Mensual).ToList();if(regulares.Count!=1)throw new Exception("Configure una sola tarifa regular activa para ese tipo.");tarifa=regulares[0];}
        if(tarifa==null || !tarifa.Activa || tarifa.TipoEspacioId!=tipo || (tarifa.Mensual && convenio==null) || tarifa.MinutosUnidad<=0 || tarifa.Precio<0)throw new Exception("La tarifa no es válida para esta modalidad o tipo de espacio.");
        var reservas=db.Reservas.ToList();var ocupados=db.Tickets.Where(t=>t.Salida==null).Select(t=>t.EspacioId).ToList();Espacio? elegido=null;
        foreach(var e in db.Espacios.Where(e=>e.TipoEspacioId==tipo && !e.FueraServicio).OrderBy(e=>e.Codigo).ToList()){
            if(ocupados.Contains(e.Id))continue;var propias=reservas.Where(r=>r.EspacioId==e.Id).ToList();
            if(convenio!=null && !propias.Any(r=>r.ConvenioId==convenio))continue;if(evento!=null && !propias.Any(r=>r.EventoId==evento))continue;
            if(propias.Any(r=>ReservaVigente(r,convenios,eventos,ahora) && (r.ConvenioId!=null?r.ConvenioId!=convenio:r.EventoId!=evento)))continue;
            elegido=e;break;
        }
        if(elegido==null)throw new Exception("No hay espacios disponibles del tipo y modalidad elegidos.");var empresa=db.Empresa.Single(e=>e.Id==1);
        var ticket=new Ticket{Placa=placa,TipoVehiculoId=vehiculo,EspacioId=elegido.Id,Entrada=ahora,TarifaId=tarifa.Id,Precio=tarifa.Precio,MinutosUnidad=tarifa.MinutosUnidad,Mensual=tarifa.Mensual,Impuesto=empresa.Impuesto,Gracia=empresa.MinutosGracia,ConvenioId=convenio,EventoId=evento,UsuarioIngresoId=Seguridad.UsuarioId};
        db.Tickets.Add(ticket);db.SaveChanges();db.Registrar("INSERT","Tickets",$"Ticket {ticket.Id}; placa {placa}; espacio {elegido.Codigo}; ingreso {ahora}");db.Registrar("UPDATE","Espacios",$"Espacio {elegido.Codigo}: Ocupado; ticket {ticket.Id}");db.SaveChanges();transaccion.Commit();return ticket.Id;
    }
    private static Ticket Abierto(ParqueoContext db,int id)=>db.Tickets.SingleOrDefault(t=>t.Id==id && t.Salida==null)??throw new Exception("El ticket no existe o ya fue cobrado.");
    private static decimal CalcularSubtotal(ParqueoContext db,Ticket t,DateTime ahora)
    {
        string periodo=ahora.ToString("yyyy-MM");
        bool mensualidadPagada=t.Mensual && db.Mensualidades.Any(m=>m.ConvenioId==t.ConvenioId && m.Periodo==periodo);
        return CalcularSubtotal(t,ahora,mensualidadPagada);
    }
    internal static decimal CalcularSubtotal(Ticket ticket,DateTime ahora,bool mensualidadPagada)
    {
        if(ticket.Mensual)return mensualidadPagada?0:ticket.Precio;
        double segundos=Math.Max(0,Math.Floor((ahora-ticket.Entrada).TotalSeconds));
        if(segundos<=ticket.Gracia*60)return 0;
        return Math.Ceiling((decimal)segundos/(ticket.MinutosUnidad*60m))*ticket.Precio;
    }
    internal static decimal TotalEstimado(Ticket ticket,DateTime ahora,bool mensualidadPagada)
        =>Math.Round(CalcularSubtotal(ticket,ahora,mensualidadPagada)*(1+ticket.Impuesto/100),2,MidpointRounding.AwayFromZero);
    internal static string Tiempo(DateTime entrada,DateTime hasta)
    {
        var tiempo=hasta>entrada?hasta-entrada:TimeSpan.Zero;
        return $"{(long)tiempo.TotalHours} h {tiempo.Minutes:00} min";
    }
    public static DataRow Cotizar(int id){Seguridad.Exigir("Operación");using var db=new ParqueoContext();var t=Abierto(db,id);DateTime ahora=DateTime.Now;decimal sub=CalcularSubtotal(db,t,ahora);return Tablas.Crear(new[]{new{Total=Math.Round(sub*(1+t.Impuesto/100),2,MidpointRounding.AwayFromZero),Minutos=(long)(ahora-t.Entrada).TotalMinutes,Subtotal=sub}}).Rows[0];}
    public static void Cobrar(int id)
    {
        Seguridad.Exigir("Operación",true);using var db=new ParqueoContext();using var transaccion=db.Database.BeginTransaction(IsolationLevel.Serializable);var t=Abierto(db,id);DateTime ahora=DateTime.Now;decimal sub=CalcularSubtotal(db,t,ahora);
        if(t.Mensual){string periodo=ahora.ToString("yyyy-MM");if(!db.Mensualidades.Any(m=>m.ConvenioId==t.ConvenioId && m.Periodo==periodo)){db.Mensualidades.Add(new Mensualidad{ConvenioId=t.ConvenioId!.Value,Periodo=periodo,TicketId=id,Monto=sub});db.Registrar("INSERT","Mensualidades",$"Convenio {t.ConvenioId}; período {periodo}; cuota {sub}");}}
        t.Salida=ahora;t.Subtotal=sub;t.Total=Math.Round(sub*(1+t.Impuesto/100),2,MidpointRounding.AwayFromZero);t.UsuarioSalidaId=Seguridad.UsuarioId;
        db.Registrar("UPDATE","Tickets",$"Ticket {id}; salida {ahora}; subtotal {sub}; total {t.Total}");db.Registrar("UPDATE","Espacios",$"Espacio {t.EspacioId}: liberado; ticket {id}");db.SaveChanges();transaccion.Commit();
    }
    public static DataTable Ticket(int id){using var db=new ParqueoContext();var t=db.Tickets.Single(t=>t.Id==id);var tabla=Tablas.Crear(new[]{t});tabla.Columns.Add("Codigo");tabla.Rows[0]["Codigo"]=db.Espacios.Single(e=>e.Id==t.EspacioId).Codigo;return tabla;}
    public static string Comprobante(int id)
    {
        Seguridad.Exigir("Operación"); var t=Ticket(id).Rows[0]; var c=Consultas.EmpresaTabla().Rows[0];
        string texto=$"{c["Nombre"]}\r\n{c["Direccion"]}\r\n\r\nTICKET N.º {id:000000}\r\nPlaca: {t["Placa"]}\r\nEspacio: {t["Codigo"]}\r\nIngreso: {t["Entrada"]:dd/MM/yyyy HH:mm:ss}\r\nTarifa: {c["Moneda"]} {t["Precio"]:N2} / {t["MinutosUnidad"]} minutos\r\n";
        if(t["Salida"]!=DBNull.Value) texto+=$"Salida: {t["Salida"]:dd/MM/yyyy HH:mm:ss}\r\nSubtotal: {c["Moneda"]} {t["Subtotal"]:N2}\r\nImpuesto ({t["Impuesto"]}%): {c["Moneda"]} {Convert.ToDecimal(t["Total"])-Convert.ToDecimal(t["Subtotal"]):N2}\r\nTOTAL: {c["Moneda"]} {t["Total"]:N2}\r\n";
        if((bool)t["Mensual"]) texto+="Convenio mensual: se cobra una vez por mes calendario al convenio, en su primera salida.\r\n";
        return texto+"\r\nComprobante académico. Conserve su ticket.";
    }
}
