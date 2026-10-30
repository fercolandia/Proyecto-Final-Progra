using System.Data;
namespace Parqueo;
public static class Controles
{
    private static Dictionary<string,string> textos=new();
    public static void CargarTextos() { textos=Parqueo.Negocio.Consultas.Textos(); }
    public static string Texto(string clave)=>textos.TryGetValue(clave,out var valor)?valor:clave;
    public static void Traducir(Control c) { if(c is Label or Button or TabPage or Form) c.Text=Texto(c.Text); foreach(Control hijo in c.Controls)Traducir(hijo); }

    public static void Intentar(Action accion) { try { accion(); } catch(Exception ex) { File.AppendAllText(Path.Combine(AppContext.BaseDirectory,"errores.log"),DateTime.Now+" "+ex+Environment.NewLine); string mensaje=ex.Message; if((ex.InnerException??ex) is Microsoft.Data.SqlClient.SqlException s) mensaje=s.Number switch {547=>"El registro tiene referencias o datos incompatibles. Revise sus relaciones y fechas.",2601 or 2627=>"Ya existe un registro con esos datos. Revise duplicados y tarifas activas.",_=>s.Message}; MessageBox.Show(mensaje,"Parqueo",MessageBoxButtons.OK,MessageBoxIcon.Warning); } }
    public static void ConfigurarTabla(DataGridView g)
    {
        g.DataBindingComplete+=(_,_)=>{foreach(DataGridViewColumn col in g.Columns){if(col.DataGridView!=g)continue;if(g.AutoGenerateColumns)col.HeaderText=col.Name switch{"Codigo"=>"Código","Direccion"=>"Dirección","MinutosGracia"=>"Gracia en minutos","MinutosUnidad"=>"Minutos por unidad","FueraServicio"=>"Fuera de servicio",_=>col.HeaderText};if(g.AutoGenerateColumns){if(col.Name=="Id"){col.Visible=Equals(g.Tag,"NumeroTicket");if(col.Visible)col.HeaderText="Ticket";}else if(col.Name.EndsWith("Id"))col.Visible=false;}if(col.ValueType==typeof(DateTime))col.DefaultCellStyle.Format="dd/MM/yyyy HH:mm";if(col.ValueType==typeof(decimal)){col.DefaultCellStyle.Format="N2";col.DefaultCellStyle.Alignment=DataGridViewContentAlignment.MiddleRight;}}};
        g.CellFormatting+=(_,e)=>{
            if(e.ColumnIndex<0 || g.Columns[e.ColumnIndex].Name!="Estado" || e.Value==null || SystemInformation.HighContrast)return;
            var (fondo,color)=e.Value.ToString() switch {
                "Disponible"=>(Color.FromArgb(224,243,234),Color.FromArgb(24,100,65)),
                "Ocupado" or "Dentro"=>(Color.FromArgb(226,236,248),Color.FromArgb(36,78,132)),
                "Finalizado"=>(Color.FromArgb(224,243,234),Color.FromArgb(24,100,65)),
                "Reservado"=>(Color.FromArgb(255,242,211),Color.FromArgb(121,80,10)),
                _=>(Color.FromArgb(240,229,232),Color.FromArgb(130,48,60))};
            e.CellStyle!.BackColor=fondo;e.CellStyle.ForeColor=color;
        };
    }
    public static int Id(ComboBox c)=>Convert.ToInt32(c.SelectedValue);
    public static void Ticket(string texto)
    {
        using var formulario=new TicketForm(texto);formulario.ShowDialog();
    }
}
