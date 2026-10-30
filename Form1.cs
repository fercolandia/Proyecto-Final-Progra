using System.ComponentModel;
using System.Data;
using Parqueo.Negocio;
namespace Parqueo
{
public partial class Form1 : System.Windows.Forms.Form
{
    public bool CerrarSesionSolicitado { get; private set; }
    private bool preparando=true;
    public Form1() { InitializeComponent(); }
    private bool EsDiseno => DesignMode || LicenseManager.UsageMode==LicenseUsageMode.Designtime;
    private void Form1_Shown(object? sender,EventArgs e)
    {
        if(EsDiseno)return;
        Controles.Intentar(()=>{
            Controles.CargarTextos();Text=$"Parqueo · {Seguridad.Nombre}";
            var empresa=Consultas.EmpresaTabla().Rows[0];nombreEmpresa.Text=Convert.ToString(empresa["Nombre"]);sesion.Text=$"Sesión activa\n{Seguridad.Nombre}";
            string ruta=Convert.ToString(empresa["Logotipo"])??"";if(File.Exists(ruta)){using var imagen=Image.FromFile(ruta);logo.Image=new Bitmap(imagen);logo.Visible=true;marca.Visible=false;}
            foreach(var grid in new[]{gridCat,gridEmp,gridSeg,gridReporte,gridBitacora})Controles.ConfigurarTabla(grid);
            foreach(var (pagina,modulo) in new[]{(paginaOperacion,"Operación"),(paginaCatalogos,"Catálogos"),(paginaEmpresa,"Empresa"),(paginaSeguridad,"Seguridad"),(paginaReportes,"Reportes"),(paginaBitacora,"Bitácora")})if(!Seguridad.Puede(modulo))tabs.TabPages.Remove(pagina);
            if(Seguridad.Puede("Catálogos"))PrepararCatalogo(comboCat,"Catálogos");
            if(Seguridad.Puede("Empresa"))PrepararCatalogo(comboEmp,"Empresa");
            if(Seguridad.Puede("Seguridad"))PrepararCatalogo(comboSeg,"Seguridad");
            if(Seguridad.Puede("Reportes")){tipoReporte.DataSource=Consultas.Opciones("TiposEspacio");var t=(DataTable)tipoReporte.DataSource;var fila=t.NewRow();fila[0]=0;fila[1]="Todos los tipos";t.Rows.InsertAt(fila,0);tipoReporte.ValueMember="Id";tipoReporte.DisplayMember="Nombre";tipoReporte.SelectedValue=0;}
            desdeReporte.Value=hastaReporte.Value=desdeBitacora.Value=hastaBitacora.Value=DateTime.Today;
            Controles.Traducir(this);preparando=false;
            foreach(var combo in new[]{comboCat,comboEmp,comboSeg})if(combo.SelectedItem is Catalogo)CargarCatalogo(combo);
            if(Seguridad.Puede("Operación")){operacion.Iniciar();reloj.Start();}
        });
    }
    private void Form1_FormClosed(object? sender,FormClosedEventArgs e){reloj.Stop();logo.Image?.Dispose();}
    private void CerrarSesion_Click(object? sender,EventArgs e){CerrarSesionSolicitado=true;Close();}
    private void Reloj_Tick(object? sender,EventArgs e){if(!preparando && Seguridad.Puede("Operación"))Controles.Intentar(Actualizar);}
    private static int Seleccion(DataGridView grid){if(grid.CurrentRow==null)throw new Exception("Seleccione una fila.");return Convert.ToInt32(grid.CurrentRow.Cells["Id"].Value);}
    private void Actualizar()=>operacion.Actualizar();
    private void PrepararCatalogo(ComboBox combo,string modulo){combo.Items.Clear();combo.Items.AddRange(Catalogos.Lista.Where(c=>c.Modulo==modulo).ToArray());combo.SelectedIndex=0;}
    private ComboBox ComboDe(object? control)=>control==comboCat || control==gridCat || control==btnNuevoCat || control==btnEliminarCat?comboCat:control==comboEmp || control==gridEmp || control==btnNuevoEmp || control==btnEliminarEmp || control==btnEditarEmpresa?comboEmp:comboSeg;
    private (DataGridView Grid,Button Nuevo,Button Eliminar,Label Ayuda) Vista(ComboBox combo)=>combo==comboCat?(gridCat,btnNuevoCat,btnEliminarCat,ayudaCat):combo==comboEmp?(gridEmp,btnNuevoEmp,btnEliminarEmp,ayudaEmp):(gridSeg,btnNuevoSeg,btnEliminarSeg,ayudaSeg);
    private void CargarCatalogo(ComboBox combo)
    {
        if(combo.SelectedItem is not Catalogo catalogo)return;var vista=Vista(combo);bool empresa=catalogo.Tabla=="Empresa",editar=Seguridad.Puede(catalogo.Modulo,true);
        vista.Nuevo.Visible=editar&&!empresa;vista.Eliminar.Visible=editar&&!empresa;if(combo==comboEmp)btnEditarEmpresa.Visible=editar&&empresa;
        vista.Ayuda.Text=!editar?"Consulta de registros.":empresa?"Seleccione la empresa y pulse Editar empresa.":catalogo.Modulo=="Seguridad"?"Doble clic para editar. Los permisos se aplican al iniciar sesión de nuevo.":"Doble clic en una fila para editar sus datos.";
        vista.Grid.DataSource=Catalogos.Leer(catalogo);
    }
    private void Catalogo_Changed(object? sender,EventArgs e){if(!preparando && sender is ComboBox combo)Controles.Intentar(()=>CargarCatalogo(combo));}
    private void EditarSeleccion(ComboBox combo)
    {
        if(combo.SelectedItem is not Catalogo catalogo)return;var grid=Vista(combo).Grid;int id=Seleccion(grid);var fila=((DataTable)grid.DataSource!).AsEnumerable().Single(r=>(int)r["Id"]==id);using var formulario=new EditorForm(catalogo,fila);if(formulario.ShowDialog(this)==DialogResult.OK)CargarCatalogo(combo);
    }
    private void Catalogo_DobleClick(object? sender,DataGridViewCellEventArgs e){if(e.RowIndex>=0)Controles.Intentar(()=>{var combo=ComboDe(sender);if(combo.SelectedItem is Catalogo c && Seguridad.Puede(c.Modulo,true))EditarSeleccion(combo);});}
    private void EditarEmpresa_Click(object? sender,EventArgs e)=>Controles.Intentar(()=>EditarSeleccion(comboEmp));
    private void NuevoCatalogo_Click(object? sender,EventArgs e)=>Controles.Intentar(()=>{var combo=ComboDe(sender);if(combo.SelectedItem is Catalogo c){using var f=new EditorForm(c,null);if(f.ShowDialog(this)==DialogResult.OK)CargarCatalogo(combo);}});
    private void EliminarCatalogo_Click(object? sender,EventArgs e)=>Controles.Intentar(()=>{var combo=ComboDe(sender);if(combo.SelectedItem is Catalogo c && MessageBox.Show("¿Eliminar el registro seleccionado?","Eliminar",MessageBoxButtons.YesNo)==DialogResult.Yes){Catalogos.Eliminar(c,Seleccion(Vista(combo).Grid));CargarCatalogo(combo);}});
    private void ConsultarReporte_Click(object? sender,EventArgs e)=>Controles.Intentar(()=>{
        DateTime desde=desdeReporte.Value.Date,hasta=hastaReporte.Value.Date.AddDays(1);if(hasta<=desde)throw new Exception("Revise las fechas.");gridReporte.Tag=opcionReporte.SelectedIndex==3?"NumeroTicket":null;gridReporte.DataSource=Consultas.Reporte(opcionReporte.SelectedIndex,desde,hasta,Controles.Id(tipoReporte));notaReporte.Text=opcionReporte.SelectedIndex==3?"Doble clic en un comprobante para abrirlo.":"Resultados del período y tipo seleccionados. La ocupación actual usa el estado presente.";
    });
    private void ConsultarBitacora_Click(object? sender,EventArgs e)=>Controles.Intentar(()=>{DateTime desde=desdeBitacora.Value.Date,hasta=hastaBitacora.Value.Date.AddDays(1);if(hasta<=desde)throw new Exception("Revise las fechas.");gridBitacora.DataSource=Consultas.Bitacora(desde,hasta,usuarioBitacora.Text.Trim(),accionBitacora.Text);});
    private void Reporte_DobleClick(object? sender,DataGridViewCellEventArgs e){if(e.RowIndex>=0 && opcionReporte.SelectedIndex==3 && Seguridad.Puede("Operación"))Controles.Intentar(()=>Controles.Ticket(ParqueoServicio.Comprobante(Seleccion(gridReporte))));}
}

}
