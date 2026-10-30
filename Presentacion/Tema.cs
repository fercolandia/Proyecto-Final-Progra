using System.Drawing.Drawing2D;
namespace Parqueo;
// Un solo lugar para cambiar los colores y tamaños de la interfaz.
public static class Tema
{
    public static Color Fondo => SystemInformation.HighContrast ? SystemColors.Window : Color.FromArgb(241,245,249);
    public static Color Texto => SystemInformation.HighContrast ? SystemColors.WindowText : Color.FromArgb(30,45,61);
    public static Color Suave => SystemInformation.HighContrast ? SystemColors.WindowText : Color.FromArgb(82,101,121);
    public static Color Azul => SystemInformation.HighContrast ? SystemColors.Highlight : Color.FromArgb(22,48,72);
    public static Color Acento => SystemInformation.HighContrast ? SystemColors.Highlight : Color.FromArgb(0,112,105);
    public static Color Linea => SystemInformation.HighContrast ? SystemColors.WindowText : Color.FromArgb(215,225,233);
    public static Font Fuente(float size=10,FontStyle estilo=FontStyle.Regular)=>new("Segoe UI",size,estilo);
    public static void Aplicar(Control c)
    {
        if(c is Form f){f.Font=Fuente();f.BackColor=Fondo;f.ForeColor=Texto;f.AutoScaleMode=AutoScaleMode.Dpi;}
        if(c is TabPage pagina){pagina.BackColor=Fondo;pagina.ForeColor=Texto;}
        if(c is TabControl tab) EstilizarTabs(tab);
        if(c is TextBox txt){txt.BorderStyle=BorderStyle.FixedSingle;txt.BackColor=SystemColors.Window;txt.ForeColor=Texto;}
        if(c is ComboBox combo){combo.FlatStyle=FlatStyle.Standard;combo.Font=Fuente();combo.Margin=new Padding(5,7,5,7);}
        if(c is DateTimePicker or NumericUpDown){c.Font=Fuente();c.Margin=new Padding(5,7,5,7);}
        foreach(Control hijo in c.Controls)Aplicar(hijo);
    }
    public static void EstilizarTabs(TabControl tab)
    {
        if(Equals(tab.Tag,"tema"))return;
        tab.Tag="tema";tab.DrawMode=TabDrawMode.OwnerDrawFixed;tab.SizeMode=TabSizeMode.Normal;tab.ItemSize=new Size(130,42);tab.Padding=new Point(20,8);tab.Font=Fuente(10,FontStyle.Bold);
        tab.DrawItem+=(_,e)=>{
            bool activo=e.Index==tab.SelectedIndex;var r=e.Bounds;
            using var fondo=new SolidBrush(activo?SystemColors.Window:Fondo);e.Graphics.FillRectangle(fondo,r);
            TextRenderer.DrawText(e.Graphics,tab.TabPages[e.Index].Text,tab.Font,r,activo?Acento:Suave,TextFormatFlags.HorizontalCenter|TextFormatFlags.VerticalCenter|TextFormatFlags.EndEllipsis);
            if(activo){using var pincel=new SolidBrush(Acento);e.Graphics.FillRectangle(pincel,r.X+12,r.Bottom-4,r.Width-24,3);}
        };
    }
    public static Label Dato(string titulo,Color acento,out Panel tarjeta)
    {
        tarjeta=new Panel{Dock=DockStyle.Fill,BackColor=SystemColors.Window,Margin=new Padding(5),Padding=new Padding(16,10,12,8)};
        var valor=new Label{Text="0",Dock=DockStyle.Fill,TextAlign=ContentAlignment.MiddleLeft,Font=Fuente(23,FontStyle.Bold),ForeColor=acento};
        var nombre=new Label{Text=titulo,Dock=DockStyle.Bottom,Height=23,Font=Fuente(10),ForeColor=Suave};
        var contenido=new TableLayoutPanel{Dock=DockStyle.Fill,ColumnCount=1,RowCount=2,Margin=Padding.Empty};contenido.RowStyles.Add(new RowStyle(SizeType.Percent,60));contenido.RowStyles.Add(new RowStyle(SizeType.Percent,40));valor.Margin=Padding.Empty;nombre.Margin=Padding.Empty;nombre.Dock=DockStyle.Fill;nombre.TextAlign=ContentAlignment.MiddleLeft;contenido.Controls.Add(valor,0,0);contenido.Controls.Add(nombre,0,1);tarjeta.Controls.Add(contenido);
        var panel=tarjeta;tarjeta.Paint+=(_,e)=>{using var brush=new SolidBrush(acento);e.Graphics.FillRectangle(brush,0,0,4,panel.Height);};
        return valor;
    }
    public static Label Titulo(string texto)=>new(){Text=texto,AutoSize=true,Font=Fuente(18,FontStyle.Bold),ForeColor=Azul,Margin=new Padding(0,0,0,8)};
}
