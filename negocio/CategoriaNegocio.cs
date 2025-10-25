using dominio;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class CategoriaNegocio
    {
        public List<Categorias> listar()
        {
            List<Categorias> lista = new List<Categorias>();

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id, Descripcion from CATEGORIAS");
                datos.ejectuarLectura();

                while (datos.Lector.Read())
                {
                    Categorias aux = new Categorias();

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

        public int agregar(string descripcion)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO CATEGORIAS (Descripcion) VALUES (@desc); SELECT CAST(SCOPE_IDENTITY() AS INT);");
                datos.setearParametro("@desc", descripcion);

                int id = (int)datos.ejecutarScalar();
                return id;
            }
            catch (SqlException ex)
            {
                throw new Exception("No se pudo agregar la categoría. Detalle: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
