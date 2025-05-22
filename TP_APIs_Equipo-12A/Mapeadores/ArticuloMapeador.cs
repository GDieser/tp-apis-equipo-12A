using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Mapeadores
{
    public static class ArticuloMapeador
    {

        public static Articulo DTOaArticulo(ArticuloDTO art)
        {
            Articulo articulo = new Articulo();

            articulo.IdArticulo = art.IdArticulo;
            articulo.Codigo = art.Codigo;
            articulo.Nombre = art.Nombre;
            articulo.Descripcion = art.Descripcion;

            Marca marca = new Marca();
            marca.IdMarca = art.IdMarca;
            articulo.Marca = marca;

            Categoria categoria = new Categoria();
            categoria.IdCategoria = art.IdCategoria;
            articulo.Categoria = categoria;

            articulo.Precio = art.Precio;
            foreach (var imagen in art.Imagenes)
            {
                Imagen img = new Imagen();
                img.ImagenUrl = imagen.ImagenURL;
                img.IdArticulo = articulo.IdArticulo;
                articulo.Imagenes.Add(img);
            }

            return articulo;
        }

    }
}
