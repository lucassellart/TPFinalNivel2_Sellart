using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace presentacion
{
    public partial class frmArticulos : Form
    {
        private List<Articulos> listaArticulos;     // Atributo privado para poder manipular los articulos antes de pasarlos al DataGridView
                                                    // También la voy a usar para filtrar Articulos

        public frmArticulos()
        {
            InitializeComponent();
        }

        private void frmArticulos_Load(object sender, EventArgs e)      // Evento que carga el formulario
        {                                                               // Uso try-catch para excepciones (imagen nula)
            try
            {
                cargar();

                cboCampo.Items.Add("Nombre");
                cboCampo.Items.Add("Marca");
                cboCampo.Items.Add("Categoria");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulos seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;      // Selecciono el articulo de la grilla, fila actual, elemento enlazado

                cargarImagen(seleccionado.ImagenUrl);                                           // Cargar la imagen del articulo seleccionado
            }
            
        }

        private void cargar()           // Método para cargar la lista de Artículos
        {
            ArticuloNegocio negocio = new ArticuloNegocio();        // Voy a invocar la lectura a la base de datos

            try
            {
                listaArticulos = negocio.listar();

                dgvArticulos.DataSource = listaArticulos;

                ocultarColumnas();
                
                cargarImagen(listaArticulos[0].ImagenUrl);          // Cargo la imagen del primer artículo cuando se carga el formulario

                // negocio.listar(); Va a la base de datos y me devuelve una lista de datos (lista de Artículos)
                // dgvArticulos.DataSource -> DataSource recibe esos datos y los modela en la tabla (dgvArticulos)

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
                pbxArticulos.Load(imagen);      // Cargar la imagen del articulo seleccionado
            }
            catch (Exception)
            {

                pbxArticulos.Load("https://developers.elementor.com/docs/assets/img/elementor-placeholder-image.png");
            }

        }
        
        private void ocultarColumnas()
        {
            dgvArticulos.Columns["ImagenUrl"].Visible = false;      // Al cargar el formulario, oculto la columna de la URL de la imagen en el DataGridView
            dgvArticulos.Columns["Id"].Visible = false;             // Oculto la columna del Id del Articulo en el DataGridView
            dgvArticulos.Columns["Precio"].Visible = false;

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Al clickear el botón 'Agregar', voy a abrir el formulario de agregar un nuevo Artículo:

            frmAltaArticulos nuevo = new frmAltaArticulos();
            nuevo.ShowDialog();

            // Acá voy a actualizar la carga del nuevo artículo
            // Una vez que el usuario clickea 'Aceptar' se muestra el nuevo Artículo en el DataGridView

            cargar();       // Con este método debería aparecer el Artículo que el usuario agrega en el DataGridView

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulos seleccionado;
            seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;        // El Artículo que selecciono de la grilla

            // Al clickear el botón 'Modificar', voy a abrir el formulario de modificar un nuevo Artículo:

            frmAltaArticulos modificar = new frmAltaArticulos(seleccionado);        // A este formulario le voy a pasar por parámetro el objeto Artículo que quiero modificar
            modificar.ShowDialog();

            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulos seleccionado;

            try
            {
                DialogResult respuesta = MessageBox.Show("¿Deseas eliminar el artículo de forma permanente?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado.Id);
                    cargar();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool validarFiltro()
        {
            if (cboCampo.SelectedIndex < 0)         // No hay nada seleccionado en el Campo
            {
                MessageBox.Show("Por favor, seleccione el campo para filtrar.");
                return true;
            }

            if (cboCriterio.SelectedIndex < 0)      // No hay nada seleccionado en el Criterio
            {
                MessageBox.Show("Por favor, seleccione el criterio para filtrar.");
                return true;
            }
            
            if (!(soloLetras(txtFiltroAvanzado.Text)))
            {
                MessageBox.Show("Ingrese solo letras para filtrar por favor.");
                return true;
            }
            

            return false;
        }

        private bool soloLetras(string cadena)      // Método para validar que el usuario escriba solamente letras
        {
            foreach (char caracter in cadena)        // for-each para recorrer la cadena
            {
                if (!(char.IsLetter(caracter)))
                    return false;
            }
            return true;
        }

        private void btnFiltro_Click(object sender, EventArgs e)        // Botón buscar:
        {                                                               // Evento para filtrar (base de datos)
            ArticuloNegocio negocio = new ArticuloNegocio();            // Capturar los 3 filtros (campo, criterio y filtro)
            try
            {
                if (validarFiltro())
                {
                    return;         // Cancela la ejecución del evento
                }

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;

                dgvArticulos.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)      // Evento para filtrar rápido
        {
            List<Articulos> listaFiltrada;      // La voy a obtener de una lista que voy a filtrar, NO la instancio acá

            string filtro = txtFiltro.Text;

            if (filtro.Length >= 3)
            {
                listaFiltrada = listaArticulos.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Categoria.Descripcion.ToUpper().Contains(filtro.ToUpper()));
                // El método 'FindAll' me devuelve todos los objetos que cumplen con un criterio de búsqueda
            }
            else
            {
                listaFiltrada = listaArticulos;     // Si el usuario no filtra nada, en la grilla aparecerán todos los artículos
            }

            // Ya con la lista filtrada, le paso los objetos encontrados al datasource:

            dgvArticulos.DataSource = null;      // Primero pongo el DataSource en nulo para que no se repita la lista
            dgvArticulos.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();       // En base a lo seleccionado en 'Campo' voy a cargar las opciones posibles en 'Criterio'

            cboCriterio.Items.Clear();

            cboCriterio.Items.Add("Comienza con:");
            cboCriterio.Items.Add("Termina con:");
            cboCriterio.Items.Add("Contiene:");
        }

        private void btnVerDetalle_Click(object sender, EventArgs e)
        {
            Articulos seleccionado;
            seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;

            frmVerDetalle Detalle = new frmVerDetalle(seleccionado);
            Detalle.ShowDialog();

            cargar();
        }

    }
}
