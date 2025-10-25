namespace presentacion
{
    partial class frmAltaSimple
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblDescripcionMC = new System.Windows.Forms.Label();
            this.txtDescripcionMC = new System.Windows.Forms.TextBox();
            this.btnAceptarMC = new System.Windows.Forms.Button();
            this.btnCancelarMC = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblDescripcionMC
            // 
            this.lblDescripcionMC.AutoSize = true;
            this.lblDescripcionMC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescripcionMC.Location = new System.Drawing.Point(38, 72);
            this.lblDescripcionMC.Name = "lblDescripcionMC";
            this.lblDescripcionMC.Size = new System.Drawing.Size(91, 18);
            this.lblDescripcionMC.TabIndex = 0;
            this.lblDescripcionMC.Text = "Descripción:";
            // 
            // txtDescripcionMC
            // 
            this.txtDescripcionMC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescripcionMC.Location = new System.Drawing.Point(150, 72);
            this.txtDescripcionMC.Name = "txtDescripcionMC";
            this.txtDescripcionMC.Size = new System.Drawing.Size(175, 24);
            this.txtDescripcionMC.TabIndex = 1;
            // 
            // btnAceptarMC
            // 
            this.btnAceptarMC.Location = new System.Drawing.Point(50, 191);
            this.btnAceptarMC.Name = "btnAceptarMC";
            this.btnAceptarMC.Size = new System.Drawing.Size(85, 32);
            this.btnAceptarMC.TabIndex = 2;
            this.btnAceptarMC.Text = "Aceptar:";
            this.btnAceptarMC.UseVisualStyleBackColor = true;
            this.btnAceptarMC.Click += new System.EventHandler(this.btnAceptarMC_Click);
            // 
            // btnCancelarMC
            // 
            this.btnCancelarMC.Location = new System.Drawing.Point(218, 191);
            this.btnCancelarMC.Name = "btnCancelarMC";
            this.btnCancelarMC.Size = new System.Drawing.Size(87, 32);
            this.btnCancelarMC.TabIndex = 3;
            this.btnCancelarMC.Text = "Cancelar:";
            this.btnCancelarMC.UseVisualStyleBackColor = true;
            this.btnCancelarMC.Click += new System.EventHandler(this.btnCancelarMC_Click);
            // 
            // frmAltaSimple
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(395, 279);
            this.Controls.Add(this.btnCancelarMC);
            this.Controls.Add(this.btnAceptarMC);
            this.Controls.Add(this.txtDescripcionMC);
            this.Controls.Add(this.lblDescripcionMC);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(413, 326);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(413, 326);
            this.Name = "frmAltaSimple";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Marca - Categoría";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDescripcionMC;
        private System.Windows.Forms.TextBox txtDescripcionMC;
        private System.Windows.Forms.Button btnAceptarMC;
        private System.Windows.Forms.Button btnCancelarMC;
    }
}