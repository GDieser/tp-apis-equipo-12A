using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Articulo
    {
        public Articulo()
        {
            Imagenes = new List<Imagen>();
        }

        public int IdArticulo { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Marca Marca { get; set; }
        public Categoria Categoria { get; set; }
        public decimal Precio { get; set; }

        public List<Imagen> Imagenes { get; set; }
        public string UrlImagen => Imagenes != null && Imagenes.Count > 0 ? Imagenes[0].ImagenUrl : null;
    }
}
