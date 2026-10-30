#nullable disable
namespace Parqueo
{

partial class IngresoForm
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
            this.panelIngreso = new System.Windows.Forms.Panel();
            this.lblplaca = new System.Windows.Forms.Label();
            this.placa = new System.Windows.Forms.TextBox();
            this.lblvehiculo = new System.Windows.Forms.Label();
            this.vehiculo = new System.Windows.Forms.ComboBox();
            this.lbltipo = new System.Windows.Forms.Label();
            this.tipo = new System.Windows.Forms.ComboBox();
            this.lblconvenio = new System.Windows.Forms.Label();
            this.convenio = new System.Windows.Forms.ComboBox();
            this.lblevento = new System.Windows.Forms.Label();
            this.evento = new System.Windows.Forms.ComboBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.panelIngreso.SuspendLayout();
            this.panelIngreso.Name = "panelIngreso";
            this.lblplaca.Name = "lblplaca";
            this.placa.Name = "placa";
            this.lblvehiculo.Name = "lblvehiculo";
            this.vehiculo.Name = "vehiculo";
            this.lbltipo.Name = "lbltipo";
            this.tipo.Name = "tipo";
            this.lblconvenio.Name = "lblconvenio";
            this.convenio.Name = "convenio";
            this.lblevento.Name = "lblevento";
            this.evento.Name = "evento";
            this.btnGuardar.Name = "btnGuardar";
            this.btnCancelar.Name = "btnCancelar";
            this.panelIngreso.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelIngreso.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.panelIngreso.Padding = new System.Windows.Forms.Padding(25);
            this.lblplaca.Text = "Placa";
            this.lblplaca.AutoSize = true;
            this.lblplaca.Location = new System.Drawing.Point(28, 24);
            this.lblplaca.ForeColor = System.Drawing.Color.FromArgb(82, 101, 121);
            this.placa.Width = 415;
            this.placa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.placa.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.placa.Location = new System.Drawing.Point(28, 52);
            this.placa.MaxLength = 15;
            this.placa.CharacterCasing = CharacterCasing.Upper;
            this.lblvehiculo.Text = "Tipo de vehículo";
            this.lblvehiculo.AutoSize = true;
            this.lblvehiculo.Location = new System.Drawing.Point(28, 98);
            this.lblvehiculo.ForeColor = System.Drawing.Color.FromArgb(82, 101, 121);
            this.vehiculo.Width = 415;
            this.vehiculo.DropDownStyle = ComboBoxStyle.DropDownList;
            this.vehiculo.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.vehiculo.Location = new System.Drawing.Point(28, 126);
            this.lbltipo.Text = "Tipo de espacio";
            this.lbltipo.AutoSize = true;
            this.lbltipo.Location = new System.Drawing.Point(28, 172);
            this.lbltipo.ForeColor = System.Drawing.Color.FromArgb(82, 101, 121);
            this.tipo.Width = 415;
            this.tipo.DropDownStyle = ComboBoxStyle.DropDownList;
            this.tipo.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.tipo.Location = new System.Drawing.Point(28, 200);
            this.lblconvenio.Text = "Convenio opcional";
            this.lblconvenio.AutoSize = true;
            this.lblconvenio.Location = new System.Drawing.Point(28, 246);
            this.lblconvenio.ForeColor = System.Drawing.Color.FromArgb(82, 101, 121);
            this.convenio.Width = 415;
            this.convenio.DropDownStyle = ComboBoxStyle.DropDownList;
            this.convenio.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.convenio.Location = new System.Drawing.Point(28, 274);
            this.lblevento.Text = "Evento opcional";
            this.lblevento.AutoSize = true;
            this.lblevento.Location = new System.Drawing.Point(28, 320);
            this.lblevento.ForeColor = System.Drawing.Color.FromArgb(82, 101, 121);
            this.evento.Width = 415;
            this.evento.DropDownStyle = ComboBoxStyle.DropDownList;
            this.evento.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.evento.Location = new System.Drawing.Point(28, 348);
            this.btnGuardar.Text = "Registrar ingreso";
            this.btnGuardar.AutoSize = true;
            this.btnGuardar.MinimumSize = new System.Drawing.Size(90, 38);
            this.btnGuardar.Margin = new System.Windows.Forms.Padding(5);
            this.btnGuardar.Padding = new System.Windows.Forms.Padding(12, 5, 12, 5);
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(0, 112, 105);
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGuardar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(215, 225, 233);
            this.btnGuardar.Location = new System.Drawing.Point(28, 415);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.AutoSize = true;
            this.btnCancelar.MinimumSize = new System.Drawing.Size(90, 38);
            this.btnCancelar.Margin = new System.Windows.Forms.Padding(5);
            this.btnCancelar.Padding = new System.Windows.Forms.Padding(12, 5, 12, 5);
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.BackColor = System.Drawing.Color.White;
            this.btnCancelar.ForeColor = System.Drawing.Color.FromArgb(30, 45, 61);
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(215, 225, 233);
            this.btnCancelar.Location = new System.Drawing.Point(310, 415);
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AcceptButton = this.btnGuardar;
            this.CancelButton = this.btnCancelar;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Controls.Add(this.panelIngreso);
            this.panelIngreso.Controls.Add(this.lblplaca);
            this.panelIngreso.Controls.Add(this.placa);
            this.panelIngreso.Controls.Add(this.lblvehiculo);
            this.panelIngreso.Controls.Add(this.vehiculo);
            this.panelIngreso.Controls.Add(this.lbltipo);
            this.panelIngreso.Controls.Add(this.tipo);
            this.panelIngreso.Controls.Add(this.lblconvenio);
            this.panelIngreso.Controls.Add(this.convenio);
            this.panelIngreso.Controls.Add(this.lblevento);
            this.panelIngreso.Controls.Add(this.evento);
            this.panelIngreso.Controls.Add(this.btnGuardar);
            this.panelIngreso.Controls.Add(this.btnCancelar);
            this.btnGuardar.Click += Guardar_Click;
            this.Shown += IngresoForm_Shown;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(480, 540);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.ForeColor = System.Drawing.Color.FromArgb(30, 45, 61);
            this.Name = "IngresoForm";
            this.Text = "Ingreso de vehículo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.panelIngreso.ResumeLayout(false);
            this.panelIngreso.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
    }

    private System.Windows.Forms.Panel panelIngreso;
    private System.Windows.Forms.Label lblplaca;
    private System.Windows.Forms.TextBox placa;
    private System.Windows.Forms.Label lblvehiculo;
    private System.Windows.Forms.ComboBox vehiculo;
    private System.Windows.Forms.Label lbltipo;
    private System.Windows.Forms.ComboBox tipo;
    private System.Windows.Forms.Label lblconvenio;
    private System.Windows.Forms.ComboBox convenio;
    private System.Windows.Forms.Label lblevento;
    private System.Windows.Forms.ComboBox evento;
    private System.Windows.Forms.Button btnGuardar;
    private System.Windows.Forms.Button btnCancelar;
}
}
