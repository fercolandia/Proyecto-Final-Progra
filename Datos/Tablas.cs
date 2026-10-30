using System.Data;
using System.ComponentModel.DataAnnotations;
namespace Parqueo.Datos;
// Solo adapta listas de objetos a las tablas visuales de Windows Forms.
// El acceso a SQL Server lo realiza Entity Framework con ParqueoContext.
public static class Tablas
{
    public static DataTable Crear<T>(IEnumerable<T> objetos)=>Crear(objetos.Cast<object>(),typeof(T));
    public static DataTable Crear(IEnumerable<object> objetos,Type tipo,string[]? campos=null)
    {
        var tabla=Esquema(tipo,campos);foreach(var objeto in objetos){var r=tabla.NewRow();foreach(DataColumn c in tabla.Columns)r[c]=tipo.GetProperty(c.ColumnName)!.GetValue(objeto)??DBNull.Value;tabla.Rows.Add(r);}return tabla;
    }
    public static DataTable Esquema(Type tipo,string[]? campos=null)
    {
        var tabla=new DataTable();foreach(var propiedad in tipo.GetProperties()){
            if(campos!=null && !campos.Contains(propiedad.Name))continue;
            var t=Nullable.GetUnderlyingType(propiedad.PropertyType)??propiedad.PropertyType;
            var c=tabla.Columns.Add(propiedad.Name,t);c.AllowDBNull=Nullable.GetUnderlyingType(propiedad.PropertyType)!=null || (t==typeof(string) && !propiedad.IsDefined(typeof(RequiredAttribute),true));
            if(t==typeof(string))c.MaxLength=propiedad.GetCustomAttributes(typeof(MaxLengthAttribute),true).Cast<MaxLengthAttribute>().FirstOrDefault()?.Length??500;
        }return tabla;
    }
}
