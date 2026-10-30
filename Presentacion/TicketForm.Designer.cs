#nullable disable
namespace Parqueo
{

partial class TicketForm
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
            this.textoTicket = new System.Windows.Forms.TextBox();
            this.barraTicket = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.barraTicket.SuspendLayout();
            this.textoTicket.Name = "textoTicket";
            this.barraTicket.Name = "barraTicket";
            this.btnCerrar.Name = "btnCerrar";
            this.textoTicket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textoTicket.Multiline = true;
            this.textoTicket.ReadOnly = true;
            this.textoTicket.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textoTicket.Font = new System.Drawing.Font("Consolas", 11F);
            this.textoTicket.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textoTicket.BackColor = System.Drawing.Color.White;
            this.barraTicket.Dock = System.Windows.Forms.DockStyle.Top;
            this.barraTicket.AutoSize = true;
            this.barraTicket.WrapContents = true;
            this.barraTicket.Padding = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.barraTicket.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.barraTicket.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.AutoSize = true;
            this.btnCerrar.MinimumSize = new System.Drawing.Size(90, 38);
            this.btnCerrar.Margin = new System.Windows.Forms.Padding(5);
            this.btnCerrar.Padding = new System.Windows.Forms.Padding(12, 5, 12, 5);
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.BackColor = System.Drawing.Color.White;
            this.btnCerrar.ForeColor = System.Drawing.Color.FromArgb(30, 45, 61);
            this.btnCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(215, 225, 233);
            this.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton = this.btnCerrar;
            this.Controls.Add(this.textoTicket);
            this.Controls.Add(this.barraTicket);
            this.barraTicket.Controls.Add(this.btnCerrar);
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(560, 570);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.ForeColor = System.Drawing.Color.FromArgb(30, 45, 61);
            this.Name = "TicketForm";
            this.Text = "Ticket y comprobante";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.barraTicket.ResumeLayout(false);
            this.barraTicket.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
    }

    private System.Windows.Forms.TextBox textoTicket;
    private System.Windows.Forms.FlowLayoutPanel barraTicket;
    private System.Windows.Forms.Button btnCerrar;
}
}
