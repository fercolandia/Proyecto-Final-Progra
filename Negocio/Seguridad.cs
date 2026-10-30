using System.Security.Cryptography;
using System.Data;
using Parqueo.Datos;
using Parqueo.Entidades;
using Microsoft.EntityFrameworkCore;
namespace Parqueo.Negocio;
public static class Seguridad
{
    public static int UsuarioId { get; private set; }
    public static int RolId { get; private set; }
    public static string Nombre { get; private set; } = "";
    private static List<(string Modulo,bool Consultar,bool Modificar)> permisos = new();
    public static string Hash(string clave)
    {
        if (clave.Length < 8) throw new Exception("La contraseña debe tener al menos 8 caracteres.");
        byte[] sal = RandomNumberGenerator.GetBytes(16);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(clave,sal,100000,HashAlgorithmName.SHA256,32);
        return $"100000:{Convert.ToBase64String(sal)}:{Convert.ToBase64String(hash)}";
    }
    public static bool Verificar(string clave,string guardado)
    {
        try { var p=guardado.Split(':'); var hash=Rfc2898DeriveBytes.Pbkdf2(clave,Convert.FromBase64String(p[1]),int.Parse(p[0]),HashAlgorithmName.SHA256,32); return CryptographicOperations.FixedTimeEquals(hash,Convert.FromBase64String(p[2])); }
        catch { return false; }
    }
    public static bool Login(string nombre,string clave)
    {
        using var db=new ParqueoContext();var usuario=db.Usuarios.SingleOrDefault(u=>u.Nombre==nombre && u.Activo);
        if(usuario==null || !Verificar(clave,usuario.Hash))return false;
        UsuarioId=usuario.Id;RolId=usuario.RolId;Nombre=usuario.Nombre;BaseDatos.Usuario=Nombre;
        permisos=(from p in db.Permisos join m in db.Modulos on p.ModuloId equals m.Id where p.RolId==RolId select new{m.Nombre,p.Consultar,p.Modificar}).ToList().Select(x=>(x.Nombre,x.Consultar,x.Modificar)).ToList();return true;
    }
    public static void CerrarSesion()
    {
        UsuarioId=0;RolId=0;Nombre="";permisos.Clear();BaseDatos.Usuario="Sin sesión";
    }
    public static bool Puede(string modulo,bool escribir=false)=>permisos.Any(p=>p.Modulo==modulo && (escribir?p.Modificar:p.Consultar));
    public static void Exigir(string modulo,bool escribir=false) { if(!Puede(modulo,escribir)) throw new Exception("Su usuario no tiene permiso para esta operación."); }
    public static void CrearAdministrador(string nombre,string clave)
    {
        if(string.IsNullOrWhiteSpace(nombre)) throw new Exception("Ingrese un nombre de usuario.");
        using var db=new ParqueoContext();using var transaccion=db.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
        if(db.Usuarios.Any())throw new Exception("El administrador inicial ya existe.");
        var administrador=new Usuario{Nombre=nombre.Trim(),Hash=Hash(clave),RolId=db.Roles.OrderBy(r=>r.Id).First().Id,Activo=true};
        db.Usuarios.Add(administrador);db.Registrar("INSERT","Usuarios",$"Administrador inicial: {administrador.Nombre}");db.SaveChanges();transaccion.Commit();
    }
}
