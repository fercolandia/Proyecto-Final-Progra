using System.ComponentModel;
using Parqueo.Negocio;
namespace Parqueo
{
public partial class IngresoForm : System.Windows.Forms.Form
{
    public int TicketId { get; private set; }
    public IngresoForm()
    {
        InitializeComponent();
    }
    private void IngresoForm_Shown(object? sender,EventArgs e)
    {
        if(DesignMode || LicenseManager.UsageMode==LicenseUsageMode.Designtime)return;
        Controles.Intentar(()=>{Seguridad.Exigir("Operación",true);Cargar(vehiculo,"TiposVehiculo",false);Cargar(tipo,"TiposEspacio",false);Cargar(convenio,"Convenios",true);Cargar(evento,"Eventos",true);});
    }
    private static void Cargar(ComboBox combo,string tabla,bool opcional){var datos=Consultas.Opciones(tabla,opcional);if(opcional){var r=datos.NewRow();r[0]=0;r[1]="Sin selección";datos.Rows.InsertAt(r,0);}combo.ValueMember="Id";combo.DisplayMember="Nombre";combo.DataSource=datos;}
    private void Guardar_Click(object? sender,EventArgs e)=>Controles.Intentar(()=>{int c=Controles.Id(convenio),ev=Controles.Id(evento);TicketId=ParqueoServicio.Ingresar(placa.Text,Controles.Id(vehiculo),Controles.Id(tipo),c==0?null:c,ev==0?null:ev);DialogResult=DialogResult.OK;Close();});
}

}
