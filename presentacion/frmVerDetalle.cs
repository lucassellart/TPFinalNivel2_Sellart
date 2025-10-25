using dominio;
using negocio;
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
    public partial class frmVerDetalle : Form
    {
        private Articulos articulo = null;
        public frmVerDetalle()
        {
            InitializeComponent();
        }

        public frmVerDetalle(Articulos nuevo)       // Hago este 2do constructor para tener la opción de pasar por parámetro el artículo del cual quiero ver el detalle
        {
            InitializeComponent();

            this.articulo = nuevo;
        }

        private void frmVerDetalle_Load(object sender, EventArgs e)
        {
            try
            {
                
                if (articulo != null)   // Si el artículo es != de null es porque existe el artículo y quiero ver el detalle
                                        // tengo que precargar los datos
                {                       
                    txtIdDetalle.Text = articulo.Id.ToString();
                    txtCodArticuloDetalle.Text = articulo.CodArticulo;
                    txtNombreDetalle.Text = articulo.Nombre;
                    txtDescripcionDetalle.Text = articulo.Descripcion;

                    txtMarcaDetalle.Text = articulo.Marca.Descripcion;
                    txtCategoriaDetalle.Text = articulo.Categoria.Descripcion;
                    
                    cargarImagen(articulo.ImagenUrl);

                    txtPrecioDetalle.Text = articulo.Precio.ToString("0.00");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cargarImagen(string imagen)        // Método privado para manejar excepciones (URL no válida / imagen vacía / etc)
        {
            try
            {
                pbxArticulosDetalle.Load(imagen);      // Cargar la imagen del articulo seleccionado
            }
            catch (Exception)
            {

                pbxArticulosDetalle.Load("https://developers.elementor.com/docs/assets/img/elementor-placeholder-image.png");
            }

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
