using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Dominio;
using Servicio;
using TPApis.Models;

namespace TPApis.Controllers
{
    public class ArticuloController : ApiController
    {
        // GET: api/Articulo
        /* Listar v1
        public IEnumerable<Articulo> Get()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            return negocio.listar();
        }
        */
        //Listar v2
        [ResponseType(typeof(IEnumerable<Articulo>))]
        public IHttpActionResult Get()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            List<Articulo> articulos = negocio.listar();

            if (articulos == null)
            {
                return NotFound();
            }

            return Ok(articulos);
        }

        // GET: api/Articulo/5
        public Articulo Get(int id)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            List<Articulo> articulos = negocio.listar();

            return articulos.Find(x=> x.IdArticulo == id);
        }

        // POST: api/Articulo (Imagenes)
        [HttpPost]
        [Route("api/Articulo/{id}/Imagen")]
        public HttpResponseMessage Post([FromBody] List<ImagenDto> imagenes, int id)
        {
            ImagenNegocio negocio = new ImagenNegocio();
            Imagen nuevo = new Imagen();

            ArticuloNegocio artNegocio = new ArticuloNegocio();

            Articulo art = artNegocio.getArticulo(id);

            if (art == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ID de Artículo incorrecto.");
            }

            foreach (var imagen in imagenes)
            {
                if (string.IsNullOrWhiteSpace(imagen.ImagenUrl))
                    continue;

                Imagen nueva = new Imagen();
                nueva.IdArticulo = id;
                nueva.ImagenUrl = imagen.ImagenUrl;

                negocio.agregarImagen(nueva);
            }

            return Request.CreateResponse(HttpStatusCode.OK, "Imagen/es agregada/s con éxito");
        }

        // POST: api/Articulo
        public IHttpActionResult Post([FromBody] ArticuloDTO art)
        {
            if (string.IsNullOrEmpty(art.Codigo) || string.IsNullOrEmpty(art.Nombre) || string.IsNullOrEmpty(art.Descripcion))
                return Content(HttpStatusCode.BadRequest, new { message = "Error en los datos ingresados. Verifique e intente nuevamente..." });
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (negocio.existeArticulo(art.Codigo))
                    return Content(HttpStatusCode.BadRequest, new { message = "Existe articulo con este codigo, verifique e intente nuevamente." });
                negocio.agregar(art);
                return Content(HttpStatusCode.OK, new { message = "Articulo agregado correctamente." });
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Ocurrio un error inesperado." });
            }
        }

        // PUT: api/Articulo
        public IHttpActionResult Put([FromBody] ArticuloDTO art)
        {
            if (art.IdArticulo <= 0 || string.IsNullOrEmpty(art.Codigo) || string.IsNullOrEmpty(art.Nombre) || string.IsNullOrEmpty(art.Descripcion))
                return Content(HttpStatusCode.BadRequest, new { message = "Error en los datos proporcionados. Verifique e intente nuevamente." });
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (!negocio.modificarArticulo(art))
                {
                    return Content(HttpStatusCode.BadRequest, new { message = "Error al modificar el articulo." });
                }
                return Content(HttpStatusCode.OK, new { message = "Articulo modificado correctamente." });
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Ocurrió un error inesperado." });
            }
        }

        // DELETE: api/Articulo/5
        public void Delete(int id)
        {
        }
    }
}
