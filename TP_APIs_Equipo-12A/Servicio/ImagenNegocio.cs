using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;


namespace Servicio
{
    public class ImagenNegocio


    {

        public List<Imagen> getImagenesIdArticulo(int id)
        {
            List<Imagen> imagenes = new List<Imagen>(); 
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setConsulta("SELECT id, idArticulo, imagenURL FROM imagenes WHERE idArticulo = @id");
                datos.limpiarParametros();
                datos.setParametro("@id", id);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Imagen im = new Imagen();
                    im.IdImagen = (int)datos.Lector["id"];
                    im.IdArticulo = (int)datos.Lector["idArticulo"];
                    im.ImagenUrl = (string)datos.Lector["imagenURL"];
                    imagenes.Add(im);
                }
                return imagenes;
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

        public void agregarImagen(Imagen img)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setConsulta("INSERT INTO imagenes (idArticulo, imagenURL) VALUES (@idArticulo, @imagenURL)");
                datos.limpiarParametros();
                datos.setParametro("@idArticulo", img.IdArticulo);
                datos.setParametro("@imagenURL", img.ImagenUrl);
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

        public void borrarImagen(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setConsulta("DELETE FROM imagenes WHERE id = @id");
                datos.limpiarParametros();
                datos.setParametro("@id", id);
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

        public void editarImagen(Imagen img)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setConsulta("UPDATE imagenes SET imagenURL = @imagenURL WHERE id = @id");
                datos.limpiarParametros();
                datos.setParametro("@imagenURL", img.ImagenUrl);
                datos.setParametro("@id", img.IdImagen);
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


    }
}
