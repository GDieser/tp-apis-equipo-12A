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
        public void Post([FromBody]string value)
        {


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
