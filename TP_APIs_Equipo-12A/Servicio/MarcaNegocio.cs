using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Servicio
{
    public class MarcaNegocio
    {

        private AccesoDatos datos;


        public List<Marca> listar()
        {
            List<Marca> marcas = new List<Marca>();

            try
            {
                datos = new AccesoDatos();
                datos.setConsulta("Select id, descripcion FROM marcas");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Marca marca = new Marca();
                    marca.IdMarca = (int)datos.Lector["id"];
                    marca.Descripcion = (string)datos.Lector["descripcion"];

                    marcas.Add(marca);
                }
                return marcas;
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

        public void agregarMarca(Marca marca)
        {
            AccesoDatos datos = new AccesoDatos();

            string consulta = "INSERT INTO MARCAS(Descripcion) VALUES (@descripcion)";

            try
            {
                datos.setConsulta(consulta);

                datos.setParametro("@descripcion", marca.Descripcion);
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

        public void modificarMarca(Marca marca)
        {
            AccesoDatos datos = new AccesoDatos();

            string consulta = "UPDATE MARCAS SET Descripcion = @descripcion WHERE Id = @id";

            try
            {
                datos.setConsulta(consulta);

                datos.setParametro("@descripcion", marca.Descripcion);
                datos.setParametro("@id", marca.IdMarca);
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

        public void eliminarMarca(int marca)
        {
            AccesoDatos datos = new AccesoDatos();

            string consulta = "DELETE FROM MARCAS WHERE Id = @id";

            try
            {
                datos.setConsulta(consulta);
                datos.setParametro("@id", marca);
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
