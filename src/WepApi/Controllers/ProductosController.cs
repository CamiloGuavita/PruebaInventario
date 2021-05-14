using Aplicacion.Dto;
using Aplicacion.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Comun.Modelos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private IProductosServicio _servicioProductos;

        public ProductosController(IProductosServicio servicio)
        {
            this._servicioProductos = servicio;
        }

        // GET: api/<ProductosController>
        [HttpGet]
        public async Task<ActionResult<List<ProductoDto>>> Get()
        {
            List<ProductoDto> listaProductos = await _servicioProductos.ObtenerProductosExistentes();
            return Ok(listaProductos);
        }

        // GET api/<ProductosController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Resultado<ProductoDto>>> Get(string id)
        {
            List<string> errores = new List<string>();
            if (string.IsNullOrEmpty(id))
                errores.Add("Id no especificado");

            if (id.Length != 36)
                errores.Add("Id no tiene la longitud correcta (36 caracteres)");

            Guid idGuid;
            if (!Guid.TryParse(id,out idGuid))
            {
                errores.Add("Id no tiene el formato correcto xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
            }
            if (errores.Count > 0)
            {
                return BadRequest(Resultado<ProductoDto>.Failure(errores));
            }

            ProductoDto producto = await _servicioProductos.ObtenerProducto(idGuid);
            if(producto != null)
            {
                return Ok(Resultado<ProductoDto>.Success(producto));
            }
            else
            {
                errores.Add("Id no encontrado");
                return NotFound(Resultado<ProductoDto>.Failure(errores));
            }
        }

    }
}
