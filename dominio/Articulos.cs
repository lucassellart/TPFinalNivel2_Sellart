using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Articulos  // La clase debe ser 'public' para poder se accedida desde otro proyecto como "winform-app" o "negocio"
    {
        // Propiedades de los objetos:

        public int Id { get; set; }
        public string CodArticulo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Marcas Marca { get; set; }
        public Categorias Categoria { get; set; }
        public string UrlImagen { get; set; }
        public decimal Precio { get; set; }
    }
}
