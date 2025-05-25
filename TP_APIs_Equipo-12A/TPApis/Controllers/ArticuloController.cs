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
        //Listar v2
        [ResponseType(typeof(IEnumerable<Articulo>))]
        public IHttpActionResult Get()
        {
            try
            {
                ArticuloNegocio negocio = new ArticuloNegocio();
                List<Articulo> articulos = negocio.listar();

                if (articulos == null)
                {
                    return NotFound();
                }

                return Ok(articulos);

            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }
        }

        // GET: api/Articulo/5
        /*public Articulo Get(int id)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            List<Articulo> articulos = negocio.listar();

            return articulos.Find(x=> x.IdArticulo == id);
        }*/
        public HttpResponseMessage Get(int id)
        {

            ArticuloNegocio artNegocio = new ArticuloNegocio();

            Articulo art = artNegocio.getArticulo(id);

            if (art == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, " el ID no existe ");
            }
            return Request.CreateResponse(HttpStatusCode.OK, art);

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
            if (string.IsNullOrWhiteSpace(art.Codigo) ||
               string.IsNullOrWhiteSpace(art.Nombre) ||
               string.IsNullOrWhiteSpace(art.Descripcion))
                return BadRequest("Todos los campos obligatorios deben estar completos.");
            
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (negocio.existeArticulo(art.Codigo) !=0)
                    return BadRequest("Existe articulo con este codigo, verifique e intente nuevamente.");
                negocio.agregar(art);
                return Content(HttpStatusCode.OK, new { message = "Articulo agregado correctamente." });
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Ocurrio un error inesperado." });
            }
        }

        // PUT: api/Articulo/{id}
        public IHttpActionResult Put([FromBody] ArticuloDTO art, int id)
        {
            if (art == null)
                return BadRequest("El cuerpo de la solicitud no puede ser nulo.");

            if (id <= 0)
                return BadRequest("El ID proporcionado no es valido.");

            if (string.IsNullOrWhiteSpace(art.Codigo) ||
                string.IsNullOrWhiteSpace(art.Nombre) ||
                string.IsNullOrWhiteSpace(art.Descripcion))
                return BadRequest("Todos los campos obligatorios deben estar completos.");

            art.IdArticulo = id;
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                int idArticuloExistente = negocio.existeArticulo(art.Codigo);

                if (idArticuloExistente != art.IdArticulo && idArticuloExistente != 0)
                    return BadRequest($"Ya existe otro artículo (ID = {idArticuloExistente}) con el mismo código. Verifique e intente nuevamente.");
                if (!negocio.modificarArticulo(art))
                    return BadRequest("Error al modificar el articulo.");
                return Content(HttpStatusCode.OK, new { message = $"Articulo ID={id} modificado correctamente." });
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Ocurrió un error inesperado." });
            }
        }

        // DELETE: api/Articulo/5
        public void Delete(int id)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            negocio.eliminarArticulo(id);
        }
    }
}
