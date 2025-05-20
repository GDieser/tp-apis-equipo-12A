using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Servicio
{
    public class CategoriaNegocio
    {

        private AccesoDatos datos;


        public List<Categoria> listar()
        {
            List<Categoria> categorias = new List<Categoria>();

            try
            {
                datos = new AccesoDatos();
                datos.setConsulta("Select id, descripcion FROM categorias");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Categoria categoria = new Categoria();
                    categoria.IdCategoria = (int)datos.Lector["id"];
                    categoria.Descripcion = (string)datos.Lector["descripcion"];

                    categorias.Add(categoria);
                }
                return categorias;
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

        public void agregarCategoria(Categoria categoria)
        {
            AccesoDatos datos = new AccesoDatos();

            string consulta = "INSERT INTO CATEGORIAS(Descripcion) VALUES (@descripcion)";

            try
            {
                datos.setConsulta(consulta);

                datos.setParametro("@descripcion", categoria.Descripcion);
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

        public void modificarCategoria(Categoria categoria)
        {
            AccesoDatos datos = new AccesoDatos();

            string consulta = "UPDATE CATEGORIAS SET Descripcion = @descripcion WHERE Id = @id";

            try
            {
                datos.setConsulta(consulta);

                datos.setParametro("@descripcion", categoria.Descripcion);
                datos.setParametro("@id", categoria.IdCategoria);
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

        public void eliminarCategoria(int categoria)
        {
            AccesoDatos datos = new AccesoDatos();

            string consulta = "DELETE FROM CATEGORIAS WHERE Id = @id";

            try
            {
                datos.setConsulta(consulta);
                datos.setParametro("@id", categoria);
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
    }

}



