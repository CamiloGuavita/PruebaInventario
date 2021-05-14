using Aplicacion.Dto;
using Aplicacion.Comun.Modelos;
using Aplicacion.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioController : ControllerBase
    {

        private IInventarioServicio _servicio;

        public InventarioController(IInventarioServicio servicio)
        {
            this._servicio = servicio;
        }

        [HttpGet]
        [Route("ObtenerInvetarioProducto")]
        public async Task<ActionResult<Resultado<ResultadoInventarioProducto>>> ObtenerInvetarioProducto(string idProducto)
        {
            
                List<string> errores = new List<string>();
                if (string.IsNullOrEmpty(idProducto))
                    errores.Add("El id del producto no especificado");

                if (idProducto.Length != 36)
                    errores.Add("El id del producto no tiene la longitud correcta (36 caracteres)");

                if (errores.Count > 0)
                {
                    return BadRequest(Resultado<ProductoDto>.Failure(errores));
                }

                Resultado<ResultadoInventarioProducto> resultado = await _servicio.ObtenerInvetarioProducto(idProducto);
                if (resultado.Correcto)
                {
                    return Ok(resultado);
                }
                else
                {
                    return BadRequest(resultado);
                }
            
        }

        // POST api/<InventarioController>
        [HttpPost]
        [Route("CargarProducto")]
        public async Task<ActionResult<Resultado<MovimientoDto>>> CargarProducto([FromBody] MovimientoRequest movimiento)
        {
            if (movimiento == null)
            {
                return BadRequest(Resultado<MovimientoDto>.Failure("No se encontraron datos para procesar"));
            }

            var resutlado = await _servicio.CargarProducto(movimiento);

            if (resutlado.Correcto)
            {
                return Ok(resutlado);
            }
            else
            {
                return BadRequest(resutlado);
            }
        }

        [HttpPost]
        [Route("SacarProducto")]
        public async Task<ActionResult<Resultado<MovimientoDto>>> SacarProducto([FromBody] MovimientoRequest movimiento)
        {
            if (movimiento == null)
            {
                return BadRequest(Resultado<MovimientoDto>.Failure("No se encontraron datos para procesar"));
            }

            var resutlado = await _servicio.SacarProducto(movimiento);

            if (resutlado.Correcto)
            {
                return Ok(resutlado);
            }
            else
            {
                return BadRequest(resutlado);
            }
        }

        [HttpPost]
        [Route("MoverProductoBodegas")]
        public async Task<ActionResult<Resultado<MovimientoDto>>> MoverProductoBodegas([FromBody] MovimientoBodegasRequest movimiento)
        {
            if (movimiento == null)
            {
                return BadRequest(Resultado<MovimientoDto>.Failure("No se encontraron datos para procesar"));
            }
            var resutlado = await _servicio.MoverProductoBodegas(movimiento);
            if (resutlado.Correcto)
            {
                return Ok(resutlado);
            }
            else
            {
                return BadRequest(resutlado);
            }
        }
    }
}
