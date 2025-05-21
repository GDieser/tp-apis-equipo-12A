using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dominio;
using Servicio;

namespace TPApis.Controllers
{
    public class ArticuloController : ApiController
    {
        // GET: api/Articulo
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Articulo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Articulo
        public IHttpActionResult Post([FromBody] ArticuloDTO art)
        {
            if (string.IsNullOrEmpty(art.Codigo) || string.IsNullOrEmpty(art.Nombre))
                return BadRequest("Los campos obligatorios no están completos.");

            ArticuloNegocio negocio = new ArticuloNegocio();

            if (negocio.existeArticulo(art.Codigo))
                return BadRequest("El artículo ya existe.");

            negocio.agregar(art);


            return Ok("Artículo agregado con éxito");
        }

        public IHttpActionResult Put([FromBody] ArticuloDTO art)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            if (!negocio.modificarArticulo(art))
            {
                return BadRequest("Error al modificar el artículo");
            }
            return Ok("Artículo modificado con éxito");
        }

        // DELETE: api/Articulo/5
        public void Delete(int id)
        {
        }
    }
}
