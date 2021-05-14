using Aplicacion.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interfaces
{
    public interface IProductosServicio
    {
        Task<List<ProductoDto>> ObtenerProductosExistentes();
        Task<ProductoDto> ObtenerProducto(Guid id);
    }
}
