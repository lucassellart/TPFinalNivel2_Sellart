using dominio;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class MarcaNegocio
    {
        public List<Marcas> listar()        // Método para leer las Marcas de la base de datos:
        {
            List<Marcas> lista = new List<Marcas>();

            AccesoDatos datos = new AccesoDatos();      // Instancio la clase AccesoDatos para poder acceder a la base de datos

            try
            {
                datos.setearConsulta("SELECT Id, Descripcion from MARCAS");
                datos.ejectuarLectura();

                while (datos.Lector.Read())
                {
                    Marcas aux = new Marcas();

                    aux.Id = (int)datos.Lector["Id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            { 
                datos.cerrarConexion(); 
            }
        }

        public int agregar(string descripcion)      // Método para darle posibilidad al usuario de agg. nueva Marca
        {                                           // Recibo por parámetro la Marca que escribe el usuario en el frmAltaSimple
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO MARCAS (Descripcion) VALUES (@desc); SELECT CAST(SCOPE_IDENTITY() AS INT)");      // Selecciona el últ. valor autogenerado (Id de la nueva Marca)
                datos.setearParametro("@desc", descripcion);

                // Debe devolver el Id (int) recién insertado
                int id = (int)datos.ejecutarScalar();
                return id;
            }
            catch (SqlException ex)
            {
                throw new Exception("No se pudo agregar la marca. Detalle: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
