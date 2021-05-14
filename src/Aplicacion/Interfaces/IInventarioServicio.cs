using Aplicacion.Comun.Modelos;
using Aplicacion.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interfaces
{
    public interface IInventarioServicio
    {
        Task<Resultado<MovimientoDto>> CargarProducto(MovimientoRequest movimiento);
        Task<Resultado<MovimientoDto>> SacarProducto(MovimientoRequest movimiento);
        Task<Resultado<MovimientoDto>> MoverProductoBodegas(MovimientoBodegasRequest movimiento);
        Task<Resultado<ResultadoInventarioProducto>> ObtenerInvetarioProducto(string productoId);
    }
}
