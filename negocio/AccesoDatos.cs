using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;

namespace negocio
{
    public class AccesoDatos        // Centralizar conexión a DB y métodos necesarios
    {
        // Objetos para establecer la conexión:

        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;


    }
}
