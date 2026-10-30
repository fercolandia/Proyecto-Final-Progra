using System.Data;
using Parqueo.Datos;
using Parqueo.Entidades;
using Microsoft.EntityFrameworkCore;
namespace Parqueo.Negocio;
public record Catalogo(string Tabla,string Modulo,string[] Campos)
{
    public string Titulo=>Tabla switch{"TiposEspacio"=>"Tipos de espacio","TiposVehiculo"=>"Tipos de vehículo","TextosInterfaz"=>"Textos de interfaz","Empresa"=>"Datos de empresa",_=>Tabla};
}
public static class Catalogos
{
    public static readonly Catalogo[] Lista = [
        new("Zonas","Catálogos",["Nombre"]),new("TiposEspacio","Catálogos",["Nombre"]),new("TiposVehiculo","Catálogos",["Nombre"]),
        new("Tarifas","Catálogos",["Nombre","TipoEspacioId","MinutosUnidad","Precio","Mensual","Regular","Activa"]),
        new("Espacios","Catálogos",["Codigo","ZonaId","TipoEspacioId","FueraServicio"]),
        new("Convenios","Catálogos",["Nombre","Contacto","Inicio","Fin","TarifaId","Activo"]),
        new("Eventos","Catálogos",["Nombre","Inicio","Fin","TarifaId","Activo"]),
        new("Reservas","Catálogos",["EspacioId","ConvenioId","EventoId"]),
        new("Roles","Seguridad",["Nombre"]),new("Permisos","Seguridad",["RolId","ModuloId","Consultar","Modificar"]),
        new("Usuarios","Seguridad",["Nombre","Hash","RolId","Activo"]),
        new("Empresa","Empresa",["Nombre","Direccion","Moneda","Impuesto","Logotipo","MinutosGracia"]),
        new("TextosInterfaz","Empresa",["Clave","Texto"]) ];
    public static Type Tipo(string tabla)=>tabla switch{
        "Zonas"=>typeof(Zona),"TiposEspacio"=>typeof(TipoEspacio),"TiposVehiculo"=>typeof(TipoVehiculo),"Tarifas"=>typeof(Tarifa),"Espacios"=>typeof(Espacio),"Convenios"=>typeof(Convenio),"Eventos"=>typeof(Evento),"Reservas"=>typeof(Reserva),"Roles"=>typeof(Rol),"Modulos"=>typeof(Modulo),"Permisos"=>typeof(Permiso),"Usuarios"=>typeof(Usuario),"Empresa"=>typeof(Empresa),"TextosInterfaz"=>typeof(TextoInterfaz),_=>throw new Exception("Catálogo no permitido.")};
    public static IEnumerable<object> Objetos(ParqueoContext db,string tabla)=>tabla switch{
        "Zonas"=>db.Zonas.ToList(),"TiposEspacio"=>db.TiposEspacio.ToList(),"TiposVehiculo"=>db.TiposVehiculo.ToList(),"Tarifas"=>db.Tarifas.ToList(),"Espacios"=>db.Espacios.ToList(),"Convenios"=>db.Convenios.ToList(),"Eventos"=>db.Eventos.ToList(),"Reservas"=>db.Reservas.ToList(),"Roles"=>db.Roles.ToList(),"Modulos"=>db.Modulos.ToList(),"Permisos"=>db.Permisos.ToList(),"Usuarios"=>db.Usuarios.ToList(),"Empresa"=>db.Empresa.ToList(),"TextosInterfaz"=>db.TextosInterfaz.ToList(),_=>throw new Exception("Catálogo no permitido.")};
    public static DataTable Esquema(Catalogo c)=>Tablas.Esquema(Tipo(c.Tabla));
    public static DataTable Leer(Catalogo c)
    {
        Seguridad.Exigir(c.Modulo);using var db=new ParqueoContext();var tabla=Tablas.Crear(Objetos(db,c.Tabla),Tipo(c.Tabla),new[]{"Id"}.Concat(c.Campos.Where(x=>x!="Hash")).ToArray());
        var relaciones=new Dictionary<string,(string Tabla,string Titulo)>{{"ZonaId",("Zonas","Zona")},{"TipoEspacioId",("TiposEspacio","Tipo de espacio")},{"TarifaId",("Tarifas","Tarifa")},{"ConvenioId",("Convenios","Convenio")},{"EventoId",("Eventos","Evento")},{"EspacioId",("Espacios","Espacio")},{"RolId",("Roles","Rol")},{"ModuloId",("Modulos","Módulo")}};
        foreach(var campo in c.Campos.Where(x=>relaciones.ContainsKey(x))){var relacion=relaciones[campo];var opciones=Consultas.Opciones(relacion.Tabla).AsEnumerable().ToDictionary(r=>(int)r["Id"],r=>(string)r["Nombre"]);tabla.Columns.Add(relacion.Titulo,typeof(string));foreach(DataRow fila in tabla.Rows)fila[relacion.Titulo]=fila.IsNull(campo)?"":opciones.GetValueOrDefault(Convert.ToInt32(fila[campo]),"");}
        return tabla;
    }
    private static string Describir(object objeto)=>string.Join("; ",objeto.GetType().GetProperties().Where(p=>p.Name!="Hash").Select(p=>$"{p.Name}: {p.GetValue(objeto)}"));
    public static void Guardar(Catalogo c,int? id,Dictionary<string,object?> valores)
    {
        Seguridad.Exigir(c.Modulo,true);if(!Lista.Contains(c))throw new Exception("Catálogo no permitido.");
        using var db=new ParqueoContext();using var transaccion=db.Database.BeginTransaction(IsolationLevel.Serializable);Type tipo=Tipo(c.Tabla);var entidad=id==null?Activator.CreateInstance(tipo)!:db.Find(tipo,id)??throw new Exception("El registro ya no existe.");
        string antes=id==null?"sin registro":Describir(entidad);
        if(c.Tabla=="Empresa" && id==null)throw new Exception("La empresa ya existe. Seleccione Editar.");
        foreach(var (nombre,valor) in valores){
            if(!c.Campos.Contains(nombre))throw new Exception("Campo no permitido.");var propiedad=tipo.GetProperty(nombre)!;
            if(nombre=="Hash"){string clave=Convert.ToString(valor)??"";if(id!=null && clave.Length==0)continue;propiedad.SetValue(entidad,Seguridad.Hash(clave));}
            else propiedad.SetValue(entidad,valor==null?null:Convert.ChangeType(valor,Nullable.GetUnderlyingType(propiedad.PropertyType)??propiedad.PropertyType));
        }
        Validar(db,entidad,c.Tabla,id??0);
        // Add inserta un objeto nuevo. En edición EF detecta las propiedades modificadas.
        if(id==null)db.Add(entidad);
        db.Registrar(id==null?"INSERT":"UPDATE",c.Tabla,$"Antes: {antes}; Después: {Describir(entidad)}");db.SaveChanges();transaccion.Commit();
    }
    public static void Eliminar(Catalogo c,int id)
    {
        Seguridad.Exigir(c.Modulo,true);if(!Lista.Contains(c))throw new Exception("Catálogo no permitido.");
        if(c.Tabla=="Empresa" || (c.Tabla=="Usuarios" && id==Seguridad.UsuarioId) || (c.Tabla=="Roles" && id==Seguridad.RolId))throw new Exception("No se puede eliminar este registro.");
        using var db=new ParqueoContext();var entidad=db.Find(Tipo(c.Tabla),id)??throw new Exception("El registro ya no existe.");db.Registrar("DELETE",c.Tabla,Describir(entidad));db.Remove(entidad);db.SaveChanges();
    }
    private static void Validar(ParqueoContext db,object entidad,string tabla,int id)
    {
        // Las reglas se validan en C#; SQL conserva solamente las tablas y sus relaciones.
        var errores=new List<System.ComponentModel.DataAnnotations.ValidationResult>();if(!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(entidad,new(entidad),errores,true))throw new Exception(string.Join("\n",errores.Select(e=>e.ErrorMessage)));
        string campo=tabla=="Espacios"?"Codigo":tabla=="TextosInterfaz"?"Clave":"Nombre";var propiedad=entidad.GetType().GetProperty(campo);
        if(propiedad!=null && tabla!="Empresa" && tabla!="Eventos"){
            string valor=Convert.ToString(propiedad.GetValue(entidad))??"";if(string.IsNullOrWhiteSpace(valor))throw new Exception($"Complete {campo}.");
            if(Objetos(db,tabla).Any(o=>Convert.ToInt32(o.GetType().GetProperty("Id")!.GetValue(o))!=id && string.Equals(Convert.ToString(propiedad.GetValue(o)),valor,StringComparison.OrdinalIgnoreCase)))throw new Exception($"Ya existe un registro con ese {campo}.");
        }
        switch(entidad){
        case Empresa e:
            if(e.Impuesto<0 || e.Impuesto>100 || e.MinutosGracia<0)throw new Exception("Revise el impuesto (0 a 100) y la gracia (0 o mayor).");break;
        case Usuario u:
            if(id==Seguridad.UsuarioId && (u.RolId!=Seguridad.RolId || !u.Activo))throw new Exception("No puede cambiar su propio rol ni desactivarse.");break;
        case Permiso p:
            if(p.Modificar && !p.Consultar)throw new Exception("Marque Consultar antes de Modificar.");if(db.Permisos.Any(x=>x.Id!=id && x.RolId==p.RolId && x.ModuloId==p.ModuloId))throw new Exception("Ya existen permisos para ese rol y módulo.");break;
        case Tarifa t:
            if(t.MinutosUnidad<=0 || t.Precio<0 || (t.Mensual && t.Regular))throw new Exception("Revise la unidad, el precio y la modalidad de tarifa.");
            if(t.Regular && t.Activa && db.Tarifas.Any(x=>x.Id!=id && x.TipoEspacioId==t.TipoEspacioId && x.Activa && x.Regular))throw new Exception("Ya existe una tarifa regular activa para ese tipo.");
            if(id!=0){var antes=db.Entry(t).OriginalValues;if((antes.GetValue<int>("TipoEspacioId")!=t.TipoEspacioId || antes.GetValue<bool>("Mensual")!=t.Mensual) && (db.Convenios.Any(c=>c.TarifaId==id)||db.Eventos.Any(e=>e.TarifaId==id)))throw new Exception("Conserve el tipo y modalidad de una tarifa que ya tiene convenios o eventos.");}break;
        case Espacio e:
            if(id!=0){var antes=db.Entry(e).OriginalValues;bool tipo=antes.GetValue<int>("TipoEspacioId")!=e.TipoEspacioId,estado=antes.GetValue<bool>("FueraServicio")!=e.FueraServicio;
            if((tipo||estado) && db.Tickets.Any(t=>t.EspacioId==id && t.Salida==null))throw new Exception("No puede modificar el tipo o estado de un espacio ocupado.");
            if((tipo || (!antes.GetValue<bool>("FueraServicio") && e.FueraServicio)) && db.Reservas.Any(r=>r.EspacioId==id))throw new Exception("Quite las reservas antes de cambiar este espacio.");}break;
        case Convenio c:
            if(c.Fin<=c.Inicio)throw new Exception("El fin debe ser posterior al inicio.");if(id!=0){var a=db.Entry(c).OriginalValues;if((a.GetValue<DateTime>("Inicio")!=c.Inicio || a.GetValue<DateTime>("Fin")!=c.Fin || a.GetValue<int>("TarifaId")!=c.TarifaId || (!a.GetValue<bool>("Activo")&&c.Activo)) && db.Reservas.Any(r=>r.ConvenioId==id))throw new Exception("Quite las reservas antes de cambiar fechas, tarifa o reactivar el convenio.");}break;
        case Evento e:
            if(e.Fin<=e.Inicio)throw new Exception("El fin debe ser posterior al inicio.");if(db.Tarifas.Find(e.TarifaId)?.Mensual==true)throw new Exception("Un evento no admite tarifa mensual.");
            if(id!=0){var a=db.Entry(e).OriginalValues;if((a.GetValue<DateTime>("Inicio")!=e.Inicio || a.GetValue<DateTime>("Fin")!=e.Fin || a.GetValue<int>("TarifaId")!=e.TarifaId || (!a.GetValue<bool>("Activo")&&e.Activo)) && db.Reservas.Any(r=>r.EventoId==id))throw new Exception("Quite las reservas antes de cambiar fechas, tarifa o reactivar el evento.");}break;
        case Reserva r:ValidarReserva(db,r,id);break;
        }
    }
    private static void ValidarReserva(ParqueoContext db,Reserva r,int id)
    {
        if((r.ConvenioId==null)==(r.EventoId==null))throw new Exception("Seleccione solamente un convenio o un evento.");
        var espacio=db.Espacios.Find(r.EspacioId)??throw new Exception("El espacio no existe.");var convenios=db.Convenios.ToList();var eventos=db.Eventos.ToList();
        (DateTime Inicio,DateTime Fin,bool Activo,int TarifaId) Datos(Reserva x){if(x.ConvenioId!=null){var c=convenios.Single(c=>c.Id==x.ConvenioId);return(c.Inicio,c.Fin,c.Activo,c.TarifaId);}var e=eventos.Single(e=>e.Id==x.EventoId);return(e.Inicio,e.Fin,e.Activo,e.TarifaId);}
        var actual=Datos(r);var tarifa=db.Tarifas.Find(actual.TarifaId)??throw new Exception("La tarifa no existe.");
        if(espacio.FueraServicio || espacio.TipoEspacioId!=tarifa.TipoEspacioId || (r.EventoId!=null && tarifa.Mensual))throw new Exception("El espacio está inhabilitado o no es compatible con la tarifa.");
        if(db.Tickets.Any(t=>t.EspacioId==r.EspacioId && t.Salida==null))throw new Exception("No puede reservar un espacio ocupado.");
        foreach(var otra in db.Reservas.Where(x=>x.EspacioId==r.EspacioId && x.Id!=id).ToList()){
            if((r.ConvenioId!=null && r.ConvenioId==otra.ConvenioId)||(r.EventoId!=null && r.EventoId==otra.EventoId))throw new Exception("El espacio ya está asignado a esta modalidad.");var datos=Datos(otra);
            if(actual.Activo && datos.Activo && actual.Inicio<datos.Fin && datos.Inicio<actual.Fin)throw new Exception("Ya existe una reserva que coincide en fechas.");
        }
    }
}
