using dominio;
using Microsoft.Extensions.Options;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Globalization;
using System.Security.Cryptography;

namespace presentacion
{
    public partial class frmAltaArticulos : Form
    {
        private Articulos articulo = null;

        private OpenFileDialog archivo = null;

        private const string URL_PLACEHOLDER = "https://developers.elementor.com/docs/assets/img/elementor-placeholder-image.png";
        public frmAltaArticulos()
        {
            InitializeComponent();
            
            InicializarValidacionEnVivo();
        }

        public frmAltaArticulos(Articulos artModificado)     // Hago este 2do constructor para tener la opción de pasar por parámetro un Artículo a modificar
        {
            InitializeComponent();

            InicializarValidacionEnVivo();

            this.articulo = artModificado;
            Text = "Modificar Artículo";        // Cambio el título del formulario al modificar un Artículo
        }

        private void frmAltaArticulos_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();

            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();

            try
            {

                cboMarca.DataSource = marcaNegocio.listar();        // 'DataSource' es para visualizar datos que vienen del exterior (lista, DB, etc)

                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";             // 'DisplayMember' es para mostrar el nombre de la marca en el ComboBox

                cboCategoria.DataSource = categoriaNegocio.listar();

                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                if (articulo != null)   // Si el artículo es != de null es porque voy a modificar el artículo, tengo que precargar los datos
                {                       // Estoy precargando el artículo en el método Modificar
                    txtCodArticulo.Text = articulo.CodArticulo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtImagenUrl.Text = articulo.ImagenUrl;
                    cargarImagen(articulo.ImagenUrl);

                    cboMarca.SelectedValue = articulo.Marca.Id;
                    cboCategoria.SelectedValue = articulo.Categoria.Id;

                    txtPrecio.Text = articulo.Precio.ToString("0.00");
                
                } 
                else
                {
                    txtCodArticulo.Clear();
                    txtNombre.Clear();
                    txtDescripcion.Clear();
                    txtImagenUrl.Clear();
                    txtPrecio.Clear();
                }

                    ValidarEnUI();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool validarCampoVacio()        // Se utiliza solo en el evento Click del boton Aceptar
        {

            if (txtCodArticulo.Text == "" || txtNombre.Text == "" || txtDescripcion.Text == "" || txtPrecio.Text == "")
            {
                MessageBox.Show("Por favor, complete los campos obligatorios.");

                txtCodArticulo.BackColor = string.IsNullOrEmpty(txtCodArticulo.Text) ? Color.MistyRose : SystemColors.Window;

                txtNombre.BackColor = string.IsNullOrEmpty(txtNombre.Text) ? Color.MistyRose : SystemColors.Window;

                txtDescripcion.BackColor = string.IsNullOrEmpty(txtDescripcion.Text) ? Color.MistyRose : SystemColors.Window;

                txtPrecio.BackColor = string.IsNullOrEmpty(txtPrecio.Text) ? Color.MistyRose : SystemColors.Window;

                return true;        // hay error
            }

            // Todo OK

            txtCodArticulo.BackColor = txtNombre.BackColor = txtDescripcion.BackColor = txtPrecio.BackColor = SystemColors.Window;

            return false;
        }

        private void InicializarValidacionEnVivo()      // Acá conecto los eventos TextChanged con el validador en vivo
        {                                               

            txtCodArticulo.TextChanged  += ValidarEnUI_Handler;
            txtNombre.TextChanged       += ValidarEnUI_Handler;
            txtDescripcion.TextChanged  += ValidarEnUI_Handler;
            txtPrecio.TextChanged       += ValidarEnUI_Handler;

        }
        private void ValidarEnUI_Handler(object sender, EventArgs e)    // Hace de puente entre los eventos TextChanged y la lógica de validación
        {
            ValidarEnUI();
        }

        private void ValidarEnUI()      // Método para pintar los textbox en función si el usuario escribe o deja vacio
        {                               // También  para habilitar / deshabilitar boton aceptar

            bool okCod = !string.IsNullOrWhiteSpace(txtCodArticulo.Text);
            bool okNom = !string.IsNullOrWhiteSpace(txtNombre.Text);
            bool okDesc = !string.IsNullOrWhiteSpace(txtDescripcion.Text);
            bool okPrec = !string.IsNullOrWhiteSpace(txtPrecio.Text);

            txtCodArticulo.BackColor    = okCod ? SystemColors.Window : Color.MistyRose;
            txtNombre.BackColor         = okNom ? SystemColors.Window : Color.MistyRose;
            txtDescripcion.BackColor    = okDesc ? SystemColors.Window : Color.MistyRose;
            txtPrecio.BackColor         = okPrec ? SystemColors.Window : Color.MistyRose;

            btnAceptar.Enabled = okCod && okNom && okDesc && okPrec;
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imagen))
                {
                    pbxAltaArticulos.Load(URL_PLACEHOLDER);     // En caso de que la imagen sea nula, vacía o con espacios: se usa placeholder de respaldo
                    return;
                }
                pbxAltaArticulos.Load(imagen);
            }
            catch
            {
                pbxAltaArticulos.Load(URL_PLACEHOLDER);     
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";

            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtImagenUrl.Text = archivo.FileName;

                cargarImagen(archivo.FileName);

            }
        }
        
        private void txtImagenUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagenUrl.Text);
        }   // Cuando el usuario sale del textbox me previsualiza la imagen

        private static bool EsUrl(string s)     // Método para chequear si la url viene de la WEB o de un archivo local
        {                                       // Devuelve 'true' si es url web

            return s.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                   s.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizarRutaImagen(string s, string carpetaImagenes)        // Busco estandarizar lo que el usuario escribe en txtImagenUrl para usarlo de = manera después
        {
            // Si viene null, vacío o solo espacios, devuelvo string vacío (no hay imagen)
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;

            // Saco espacios al principio y al final
            s = s.Trim();

            // Si es una URL http/https, la devolvemos tal cual (no se normaliza como ruta local)
            if (EsUrl(s)) return s;     

            if (!Path.IsPathRooted(s))      // Detecta si la url NO es ruta absoluta
                s = Path.Combine(carpetaImagenes, s);   // Une el archivo a la carpeta de imágenes de la aplicacion

            // Devuelvo la ruta de forma limpia
            try { return Path.GetFullPath(s); } catch { return s; }
        }

        private bool ImagenUsadaPorOtroArticulo(int idActual, string rutaNormalizada, List<Articulos> existentes, string carpeta)
        {
            // Método para garantizar unicidad y evitar que dos artículos apunten al mismo archivo / URL por error
            // True -> la imagen (rutaNormalizada) ya la utiliza otro artículo

            for (int i = 0; i < existentes.Count; i++)
            {
                Articulos a = existentes[i];
                if (a.Id == idActual) continue;
                string r = NormalizarRutaImagen(a.ImagenUrl, carpeta);
                if (string.Equals(r, rutaNormalizada, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;       // Imágen única, ningún otro art. la utiliza
        }
        
        private void btnAggMarcas_Click(object sender, EventArgs e)
        {
            frmAltaSimple mar = new frmAltaSimple();

            try
            {
                // 1) Si el usuario CANCELA, no hago nada y vuelvo al formulario
                DialogResult dr = mar.ShowDialog();
                if (dr != DialogResult.OK) 
                    return;

                string nuevaDescripcion = mar.ValorIngresado.Trim();       

                // Validar duplicado en combo
                if (MarcaYaExisteEnCombo(nuevaDescripcion))
                {
                    MessageBox.Show("Esa marca ya existe. Por favor ingrese una diferente.");
                    return;
                }

                // Insertar en BD
                MarcaNegocio negocio = new MarcaNegocio();
                int idNueva = negocio.agregar(nuevaDescripcion);

                // Recargar combo y seleccionar la nueva
                RecargarComboMarca(idNueva);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible agregar la Marca. " + ex.Message);
            }
            
        }

        private bool MarcaYaExisteEnCombo(string descripcion)
        {
            string desc = (descripcion ?? "").Trim();       // Operador de coalescencia nula: se usa la 'descripcion' si no es nula

            for (int i = 0; i < cboMarca.Items.Count; i++)  // Recorro las marcas que existen
            {
                Marcas m = cboMarca.Items[i] as Marcas;
                if (m != null && string.Equals(m.Descripcion, desc, StringComparison.OrdinalIgnoreCase))
                    return true;        // La nueva marca ya existe en el combobox
            }
            return false;       // La nueva marca no existe aún
        }

        private void RecargarComboMarca(int idSeleccionar)
        {
            MarcaNegocio negocio = new MarcaNegocio();
            List<Marcas> lista = negocio.listar();

            cboMarca.DataSource = null;
            cboMarca.ValueMember = "Id";
            cboMarca.DisplayMember = "Descripcion";
            cboMarca.DataSource = lista;

            cboMarca.SelectedValue = idSeleccionar;
        }

        private void btnAggCategorias_Click(object sender, EventArgs e)
        {
            frmAltaSimple cat = new frmAltaSimple();

            try
            {
                // 1) Si el usuario CANCELA, no hago nada y vuelvo al formulario
                DialogResult dr = cat.ShowDialog();
                if (dr != DialogResult.OK)
                    return;

                string nuevaDescripcion = cat.ValorIngresado.Trim();

                // Validar duplicado en combo
                if (CategoriaYaExisteEnCombo(nuevaDescripcion))
                {
                    MessageBox.Show("Esa categoría ya existe. Por favor ingrese una diferente.");
                    return;
                }

                // Insertar en BD
                CategoriaNegocio negocio = new CategoriaNegocio();
                int idNueva = negocio.agregar(nuevaDescripcion);

                // Recargar combo y seleccionar la nueva
                RecargarComboCategoria(idNueva);
            }
            catch (Exception ex)
            {

                MessageBox.Show("No fue posible agregar la Categoría. " + ex.Message);
            }
        }

        private bool CategoriaYaExisteEnCombo(string descripcion)
        {
            string desc = (descripcion ?? "").Trim();

            for (int i = 0; i < cboCategoria.Items.Count; i++)
            {
                Categorias c = cboCategoria.Items[i] as Categorias;
                if (c != null && string.Equals(c.Descripcion, desc, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private void RecargarComboCategoria(int idSeleccionar)
        {
            CategoriaNegocio negocio = new CategoriaNegocio();
            List<Categorias> lista = negocio.listar();

            cboCategoria.DataSource = null;
            cboCategoria.ValueMember = "Id";
            cboCategoria.DisplayMember = "Descripcion";
            cboCategoria.DataSource = lista;

            cboCategoria.SelectedValue = idSeleccionar;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            // Para realizar acciones contra la DB 
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                if (articulo == null)       // Si es null, no hay Id -> nuevo artículo
                    articulo = new Articulos();

                // Validaciones de campos obligatorios
                if (validarCampoVacio())
                    return;

                // Asignación de campos
                articulo.CodArticulo = txtCodArticulo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Marca = (Marcas)cboMarca.SelectedItem;
                articulo.Categoria = (Categorias)cboCategoria.SelectedItem;
                articulo.Precio = decimal.Parse(txtPrecio.Text);

                // === IMAGEN: placeholder + unicidad + borrado/copia ===

                // Leo la carpeta de imágenes definida en app config
                string carpeta = ConfigurationManager.AppSettings["images-folder"]; // ej: "C:\\Imagenes\\"
                
                List<Articulos> existentes = negocio.listar();

                // Detecto si el texto es URL o archivo local
                bool esUrl = EsUrl(txtImagenUrl.Text);
                
                // Si el usuario eligió archivo local, preparo destino
                string nuevaImagen = (archivo != null && !esUrl)
                    ? Path.Combine(carpeta, archivo.SafeFileName)   // archivo local a copiar
                    : (txtImagenUrl.Text ?? "").Trim();             // URL escrita o vacío


                // Banderas para ver si el string es vacio o es placeholder por defecto
                bool esVacia = string.IsNullOrWhiteSpace(nuevaImagen);
                bool esPH = string.Equals(nuevaImagen, URL_PLACEHOLDER, StringComparison.OrdinalIgnoreCase);

                // 1) Unicidad: si está vacía o es placeholder, no se valida
                string candidatoNorm = NormalizarRutaImagen(nuevaImagen, carpeta);
                
                if (!(esVacia || esPH))
                {
                    if (ImagenUsadaPorOtroArticulo(articulo.Id, candidatoNorm, existentes, carpeta))
                    {
                        MessageBox.Show("Ya existe otro artículo con esa imagen. Elegí una distinta.");
                        
                        return;     // Corto el flujo para que el usuario cambie la imagen
                    }
                }

                // 2) Si MODIFICA (Id != 0) y la imagen anterior era LOCAL y cambia o se deja vacía/placeholder, borrarla si nadie más la usa
                if (articulo.Id != 0)
                {
                    string anteriorNorm = NormalizarRutaImagen(articulo.ImagenUrl, carpeta);
                    
                    // Chequeo si la imagen anterior era local y diferente a la actual
                    bool anteriorEsLocal = !string.IsNullOrWhiteSpace(anteriorNorm) && !EsUrl(anteriorNorm);
                    bool esDistinta = !string.Equals(anteriorNorm, candidatoNorm, StringComparison.OrdinalIgnoreCase);

                    if (anteriorEsLocal && (esVacia || esPH || esDistinta))
                    {
                        // ImagenUsadaPorOtroArticulo(): me aseguro que ningún otro artículo use esa imagen
                        if (!ImagenUsadaPorOtroArticulo(articulo.Id, anteriorNorm, existentes, carpeta) && File.Exists(anteriorNorm))
                        {
                            try { File.Delete(anteriorNorm); } catch { }
                        }
                    }
                }

                // 3) Asignación final + copia si es local
                if (esVacia || esPH)
                {
                    articulo.ImagenUrl = ""; // guardo vacío; el placeholder se muestra en UI
                }
                else if (archivo != null && !esUrl)
                {
                    string destino = NormalizarRutaImagen(nuevaImagen, carpeta);
                    
                    if (File.Exists(destino)) { try { File.Delete(destino); } catch { } }
                    
                    File.Copy(archivo.FileName, destino, false);
                    
                    articulo.ImagenUrl = destino;
                }
                else
                {
                    articulo.ImagenUrl = nuevaImagen; // URL real
                }
                // === FIN IMAGEN ===

                // Persistencia
                if (articulo.Id != 0)
                {
                    negocio.modificar(articulo);

                    MessageBox.Show("Artículo modificado existosamente!");

                }
                else
                {
                    negocio.agregar(articulo);

                    MessageBox.Show("Artículo agregado existosamente!");
                }

                Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, ingrese números para completar el campo Precios.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
