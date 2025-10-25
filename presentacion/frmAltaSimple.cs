using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace presentacion
{
    public partial class frmAltaSimple : Form
    {
        public string ValorIngresado { get; private set; }      // propiedad para setear y leer la nueva marca / categoría
                                                                // set privado para setear el valor solamente desde este formulario, no desde afuera
        public frmAltaSimple()
        {
            InitializeComponent();
        }

        private void btnAceptarMC_Click(object sender, EventArgs e)
        {
            String texto = txtDescripcionMC.Text.Trim();    // Trim() elimina los espacios en blanco al principio y al final de la cadena
                                                            // Evita errores al comparar o guardar texto

            if (string.IsNullOrWhiteSpace(texto))
            {
                MessageBox.Show("La descripción es obligatoria.");
                txtDescripcionMC.Focus();
                txtDescripcionMC.SelectAll();
                return;
            }
            
            ValorIngresado = texto;     // Acá se setea el valor ingresado por el usuario, la marca o categoría
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        

        private void btnCancelarMC_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;  
            this.Close();
        }
    }
}
