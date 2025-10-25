using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Categorias
    {
        // Propiedades:

        public int Id { get; set; }

        public string Descripcion { get; set; }
        public override string ToString()       // Sobreescribo el método ToString para que me devuelva la descripción de la Categoría:
        {
            return Descripcion;                 // Devuelvo la propiedad Descripción
        }
    }
}
