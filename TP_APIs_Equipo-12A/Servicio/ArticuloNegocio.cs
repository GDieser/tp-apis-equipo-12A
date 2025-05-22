using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dominio;
using static System.Net.Mime.MediaTypeNames;

namespace Servicio
{
    public class ArticuloNegocio
    {

        public List<Articulo> listar()
        {
            List<Articulo> articulos = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            ImagenNegocio im = new ImagenNegocio();

            try
            {
                datos.setConsulta("select a.id, a.Codigo, a.Descripcion, a.Nombre, a.Precio, m.Id as \"IdMarca\", m.Descripcion as \"marca\", c.Id as \"IdCategoria\", c.Descripcion as \"categoria\" from ARTICULOS a left join MARCAS m ON m.Id = a.IdMarca left join CATEGORIAS c ON c.Id = a.IdCategoria;");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo art = new Articulo();
                    Categoria cat = new Categoria();
                    Marca marca = new Marca();
                    List<Imagen> imagenes = new List<Imagen>();

                    art.IdArticulo = (int)datos.Lector["Id"];
                    art.Codigo = (string)datos.Lector["Codigo"];
                    art.Nombre = (string)datos.Lector["Nombre"];
                    art.Descripcion = (string)datos.Lector["Descripcion"];
                    art.Precio = (decimal)datos.Lector["Precio"];

                    art.Categoria = new Categoria();
                    if (!(datos.Lector["IdCategoria"] is DBNull))
                    {
                        art.Categoria.IdCategoria = (int)datos.Lector["IdCategoria"];
                        art.Categoria.Descripcion = (string)datos.Lector["categoria"];
                    }
                    else
                    {
                        //art.Marca.IdMarca = 0;
                        //art.Marca.Descripcion = "Sin categoria";
                        art.Categoria.IdCategoria = 0;
                        art.Categoria.Descripcion = "Sin categoría";
                    }

                    art.Marca = new Marca();
                    if (!(datos.Lector["IdMarca"] is DBNull))
                    {
                        art.Marca.IdMarca = (int)datos.Lector["IdMarca"];
                        art.Marca.Descripcion = (string)datos.Lector["marca"];
                    }
                    else
                    {
                        art.Marca.IdMarca = 0;
                        art.Marca.Descripcion = "Sin marca";
                    }

                    art.Imagenes = im.getImagenesIdArticulo(art.IdArticulo);


                    articulos.Add(art);
                }



                return articulos;



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
        public void agregar(Articulo nuevoArt)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setConsulta("insert into ARTICULOS (Codigo, Nombre, Descripcion, Precio, IdMarca, IdCategoria) values ('" + nuevoArt.Codigo + "', '" + nuevoArt.Nombre + "', '" + nuevoArt.Descripcion + "',  " + nuevoArt.Precio + ", @IdMarca, @IdCategoria); SELECT SCOPE_IDENTITY()");
                datos.setParametro("@IdMarca", nuevoArt.Marca.IdMarca);
                datos.setParametro("@IdCategoria", nuevoArt.Categoria.IdCategoria);

                datos.ejecutarLectura();

                if (datos.Lector.Read())
                    nuevoArt.IdArticulo = Convert.ToInt32(datos.Lector[0]);

                datos.cerrarConexion();

                string consultaImagen = "INSERT INTO IMAGENES (IdArticulo, ImagenUrl) VALUES (@idArticulo, @imagenUrl)";
                datos.setConsulta(consultaImagen);
                datos.setParametro("@idArticulo", nuevoArt.IdArticulo);
                datos.setParametro("@imagenUrl", nuevoArt.UrlImagen);
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

        public void agregar(ArticuloDTO nuevoArt)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setConsulta(@"INSERT INTO ARTICULOS 
                            (Codigo, Nombre, Descripcion, Precio, IdMarca, IdCategoria) 
                            VALUES 
                            (@Codigo, @Nombre, @Descripcion, @Precio, @IdMarca, @IdCategoria); 
                            SELECT SCOPE_IDENTITY()");

                datos.setParametro("@Codigo", nuevoArt.Codigo);
                datos.setParametro("@Nombre", nuevoArt.Nombre);
                datos.setParametro("@Descripcion", nuevoArt.Descripcion);
                datos.setParametro("@Precio", nuevoArt.Precio);
                datos.setParametro("@IdMarca", nuevoArt.IdMarca);
                datos.setParametro("@IdCategoria", nuevoArt.IdCategoria);

                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    nuevoArt.IdArticulo = Convert.ToInt32(datos.Lector[0]);

                    datos.Lector.Close();
                    foreach (ImagenDTO im in nuevoArt.Imagenes)
                    {
                        datos.setConsulta("INSERT INTO IMAGENES (IdArticulo, ImagenUrl) VALUES (@idArticulo, @imagenUrl)");
                        datos.limpiarParametros();
                        datos.setParametro("@idArticulo", nuevoArt.IdArticulo);
                        datos.setParametro("@imagenUrl", im.ImagenURL);
                        datos.ejecutarAccion();
                    }
                }
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



        public void modificarArticulo(Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();

            string consulta = "UPDATE ARTICULOS SET Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @idMarca, IdCategoria = @idCategoria, Precio = @precio WHERE Id = @id";

            try
            {
                datos.setConsulta(consulta);
                datos.setParametro("@codigo", articulo.Codigo);
                datos.setParametro("@nombre", articulo.Nombre);
                datos.setParametro("@descripcion", articulo.Descripcion);
                datos.setParametro("@idMarca", articulo.Marca.IdMarca);
                datos.setParametro("@idCategoria", articulo.Categoria.IdCategoria);
                datos.setParametro("@precio", articulo.Precio);
                datos.setParametro("@id", articulo.IdArticulo);

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

        public void eliminarArticulo(int idArticulo)
        {
            AccesoDatos datos = new AccesoDatos();

            string consulta = "DELETE FROM ARTICULOS WHERE Id = @id";
            string eliminarImg = "DELETE FROM IMAGENES WHERE IdArticulo = @idArticulo";
            try
            {
                datos.setConsulta(eliminarImg);
                datos.setParametro("@idArticulo", idArticulo);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                datos.setConsulta(consulta);
                datos.setParametro("@id", idArticulo);
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

        public List<Articulo> filtrarMarca(int marca, int categoria)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            ImagenNegocio imagen = new ImagenNegocio();

            string consulta;
            try
            {
                if (marca != 0 && categoria == 0)
                {
                    consulta = "SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, A.IdMarca, A.IdCategoria, A.Precio, M.Descripcion Marca, C.Descripcion Categoria FROM ARTICULOS A LEFT JOIN MARCAS M ON M.Id = A.IdMarca LEFT JOIN CATEGORIAS C ON C.Id = A.IdCategoria WHERE IdMarca = " + marca + "";
                }
                else if (marca != 0 && categoria != 0)
                {
                    consulta = "SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, A.IdMarca, A.IdCategoria, A.Precio, M.Descripcion Marca, C.Descripcion Categoria FROM ARTICULOS A LEFT JOIN MARCAS M ON M.Id = A.IdMarca LEFT JOIN CATEGORIAS C ON C.Id = A.IdCategoria WHERE IdMarca = " + marca + " AND IdCategoria = " + categoria + "";
                }
                else
                {
                    consulta = "SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, A.IdMarca, A.IdCategoria, A.Precio, M.Descripcion Marca, C.Descripcion Categoria FROM ARTICULOS A LEFT JOIN MARCAS M ON M.Id = A.IdMarca LEFT JOIN CATEGORIAS C ON C.Id = A.IdCategoria WHERE IdCategoria = " + categoria + "";
                }

                datos.setConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    List<Imagen> imagenes = new List<Imagen>();

                    aux.IdArticulo = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Precio = (decimal)datos.Lector["Precio"];

                    aux.Marca = new Marca();
                    aux.Marca.IdMarca = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = datos.Lector["Marca"] != DBNull.Value ? (string)datos.Lector["Marca"] : "Sin marca";


                    aux.Categoria = new Categoria();
                    aux.Categoria.IdCategoria = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = datos.Lector["Categoria"] != DBNull.Value ? (string)datos.Lector["Categoria"] : "Sin categoría";

                    aux.Imagenes = imagen.getImagenesIdArticulo(aux.IdArticulo);

                    lista.Add(aux);
                }


                return lista;
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

        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            ImagenNegocio imagen = new ImagenNegocio();

            try
            {
                string consulta = "SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, A.IdMarca, A.IdCategoria, A.Precio, M.Descripcion AS Marca, C.Descripcion AS Categoria FROM ARTICULOS A LEFT JOIN MARCAS M ON M.Id = A.IdMarca LEFT JOIN CATEGORIAS C ON C.Id = A.IdCategoria WHERE ";

                if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "A.Nombre LIKE '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "A.Nombre LIKE '%" + filtro + "'";
                            break;
                        default:
                            consulta += "A.Nombre LIKE '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Marca")
                {
                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "M.Descripcion LIKE '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "M.Descripcion LIKE '%" + filtro + "'";
                            break;
                        default:
                            consulta += "M.Descripcion LIKE '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    consulta += "A.Codigo LIKE '%" + filtro + "%'";
                }

                datos.setConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo art = new Articulo();
                    Categoria cat = new Categoria();
                    Marca marca = new Marca();
                    List<Imagen> imagenes = new List<Imagen>();

                    art.IdArticulo = (int)datos.Lector["Id"];
                    art.Codigo = (string)datos.Lector["Codigo"];
                    art.Nombre = (string)datos.Lector["Nombre"];
                    art.Descripcion = (string)datos.Lector["Descripcion"];
                    art.Precio = (decimal)datos.Lector["Precio"];

                    art.Categoria = new Categoria();
                    if (datos.Lector["IdCategoria"] != DBNull.Value)
                    {
                        art.Categoria.IdCategoria = (int)datos.Lector["IdCategoria"];
                    }
                    else
                    {
                        art.Categoria.IdCategoria = 0;
                    }
                    art.Categoria.Descripcion = datos.Lector["Categoria"] != DBNull.Value ? datos.Lector["Categoria"].ToString() : "Sin categoría";

                    art.Marca = new Marca();
                    if (datos.Lector["IdMarca"] != DBNull.Value)
                    {
                        art.Marca.IdMarca = (int)datos.Lector["IdMarca"];
                    }
                    else
                    {
                        art.Marca.IdMarca = 0;
                    }
                    art.Marca.Descripcion = datos.Lector["Marca"] != DBNull.Value ? datos.Lector["Marca"].ToString() : "Sin marca";
                    art.Imagenes = imagen.getImagenesIdArticulo(art.IdArticulo);

                    lista.Add(art);
                }

                return lista;
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

        public Articulo getArticulo(int id)
        {

            AccesoDatos datos = new AccesoDatos();
            Articulo art = new Articulo();
            ImagenNegocio im = new ImagenNegocio();
            try
            {
                datos.setConsulta("select a.id, a.Codigo, a.Descripcion, a.Nombre, a.Precio, m.Id as \"IdMarca\", m.Descripcion as \"marca\", c.Id as \"IdCategoria\", c.Descripcion as \"categoria\" from ARTICULOS a left join MARCAS m ON m.Id = a.IdMarca left join CATEGORIAS c ON c.Id = a.IdCategoria where a.id=@id");
                datos.setParametro("@id", id);
                datos.ejecutarLectura();
                if (datos.Lector.Read())
                {
                    art.IdArticulo = (int)datos.Lector["Id"];
                    art.Codigo = (string)datos.Lector["Codigo"];
                    art.Nombre = (string)datos.Lector["Nombre"];
                    art.Descripcion = (string)datos.Lector["Descripcion"];
                    art.Precio = (decimal)datos.Lector["Precio"];
                    art.Marca = new Marca();
                    if (datos.Lector["IdMarca"] != DBNull.Value)
                    {
                        art.Marca.IdMarca = (int)datos.Lector["IdMarca"];
                        art.Marca.Descripcion = (string)datos.Lector["Marca"];
                    }
                    else
                    {
                        art.Marca.IdMarca = 0;
                        art.Marca.Descripcion = "Sin marca";
                    }
                    art.Categoria = new Categoria();
                    if (datos.Lector["IdCategoria"] != DBNull.Value)
                    {
                        art.Categoria.IdCategoria = (int)datos.Lector["IdCategoria"];
                        art.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    }
                    else
                    {
                        art.Categoria.IdCategoria = 0;
                        art.Categoria.Descripcion = "Sin categoría";
                    }
                    art.Imagenes = im.getImagenesIdArticulo(art.IdArticulo);

                    return art;
                }
                else
                {
                    return null;
                }
                
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

        public bool modificarArticulo(ArticuloDTO art)
        {
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                Articulo articulo = getArticulo(art.IdArticulo);
                if (articulo == null || articulo.IdArticulo == 0)
                    return false;

                string consulta = @"UPDATE ARTICULOS 
                            SET Codigo = @codigo, 
                                Nombre = @nombre, 
                                Descripcion = @descripcion, 
                                IdMarca = @idMarca, 
                                IdCategoria = @idCategoria, 
                                Precio = @precio 
                            WHERE Id = @id";

                accesoDatos.limpiarParametros();
                accesoDatos.setConsulta(consulta);

                accesoDatos.setParametro("@codigo", art.Codigo);
                accesoDatos.setParametro("@nombre", art.Nombre);
                accesoDatos.setParametro("@descripcion", art.Descripcion);
                accesoDatos.setParametro("@idMarca", art.IdMarca);
                accesoDatos.setParametro("@idCategoria", art.IdCategoria);
                accesoDatos.setParametro("@precio", art.Precio);
                accesoDatos.setParametro("@id", art.IdArticulo);

                accesoDatos.ejecutarAccion();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public bool existeArticulo(string codigo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setConsulta("SELECT Id FROM ARTICULOS WHERE Codigo = @codigo");
                datos.setParametro("@codigo", codigo);
                datos.ejecutarLectura();

                return datos.Lector.Read();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

    }
}
