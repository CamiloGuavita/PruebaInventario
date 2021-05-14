using Aplicacion.Dto;
using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interfaces
{
    public interface IInventarioRepositorio
    {
        Task<InventarioDto> ObtenerInventario(Guid bodegaId, Guid productoId);
        Task<MovimientoDto> GuardarMovimiento(MovimientoDto movimiento);
        Task ActualizarInvetario(MovimientoDto movimiento);
        Task<List<InventarioDto>> ObtenerInvtarioProducto(Guid productoId);
    }
}
