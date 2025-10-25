using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Azure.Identity;
using dominio;
using Microsoft.Data.SqlClient;

namespace negocio
{
    public class ArticuloNegocio
    {
        public List<Articulos> listar()             // Método para leer registros de la DB
        {
            List<Articulos> lista = new List<Articulos>();

            AccesoDatos datos = new AccesoDatos();      // Instancio la clase AccesoDatos para poder acceder a la base de datos

            try
            {
                datos.setearConsulta("SELECT A.Id, Codigo, Nombre, A.Descripcion, A.IdMarca, A.IdCategoria, M.Descripcion AS Marca, C.Descripcion AS Categoria,  ImagenUrl, Precio FROM ARTICULOS A, MARCAS M, CATEGORIAS C WHERE A.IdMarca = M.Id AND A.IdCategoria = C.Id");
                datos.ejectuarLectura();

                
                while (datos.Lector.Read())                 // Mientras se lea un registro en la base de datos:
                {
                    Articulos aux = new Articulos();        // Articulo auxiliar para cargar los datos del registro (de la base de datos)

                    aux.Id = (int)datos.Lector["Id"];
                    aux.CodArticulo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    
                    // Validaciones en caso de columnas nulas:
                    
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                    {
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];      // Cargo la URL de la imagen del Artículo
                    }

                    if (!(datos.Lector["Precio"] is DBNull))
                    {
                        aux.Precio = (decimal)datos.Lector["Precio"];           // Cargo el precio del Artículo
                    }
                    
                    

                    // Voy a cargar las marcas de los artículos:
                    aux.Marca = new Marcas();      // Creo una instancia de la clase Marcas
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];         // Tengo que acceder al ID de la Marca, no del Artículo
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];

                    // Voy a cargar las categorías de los artículos:
                    aux.Categoria = new Categorias();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];     // Tengo que acceder al ID de la Categoria, no del Artículo
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];


                    lista.Add(aux);         // Agrego el artículo a la lista
                }

                return lista;
            }
            catch (SqlException)
            {
                throw;
            } 
            finally
            {
                datos.cerrarConexion();         // Cierro la conexión a la base de datos
            }
        }
    
        public void agregar(Articulos nuevo)        // Método para agregar nuevos Artículos a la DB
        {
            AccesoDatos datos = new AccesoDatos();  // Voy a utilizar métodos de esta clase para la DB

            try
            {
                datos.setearConsulta("Insert into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio) values ('"+ nuevo.CodArticulo + "', '"+ nuevo.Nombre + "', ' "+ nuevo.Descripcion + " ', @idMarca, @idCategoria, @img, @precio)");

                datos.setearParametro("@idMarca", nuevo.Marca.Id);
                datos.setearParametro("@idCategoria", nuevo.Categoria.Id);
                datos.setearParametro("@img", nuevo.ImagenUrl);
                datos.setearParametro("@precio", nuevo.Precio);

                datos.ejecutarAccion();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                datos.cerrarConexion();
            } 
        }

        public void modificar(Articulos modificar)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE ARTICULOS set Codigo = @codArt, Nombre = @nombre, Descripcion = @desc, IdMarca = @idMarca, IdCategoria = @idCategoria, ImagenUrl = @img, Precio = @precio WHERE Id = @id");
                
                datos.setearParametro("@codArt", modificar.CodArticulo);
                datos.setearParametro("@nombre", modificar.Nombre);
                datos.setearParametro("@desc", modificar.Descripcion);
                datos.setearParametro("@idMarca", modificar.Marca.Id);
                datos.setearParametro("@idCategoria", modificar.Categoria.Id);
                datos.setearParametro("@img", modificar.ImagenUrl);
                datos.setearParametro("@precio", modificar.Precio);
                datos.setearParametro("@id", modificar.Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminar(int id)        // Recibo por parámetro el Id del Artículo a eliminar:
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("DELETE from ARTICULOS WHERE Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public List<Articulos> filtrar(string campo, string criterio, string filtro)        // Método para filtro avanzado (va a la DB y trae registros específicos)
        {
            List<Articulos> lista = new List<Articulos>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "SELECT A.Id, Codigo, Nombre, A.Descripcion, A.IdMarca, A.IdCategoria, M.Descripcion AS Marca, C.Descripcion AS Categoria,  ImagenUrl, Precio FROM ARTICULOS A, MARCAS M, CATEGORIAS C WHERE A.IdMarca = M.Id AND A.IdCategoria = C.Id AND ";

                switch(campo)
                {
                    case "Nombre":

                        switch (criterio) 
                        {
                            case "Comienza con":

                                consulta += "Nombre like '" + filtro + " %'"; 
                                break;

                            case "Termina con":

                                consulta += "Nombre like '%" + filtro + "' ";
                                break;

                            default:

                                consulta += "Nombre like '%" + filtro + "%' ";
                                break;
                        }
                        break;

                    case "Marca":

                        switch (criterio)
                        {
                            case "Comienza con":

                                consulta += "M.Descripcion like '" + filtro + " %'";
                                break;

                            case "Termina con":

                                consulta += "M.Descripcion like '%" + filtro + "' ";
                                break;

                            default:

                                consulta += "M.Descripcion like '%" + filtro + "%' ";
                                break;
                        }
                        break;

                    default:        // Categoria

                        switch (criterio)
                        {
                            case "Comienza con":

                                consulta += "C.Descripcion like '" + filtro + " %'";
                                break;

                            case "Termina con":

                                consulta += "C.Descripcion like '%" + filtro + "' ";
                                break;

                            default:

                                consulta += "C.Descripcion like '%" + filtro + "%' ";
                                break;
                        }
                        break;

                }

                datos.setearConsulta(consulta);

                datos.ejectuarLectura();

                while (datos.Lector.Read())                 // Mientras se lea un registro en la base de datos:
                {
                    Articulos aux = new Articulos();        // Articulo auxiliar para cargar los datos del registro (de la base de datos)

                    aux.Id = (int)datos.Lector["Id"];
                    aux.CodArticulo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    // Validaciones en caso de columnas nulas:

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                    {
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];      // Cargo la URL de la imagen del Artículo
                    }

                    if (!(datos.Lector["Precio"] is DBNull))
                    {
                        aux.Precio = (decimal)datos.Lector["Precio"];           // Cargo el precio del Artículo
                    }



                    // Voy a cargar las marcas de los artículos:
                    aux.Marca = new Marcas();      // Creo una instancia de la clase Marcas
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];         // Tengo que acceder al ID de la Marca, no del Artículo
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];

                    // Voy a cargar las categorías de los artículos:
                    aux.Categoria = new Categorias();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];     // Tengo que acceder al ID de la Categoria, no del Artículo
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];


                    lista.Add(aux);         // Agrego el artículo a la lista
                }

                return lista;
            }
            catch (SqlException)
            {
                throw;
            }
           
        }
    }
}
