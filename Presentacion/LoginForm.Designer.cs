#nullable disable
namespace Parqueo
{

partial class LoginForm
{
    private System.ComponentModel.IContainer components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            this.panelAcceso = new System.Windows.Forms.Panel();
            this.panelIdentidad = new System.Windows.Forms.Panel();
            this.lblMarca = new System.Windows.Forms.Label();
            this.lblSistema = new System.Windows.Forms.Label();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.lblBienvenido = new System.Windows.Forms.Label();
            this.estado = new System.Windows.Forms.Label();
            this.lblusuario = new System.Windows.Forms.Label();
            this.usuario = new System.Windows.Forms.TextBox();
            this.lblclave = new System.Windows.Forms.Label();
            this.clave = new System.Windows.Forms.TextBox();
            this.entrar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.panelAcceso.SuspendLayout();
            this.panelIdentidad.SuspendLayout();
            this.panelAcceso.Name = "panelAcceso";
            this.panelIdentidad.Name = "panelIdentidad";
            this.lblMarca.Name = "lblMarca";
            this.lblSistema.Name = "lblSistema";
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblBienvenido.Name = "lblBienvenido";
            this.estado.Name = "estado";
            this.lblusuario.Name = "lblusuario";
            this.usuario.Name = "usuario";
            this.lblclave.Name = "lblclave";
            this.clave.Name = "clave";
            this.entrar.Name = "entrar";
            this.panelAcceso.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAcceso.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.panelAcceso.BackColor = System.Drawing.Color.White;
            this.panelIdentidad.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelIdentidad.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.panelIdentidad.Width = 285;
            this.panelIdentidad.BackColor = System.Drawing.Color.FromArgb(22, 48, 72);
            this.lblMarca.Text = "P";
            this.lblMarca.Location = new System.Drawing.Point(32, 48);
            this.lblMarca.Size = new System.Drawing.Size(82, 82);
            this.lblMarca.Font = new System.Drawing.Font("Segoe UI", 42F, System.Drawing.FontStyle.Bold);
            this.lblMarca.ForeColor = System.Drawing.Color.White;
            this.lblSistema.Text = "Sistema\nde parqueo";
            this.lblSistema.Location = new System.Drawing.Point(30, 160);
            this.lblSistema.Size = new System.Drawing.Size(235, 112);
            this.lblSistema.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblSistema.ForeColor = System.Drawing.Color.White;
            this.lblDescripcion.Text = "Control de espacios,\ningresos y cobros.";
            this.lblDescripcion.Location = new System.Drawing.Point(32, 290);
            this.lblDescripcion.Size = new System.Drawing.Size(220, 70);
            this.lblDescripcion.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular);
            this.lblDescripcion.ForeColor = System.Drawing.Color.White;
            this.lblMarca.BackColor = System.Drawing.Color.FromArgb(0, 112, 105);
            this.lblMarca.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblBienvenido.Text = "Bienvenido";
            this.lblBienvenido.Location = new System.Drawing.Point(38, 42);
            this.lblBienvenido.Size = new System.Drawing.Size(355, 42);
            this.lblBienvenido.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblBienvenido.ForeColor = System.Drawing.Color.FromArgb(22, 48, 72);
            this.estado.Text = "Ingrese sus credenciales.";
            this.estado.Location = new System.Drawing.Point(38, 90);
            this.estado.Size = new System.Drawing.Size(355, 58);
            this.estado.ForeColor = System.Drawing.Color.FromArgb(82, 101, 121);
            this.lblusuario.Text = "Usuario";
            this.lblusuario.Location = new System.Drawing.Point(38, 160);
            this.lblusuario.AutoSize = true;
            this.usuario.Width = 355;
            this.usuario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.usuario.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.usuario.Location = new System.Drawing.Point(38, 188);
            this.lblclave.Text = "Contraseña";
            this.lblclave.Location = new System.Drawing.Point(38, 245);
            this.lblclave.AutoSize = true;
            this.clave.Width = 355;
            this.clave.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clave.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.clave.Location = new System.Drawing.Point(38, 273);
            this.clave.UseSystemPasswordChar = true;
            this.entrar.Text = "Iniciar sesión";
            this.entrar.AutoSize = true;
            this.entrar.MinimumSize = new System.Drawing.Size(90, 38);
            this.entrar.Margin = new System.Windows.Forms.Padding(5);
            this.entrar.Padding = new System.Windows.Forms.Padding(12, 5, 12, 5);
            this.entrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.entrar.BackColor = System.Drawing.Color.FromArgb(0, 112, 105);
            this.entrar.ForeColor = System.Drawing.Color.White;
            this.entrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.entrar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(215, 225, 233);
            this.entrar.AutoSize = false;
            this.entrar.Location = new System.Drawing.Point(38, 338);
            this.entrar.Size = new System.Drawing.Size(355, 44);
            this.AcceptButton = this.entrar;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Controls.Add(this.panelAcceso);
            this.Controls.Add(this.panelIdentidad);
            this.panelIdentidad.Controls.Add(this.lblMarca);
            this.panelIdentidad.Controls.Add(this.lblSistema);
            this.panelIdentidad.Controls.Add(this.lblDescripcion);
            this.panelAcceso.Controls.Add(this.lblBienvenido);
            this.panelAcceso.Controls.Add(this.estado);
            this.panelAcceso.Controls.Add(this.lblusuario);
            this.panelAcceso.Controls.Add(this.usuario);
            this.panelAcceso.Controls.Add(this.lblclave);
            this.panelAcceso.Controls.Add(this.clave);
            this.panelAcceso.Controls.Add(this.entrar);
            this.entrar.Click += Entrar_Click;
            this.Shown += LoginForm_Shown;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(780, 510);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.ForeColor = System.Drawing.Color.FromArgb(30, 45, 61);
            this.Name = "LoginForm";
            this.Text = "Parqueo Acceso";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.panelAcceso.ResumeLayout(false);
            this.panelAcceso.PerformLayout();
            this.panelIdentidad.ResumeLayout(false);
            this.panelIdentidad.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
    }

    private System.Windows.Forms.Panel panelAcceso;
    private System.Windows.Forms.Panel panelIdentidad;
    private System.Windows.Forms.Label lblMarca;
    private System.Windows.Forms.Label lblSistema;
    private System.Windows.Forms.Label lblDescripcion;
    private System.Windows.Forms.Label lblBienvenido;
    private System.Windows.Forms.Label estado;
    private System.Windows.Forms.Label lblusuario;
    private System.Windows.Forms.TextBox usuario;
    private System.Windows.Forms.Label lblclave;
    private System.Windows.Forms.TextBox clave;
    private System.Windows.Forms.Button entrar;
}
}
