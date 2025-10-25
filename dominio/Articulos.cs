using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Articulos         // La clase debe ser 'public' para poder se accedida desde otro proyecto como "presentación" o "negocio"
    {
        // Propiedades:
        public int Id { get; set; }

        [DisplayName("Cod. Artículo")]
        public string CodArticulo { get; set; }     
        public string Nombre { get; set; }

        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        public Marcas Marca { get; set; }

        [DisplayName("Categoría")]
        public Categorias Categoria { get; set; }
        public string ImagenUrl { get; set; }
        public decimal Precio { get; set; }
    }
}
