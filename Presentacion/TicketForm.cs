namespace Parqueo
{
public partial class TicketForm : System.Windows.Forms.Form
{
    public TicketForm()
    {
        InitializeComponent();
    }
    public TicketForm(string texto):this(){textoTicket.Text=texto;}
}

}
