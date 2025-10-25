using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;     // Librería para hacer la conexión a la base de datos
using dominio;
using System.Windows.Forms;

namespace negocio
{
    public class AccesoDatos
    {
        // Atributos: (son los objetos que necesito para establecer la conexión a la DB)

        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        // Voy a crear la propiedad para poder leer el atributo 'lector' desde el exterior:

        public SqlDataReader Lector
        {
            get { return lector; }      // Retorno el lector
        }

        // Constructor: (inicializo los atributos)

        public AccesoDatos()
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true; trustServerCertificate=true;");
            comando = new SqlCommand();
        }

        // Métodos:

        public void setearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;         // La consulta es de tipo texto
            comando.CommandText = consulta;                             // Le paso la consulta al comando
        }

        public void ejectuarLectura()
        {
            comando.Connection = conexion;

            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void ejecutarAccion()                // Método para insertar un registro (nuevo Artículo) en DB:
        {
            comando.Connection = conexion;          // Le paso la conexión al comando
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();          // Ejecuto la consulta de inserción´. Acá no hay que leer registros, solo insertar
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);         // Nombre de la variable y su valor (elegido por el usuario)
        }

        public object ejecutarScalar()         // Devuelvo un único valor (Id recién insertado en DB) para nueva Marca / Categoría
        {
            comando.Connection = conexion;
            try
            {
                if (conexion.State != System.Data.ConnectionState.Open)
                    conexion.Open();

                return comando.ExecuteScalar(); // puede ser null o DBNull.Value si la consulta no trae valor
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void cerrarConexion()
        {
            if (lector != null)
            {
                lector.Close();     // Cierro el lector
            }

            conexion.Close();
        }
    }
}
