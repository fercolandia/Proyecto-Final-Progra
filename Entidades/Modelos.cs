using System.ComponentModel.DataAnnotations;
namespace Parqueo.Entidades;

public class TextoInterfaz
{
    public int Id { get; set; }
    [Required] [MaxLength(300)] public string Clave { get; set; } = "";
    [Required] [MaxLength(500)] public string Texto { get; set; } = "";
}

public class Empresa
{
    public int Id { get; set; }
    [Required] [MaxLength(120)] public string Nombre { get; set; } = "";
    [Required] [MaxLength(250)] public string Direccion { get; set; } = "";
    [Required] [MaxLength(10)] public string Moneda { get; set; } = "";
    public decimal Impuesto { get; set; }
    [Required(AllowEmptyStrings=true)] [MaxLength(500)] public string Logotipo { get; set; } = "";
    public int MinutosGracia { get; set; }
}

public class Rol
{
    public int Id { get; set; }
    [Required] [MaxLength(60)] public string Nombre { get; set; } = "";
}

public class Modulo
{
    public int Id { get; set; }
    [Required] [MaxLength(60)] public string Nombre { get; set; } = "";
}

public class Permiso
{
    public int Id { get; set; }
    public int RolId { get; set; }
    public int ModuloId { get; set; }
    public bool Consultar { get; set; }
    public bool Modificar { get; set; }
}

public class Usuario
{
    public int Id { get; set; }
    [Required] [MaxLength(60)] public string Nombre { get; set; } = "";
    [Required] [MaxLength(300)] public string Hash { get; set; } = "";
    public int RolId { get; set; }
    public bool Activo { get; set; }
}

public class Zona
{
    public int Id { get; set; }
    [Required] [MaxLength(60)] public string Nombre { get; set; } = "";
}

public class TipoEspacio
{
    public int Id { get; set; }
    [Required] [MaxLength(60)] public string Nombre { get; set; } = "";
}

public class TipoVehiculo
{
    public int Id { get; set; }
    [Required] [MaxLength(60)] public string Nombre { get; set; } = "";
}

public class Tarifa
{
    public int Id { get; set; }
    [Required] [MaxLength(80)] public string Nombre { get; set; } = "";
    public int TipoEspacioId { get; set; }
    public int MinutosUnidad { get; set; }
    public decimal Precio { get; set; }
    public bool Mensual { get; set; }
    public bool Regular { get; set; }
    public bool Activa { get; set; }
}

public class Convenio
{
    public int Id { get; set; }
    [Required] [MaxLength(100)] public string Nombre { get; set; } = "";
    [Required] [MaxLength(150)] public string Contacto { get; set; } = "";
    public DateTime Inicio { get; set; }
    public DateTime Fin { get; set; }
    public int TarifaId { get; set; }
    public bool Activo { get; set; }
}

public class Evento
{
    public int Id { get; set; }
    [Required] [MaxLength(100)] public string Nombre { get; set; } = "";
    public DateTime Inicio { get; set; }
    public DateTime Fin { get; set; }
    public int TarifaId { get; set; }
    public bool Activo { get; set; }
}

public class Espacio
{
    public int Id { get; set; }
    [Required] [MaxLength(20)] public string Codigo { get; set; } = "";
    public int ZonaId { get; set; }
    public int TipoEspacioId { get; set; }
    public bool FueraServicio { get; set; }
}

public class Reserva
{
    public int Id { get; set; }
    public int EspacioId { get; set; }
    public int? ConvenioId { get; set; }
    public int? EventoId { get; set; }
}

public class Ticket
{
    public int Id { get; set; }
    [Required] [MaxLength(15)] public string Placa { get; set; } = "";
    public int TipoVehiculoId { get; set; }
    public int EspacioId { get; set; }
    public DateTime Entrada { get; set; }
    public DateTime? Salida { get; set; }
    public int TarifaId { get; set; }
    public decimal Precio { get; set; }
    public int MinutosUnidad { get; set; }
    public bool Mensual { get; set; }
    public decimal Impuesto { get; set; }
    public int Gracia { get; set; }
    public int? ConvenioId { get; set; }
    public int? EventoId { get; set; }
    public decimal? Subtotal { get; set; }
    public decimal? Total { get; set; }
    public int UsuarioIngresoId { get; set; }
    public int? UsuarioSalidaId { get; set; }
}

public class Mensualidad
{
    public int Id { get; set; }
    public int ConvenioId { get; set; }
    [Required] [MaxLength(7)] public string Periodo { get; set; } = "";
    public int TicketId { get; set; }
    public decimal Monto { get; set; }
}

public class RegistroBitacora
{
    public long Id { get; set; }
    [Required] [MaxLength(60)] public string Usuario { get; set; } = "";
    [Required] [MaxLength(10)] public string Accion { get; set; } = "";
    [Required] [MaxLength(60)] public string Tabla { get; set; } = "";
    public DateTime Fecha { get; set; }
    [Required] public string Valores { get; set; } = "";
}