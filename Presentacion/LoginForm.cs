using System.ComponentModel;
using Parqueo.Negocio;
namespace Parqueo
{
[System.ComponentModel.DesignerCategory("Form")]
public partial class LoginForm : System.Windows.Forms.Form
{
    private bool inicial;
    public LoginForm()
    {
        InitializeComponent();
    }
    private void LoginForm_Shown(object? sender,EventArgs e){if(DesignMode || LicenseManager.UsageMode==LicenseUsageMode.Designtime)return;Comprobar();}
    private void Comprobar()=>Controles.Intentar(()=>{inicial=Consultas.SinUsuarios();entrar.Text=inicial?"Crear administrador":"Iniciar sesión";estado.Text=inicial?"Primer uso: cree su usuario administrador (8 caracteres o más).":"Ingrese sus credenciales.";});
    private void Entrar_Click(object? sender,EventArgs e)=>Controles.Intentar(()=>{if(inicial)Seguridad.CrearAdministrador(usuario.Text,clave.Text);if(!Seguridad.Login(usuario.Text,clave.Text))throw new Exception("Usuario o contraseña incorrectos.");DialogResult=DialogResult.OK;Close();});
}

}
