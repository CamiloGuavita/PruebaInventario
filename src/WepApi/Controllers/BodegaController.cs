using Aplicacion.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BodegaController : ControllerBase
    {
        private IBodegaServicio _servicioBodega;

        public BodegaController(IBodegaServicio servicioBodega)
        {
            this._servicioBodega = servicioBodega;
        }

        // GET: api/<BodegaController>
        [HttpGet]
        public async Task<ActionResult<List<BodegaDto>>> Get()
        {
            List<BodegaDto> listaBodegas = await _servicioBodega.ObtenerBodegasExistentes();
            return Ok(listaBodegas);
        }

    }
}
