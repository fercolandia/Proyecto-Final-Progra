using System.Data;
using Parqueo.Negocio;
namespace Parqueo
{
public partial class EditorForm : System.Windows.Forms.Form
{
    private Catalogo? catalogo;
    private int? registroId;
    private readonly Dictionary<string,Control> campos=new();
    public EditorForm()
    {
        InitializeComponent();
    }
    public EditorForm(Catalogo c,DataRow? fila):this()
    {
        catalogo=c;registroId=fila==null?null:(int)fila["Id"];Text=$"{c.Titulo} · {(fila==null?"Nuevo":"Editar")}";
        var relaciones=new Dictionary<string,string>{{"ZonaId","Zonas"},{"TipoEspacioId","TiposEspacio"},{"TarifaId","Tarifas"},{"ConvenioId","Convenios"},{"EventoId","Eventos"},{"EspacioId","Espacios"},{"RolId","Roles"},{"ModuloId","Modulos"}};
        var esquema=Catalogos.Esquema(c);
        campos["Nombre"]=campoNombre;filaNombre.Visible=c.Campos.Contains("Nombre");
        campos["Codigo"]=campoCodigo;filaCodigo.Visible=c.Campos.Contains("Codigo");
        campos["Clave"]=campoClave;filaClave.Visible=c.Campos.Contains("Clave");
        campos["Texto"]=campoTexto;filaTexto.Visible=c.Campos.Contains("Texto");
        campos["Direccion"]=campoDireccion;filaDireccion.Visible=c.Campos.Contains("Direccion");
        campos["Moneda"]=campoMoneda;filaMoneda.Visible=c.Campos.Contains("Moneda");
        campos["Impuesto"]=campoImpuesto;filaImpuesto.Visible=c.Campos.Contains("Impuesto");
        campos["Logotipo"]=campoLogotipo;filaLogotipo.Visible=c.Campos.Contains("Logotipo");
        campos["MinutosGracia"]=campoMinutosGracia;filaMinutosGracia.Visible=c.Campos.Contains("MinutosGracia");
        campos["ZonaId"]=campoZonaId;filaZonaId.Visible=c.Campos.Contains("ZonaId");
        campos["TipoEspacioId"]=campoTipoEspacioId;filaTipoEspacioId.Visible=c.Campos.Contains("TipoEspacioId");
        campos["MinutosUnidad"]=campoMinutosUnidad;filaMinutosUnidad.Visible=c.Campos.Contains("MinutosUnidad");
        campos["Precio"]=campoPrecio;filaPrecio.Visible=c.Campos.Contains("Precio");
        campos["Mensual"]=campoMensual;filaMensual.Visible=c.Campos.Contains("Mensual");
        campos["Regular"]=campoRegular;filaRegular.Visible=c.Campos.Contains("Regular");
        campos["Activa"]=campoActiva;filaActiva.Visible=c.Campos.Contains("Activa");
        campos["FueraServicio"]=campoFueraServicio;filaFueraServicio.Visible=c.Campos.Contains("FueraServicio");
        campos["Contacto"]=campoContacto;filaContacto.Visible=c.Campos.Contains("Contacto");
        campos["Inicio"]=campoInicio;filaInicio.Visible=c.Campos.Contains("Inicio");
        campos["Fin"]=campoFin;filaFin.Visible=c.Campos.Contains("Fin");
        campos["TarifaId"]=campoTarifaId;filaTarifaId.Visible=c.Campos.Contains("TarifaId");
        campos["Activo"]=campoActivo;filaActivo.Visible=c.Campos.Contains("Activo");
        campos["EspacioId"]=campoEspacioId;filaEspacioId.Visible=c.Campos.Contains("EspacioId");
        campos["ConvenioId"]=campoConvenioId;filaConvenioId.Visible=c.Campos.Contains("ConvenioId");
        campos["EventoId"]=campoEventoId;filaEventoId.Visible=c.Campos.Contains("EventoId");
        campos["RolId"]=campoRolId;filaRolId.Visible=c.Campos.Contains("RolId");
        campos["ModuloId"]=campoModuloId;filaModuloId.Visible=c.Campos.Contains("ModuloId");
        campos["Consultar"]=campoConsultar;filaConsultar.Visible=c.Campos.Contains("Consultar");
        campos["Modificar"]=campoModificar;filaModificar.Visible=c.Campos.Contains("Modificar");
        campos["Hash"]=campoHash;filaHash.Visible=c.Campos.Contains("Hash");
        foreach(string nombre in c.Campos){
            var control=campos[nombre];var columna=esquema.Columns[nombre]!;object? valor=fila!=null && fila.Table.Columns.Contains(nombre) && !fila.IsNull(nombre)?fila[nombre]:null;
            if(control is ComboBox combo){var datos=Consultas.Opciones(relaciones[nombre]);if(columna.AllowDBNull){var opcion=datos.NewRow();opcion[0]=0;opcion[1]="Sin selección";datos.Rows.InsertAt(opcion,0);}combo.ValueMember="Id";combo.DisplayMember="Nombre";combo.DataSource=datos;if(valor!=null)combo.SelectedValue=valor;else if(columna.AllowDBNull)combo.SelectedValue=0;}
            else if(control is CheckBox check)check.Checked=valor!=null?Convert.ToBoolean(valor):nombre is "Activo" or "Activa" or "Consultar";
            else if(control is DateTimePicker fecha)fecha.Value=valor==null?(nombre=="Fin"?DateTime.Now.AddDays(1):DateTime.Now):Convert.ToDateTime(valor);
            else if(control is NumericUpDown numero)numero.Value=valor==null?0:Convert.ToDecimal(valor);
            else if(control is TextBox texto){texto.MaxLength=columna.MaxLength>0?columna.MaxLength:500;texto.Text=nombre=="Hash"?"":Convert.ToString(valor);}
        }
        if(fila!=null)lblHash.Text="Contraseña nueva opcional";
    }
    private void Guardar_Click(object? sender,EventArgs e)=>Controles.Intentar(()=>{
        if(catalogo==null)return;var valores=new Dictionary<string,object?>();foreach(string nombre in catalogo.Campos){var control=campos[nombre];valores[nombre]=control switch{ComboBox combo=>Controles.Id(combo)==0?null:Controles.Id(combo),CheckBox check=>check.Checked,DateTimePicker fecha=>fecha.Value,NumericUpDown numero=>numero.Value,_=>control.Text.Trim()};}
        Catalogos.Guardar(catalogo,registroId,valores);DialogResult=DialogResult.OK;Close();
    });
}

}
