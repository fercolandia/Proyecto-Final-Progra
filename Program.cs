using Parqueo.Negocio;
namespace Parqueo;
internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        while(true)
        {
            Seguridad.CerrarSesion();
            using var login=new LoginForm();
            if(login.ShowDialog()!=DialogResult.OK)break;
            using var principal=new Form1();
            Application.Run(principal);
            if(!principal.CerrarSesionSolicitado)break;
        }
    }
}
