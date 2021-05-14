using Aplicacion.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interfaces
{
    public interface IProductosRepositorio
    {
        Task<List<ProductoDto>> ObtenerProductosExistentes();
        Task<ProductoDto> BuscarProducto(Guid id);
    }
}
