using System.Data;
using Parqueo.Datos;
using Parqueo.Negocio;
namespace Parqueo
{
[System.ComponentModel.DesignerCategory("UserControl")]
public partial class Form1Operaciones : System.Windows.Forms.UserControl
{
    private bool preparando=true;
    public Form1Operaciones()
    {
        InitializeComponent();
    }
    public void Iniciar()
    {
        if(DesignMode || System.ComponentModel.LicenseManager.UsageMode==System.ComponentModel.LicenseUsageMode.Designtime)return;
        moneda=Convert.ToString(Consultas.EmpresaTabla().Rows[0]["Moneda"])??"";
        foreach(var grid in new[]{espacios,activos,historial,resumenDelDia})Controles.ConfigurarTabla(grid);
        if(!Seguridad.Puede("Reportes"))vistasOperacion.TabPages.Remove(paginaResumen);
        preparando=false;Actualizar();
    }
    public void Actualizar()=>ActualizarOperacion();
    private static int Seleccion(DataGridView grid){if(grid.CurrentRow==null)throw new Exception("Seleccione una fila.");return Convert.ToInt32(grid.CurrentRow.Cells["Id"].Value);}

    private void Ingreso_Click(object? sender,EventArgs e)=>Controles.Intentar(Ingreso);
    private void Ingreso()
    {
        Seguridad.Exigir("Operación",true);using var formulario=new IngresoForm();if(formulario.ShowDialog(FindForm())==DialogResult.OK){Actualizar();Controles.Ticket(ParqueoServicio.Comprobante(formulario.TicketId));}
    }
    private void Salida_Click(object? sender,EventArgs e)=>Controles.Intentar(Salida);
    private void Salida()
    {
        Seguridad.Exigir("Operación",true);int id=Seleccion(activos);var ticket=ParqueoServicio.Ticket(id).Rows[0];var estimado=ParqueoServicio.Cotizar(id);
        if(MessageBox.Show($"Ticket {id} · {ticket["Placa"]}\nEstimado a pagar: {moneda} {estimado["Total"]:N2}\nTiempo: {estimado["Minutos"]} minutos\n\n¿Confirmar salida y cobro?","Registrar salida",MessageBoxButtons.YesNo,MessageBoxIcon.Question)!=DialogResult.Yes)return;
        ParqueoServicio.Cobrar(id);Actualizar();Controles.Ticket(ParqueoServicio.Comprobante(id));
    }
    private DataTable? todosEspacios,todosActivos;
    private string ultimaPlaca="",moneda="";
    private bool actualizandoFiltros;
    private void VistasOperacion_SelectedIndexChanged(object? sender,EventArgs e){panelIndicadores.Visible=vistasOperacion.SelectedTab==paginaEspacios;}
    private void FiltroEspacios_Changed(object? sender,EventArgs e){if(!preparando && !actualizandoFiltros && todosEspacios!=null)Controles.Intentar(AplicarFiltros);}
    private void BuscarVehiculo_Changed(object? sender,EventArgs e){if(!preparando)Controles.Intentar(AplicarBusqueda);}
    private void BuscarHistorial_Click(object? sender,EventArgs e)=>Controles.Intentar(ConsultarHistorial);
    private void PlacaHistorial_KeyDown(object? sender,KeyEventArgs e){if(e.KeyCode==Keys.Enter){e.SuppressKeyPress=true;Controles.Intentar(ConsultarHistorial);}}
    private void PlacaHistorial_Changed(object? sender,EventArgs e){if(placaHistorial.Text.Length==0){ultimaPlaca="";historial.DataSource=null;informacionHistorial.Text="Escriba una placa y pulse Buscar o Enter.";}}
    private void Activo_DobleClick(object? sender,DataGridViewCellEventArgs e){if(e.RowIndex>=0)Controles.Intentar(()=>Controles.Ticket(ParqueoServicio.Comprobante(Seleccion(activos))));}
    private void Historial_DobleClick(object? sender,DataGridViewCellEventArgs e){if(e.RowIndex>=0)Controles.Intentar(()=>Controles.Ticket(ParqueoServicio.Comprobante(Seleccion(historial))));}
    private void ActualizarOperacion()
    {
        todosEspacios=ParqueoServicio.Disponibilidad();todosActivos=Consultas.Activos();
        actualizandoFiltros=true;LlenarFiltro(filtroZona,"ZonaId","Zona","Todas las zonas");LlenarFiltro(filtroTipo,"TipoEspacioId","Tipo","Todos los tipos");actualizandoFiltros=false;
        totalEspacios.Text=todosEspacios.Rows.Count.ToString();libres.Text=todosEspacios.AsEnumerable().Count(r=>(string)r["Estado"]=="Disponible").ToString();ocupados.Text=todosActivos.Rows.Count.ToString();reservados.Text=todosEspacios.AsEnumerable().Count(r=>(string)r["Estado"]=="Reservado").ToString();
        AplicarFiltros();AplicarBusqueda();
        if(ultimaPlaca.Length>0)CargarHistorial();
        if(Seguridad.Puede("Reportes")){
            var resumen=Consultas.ResumenDia();
            resumenDelDia.DataSource=Tablas.Crear(new[]{new{Fecha=resumen.Fecha.ToString("dd/MM/yyyy"),Ingresados=resumen.IngresadosHoy,Atendidos=resumen.AtendidosHoy,VehiculosDentro=resumen.VehiculosDentro,Ingresos=resumen.IngresosHoy}});
            fechaResumen.Text=$"Resumen de hoy · {resumen.Fecha:dd/MM/yyyy} · {moneda} · Actualizado {DateTime.Now:HH:mm:ss}";
        }
    }
    private void LlenarFiltro(ComboBox combo,string campoId,string campoNombre,string todos)
    {
        if(todosEspacios==null)return;int anterior=combo.SelectedValue is int id?id:0;var tabla=new DataTable();tabla.Columns.Add("Id",typeof(int));tabla.Columns.Add("Nombre");tabla.Rows.Add(0,todos);
        foreach(var grupo in todosEspacios.AsEnumerable().GroupBy(r=>(int)r[campoId]).OrderBy(g=>(string)g.First()[campoNombre]))tabla.Rows.Add(grupo.Key,grupo.First()[campoNombre]);
        if(combo.DataSource is DataTable existente && existente.AsEnumerable().Select(r=>$"{r["Id"]}:{r["Nombre"]}").SequenceEqual(tabla.AsEnumerable().Select(r=>$"{r["Id"]}:{r["Nombre"]}")))return;
        combo.ValueMember="Id";combo.DisplayMember="Nombre";combo.DataSource=tabla;combo.SelectedValue=tabla.AsEnumerable().Any(r=>(int)r["Id"]==anterior)?anterior:0;
    }
    private void AplicarFiltros()
    {
        if(todosEspacios==null || filtroZona.SelectedValue is not int zona || filtroTipo.SelectedValue is not int tipo)return;
        var filtrado=Consultas.FiltrarEspacios(todosEspacios,zona,tipo,filtroEstado.SelectedIndex==0?"":filtroEstado.Text);
        Enlazar(espacios,filtrado.DefaultView.ToTable(false,"Id","Codigo","Zona","Tipo","Estado","Placa","Entrada"));informacionEspacios.Text=$"Mostrando {filtrado.Rows.Count} de {todosEspacios.Rows.Count}";
    }
    private void AplicarBusqueda()
    {
        if(todosActivos==null)return;var filtrados=Consultas.BuscarActivos(todosActivos,buscarVehiculo.Text);Enlazar(activos,filtrados);
        informacionActivos.Text=$"{filtrados.Rows.Count} vehículo(s) · Doble clic para ver el ticket. Estimados en {moneda} cada 15 segundos.";
    }
    private void ConsultarHistorial(){string placa=placaHistorial.Text.Trim();if(placa.Length<3)throw new Exception("Escriba la placa completa.");ultimaPlaca=placa.ToUpperInvariant();CargarHistorial();}
    private void CargarHistorial()
    {
        var datos=Consultas.Historial(ultimaPlaca);Enlazar(historial,datos);
        int abiertas=datos.AsEnumerable().Count(r=>r.IsNull("Salida"));decimal pagado=datos.AsEnumerable().Sum(r=>r.Field<decimal?>("Total")??0);
        informacionHistorial.Text=datos.Rows.Count==0?$"No hay visitas registradas para {ultimaPlaca}.":$"{ultimaPlaca} · Visitas: {datos.Rows.Count} · Dentro: {abiertas} · Total pagado: {moneda} {pagado:N2}";
    }
    private static void Enlazar(DataGridView grid,DataTable tabla)
    {
        int? seleccionado=grid.CurrentRow?.Cells["Id"].Value is int id?id:null;grid.DataSource=tabla;
        if(seleccionado!=null){foreach(DataGridViewRow fila in grid.Rows)if(Convert.ToInt32(fila.Cells["Id"].Value)==seleccionado){var primera=grid.Columns.Cast<DataGridViewColumn>().FirstOrDefault(c=>c.Visible);if(primera!=null)grid.CurrentCell=fila.Cells[primera.Index];break;}}
    }
}

}
