using Microsoft.EntityFrameworkCore;
using Parqueo.Entidades;
namespace Parqueo.Datos;
public class ParqueoContext:DbContext
{
    public ParqueoContext() { }
    public ParqueoContext(string cadena) { this.cadena=cadena; }
    private readonly string? cadena;
    protected override void OnConfiguring(DbContextOptionsBuilder opciones)=>opciones.UseSqlServer(cadena??BaseDatos.Cadena);
    public DbSet<TextoInterfaz> TextosInterfaz => Set<TextoInterfaz>();
    public DbSet<Empresa> Empresa => Set<Empresa>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Modulo> Modulos => Set<Modulo>();
    public DbSet<Permiso> Permisos => Set<Permiso>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Zona> Zonas => Set<Zona>();
    public DbSet<TipoEspacio> TiposEspacio => Set<TipoEspacio>();
    public DbSet<TipoVehiculo> TiposVehiculo => Set<TipoVehiculo>();
    public DbSet<Tarifa> Tarifas => Set<Tarifa>();
    public DbSet<Convenio> Convenios => Set<Convenio>();
    public DbSet<Evento> Eventos => Set<Evento>();
    public DbSet<Espacio> Espacios => Set<Espacio>();
    public DbSet<Reserva> Reservas => Set<Reserva>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Mensualidad> Mensualidades => Set<Mensualidad>();
    public DbSet<RegistroBitacora> Bitacora => Set<RegistroBitacora>();
    protected override void OnModelCreating(ModelBuilder model)
{
        model.Entity<TextoInterfaz>().ToTable("TextosInterfaz");
        model.Entity<Empresa>().ToTable("Empresa");
        model.Entity<Rol>().ToTable("Roles");
        model.Entity<Modulo>().ToTable("Modulos");
        model.Entity<Permiso>().ToTable("Permisos");
        model.Entity<Usuario>().ToTable("Usuarios");
        model.Entity<Zona>().ToTable("Zonas");
        model.Entity<TipoEspacio>().ToTable("TiposEspacio");
        model.Entity<TipoVehiculo>().ToTable("TiposVehiculo");
        model.Entity<Tarifa>().ToTable("Tarifas");
        model.Entity<Convenio>().ToTable("Convenios");
        model.Entity<Evento>().ToTable("Eventos");
        model.Entity<Espacio>().ToTable("Espacios");
        model.Entity<Reserva>().ToTable("Reservas");
        model.Entity<Ticket>().ToTable("Tickets");
        model.Entity<Mensualidad>().ToTable("Mensualidades");
        model.Entity<RegistroBitacora>().ToTable("Bitacora");
        model.Entity<Permiso>().HasOne<Rol>().WithMany().HasForeignKey(x=>x.RolId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Permiso>().HasOne<Modulo>().WithMany().HasForeignKey(x=>x.ModuloId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Usuario>().HasOne<Rol>().WithMany().HasForeignKey(x=>x.RolId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Tarifa>().HasOne<TipoEspacio>().WithMany().HasForeignKey(x=>x.TipoEspacioId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Convenio>().HasOne<Tarifa>().WithMany().HasForeignKey(x=>x.TarifaId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Evento>().HasOne<Tarifa>().WithMany().HasForeignKey(x=>x.TarifaId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Espacio>().HasOne<Zona>().WithMany().HasForeignKey(x=>x.ZonaId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Espacio>().HasOne<TipoEspacio>().WithMany().HasForeignKey(x=>x.TipoEspacioId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Reserva>().HasOne<Espacio>().WithMany().HasForeignKey(x=>x.EspacioId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Reserva>().HasOne<Convenio>().WithMany().HasForeignKey(x=>x.ConvenioId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Reserva>().HasOne<Evento>().WithMany().HasForeignKey(x=>x.EventoId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Ticket>().HasOne<TipoVehiculo>().WithMany().HasForeignKey(x=>x.TipoVehiculoId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Ticket>().HasOne<Espacio>().WithMany().HasForeignKey(x=>x.EspacioId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Ticket>().HasOne<Tarifa>().WithMany().HasForeignKey(x=>x.TarifaId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Ticket>().HasOne<Convenio>().WithMany().HasForeignKey(x=>x.ConvenioId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Ticket>().HasOne<Evento>().WithMany().HasForeignKey(x=>x.EventoId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Ticket>().HasOne<Usuario>().WithMany().HasForeignKey(x=>x.UsuarioIngresoId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Ticket>().HasOne<Usuario>().WithMany().HasForeignKey(x=>x.UsuarioSalidaId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Mensualidad>().HasOne<Convenio>().WithMany().HasForeignKey(x=>x.ConvenioId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Mensualidad>().HasOne<Ticket>().WithMany().HasForeignKey(x=>x.TicketId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Empresa>().Property(x=>x.Id).ValueGeneratedNever();
        foreach(var entidad in model.Model.GetEntityTypes())
            foreach(var propiedad in entidad.GetProperties())
                if(propiedad.ClrType==typeof(decimal) || propiedad.ClrType==typeof(decimal?)){propiedad.SetPrecision(12);propiedad.SetScale(2);}
    }
    public void Registrar(string accion,string tabla,string valores)
    { Bitacora.Add(new RegistroBitacora{Usuario=BaseDatos.Usuario,Accion=accion,Tabla=tabla,Fecha=DateTime.Now,Valores=valores}); }
}