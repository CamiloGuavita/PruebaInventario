using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Dto;
using Dominio.Entidades;

namespace Aplicacion.Interfaces
{
    public interface IBodegaRepositorio
    {
        Task<List<BodegaDto>> ObtenerBodegasExistentes();
        Task<BodegaDto> BuscarBodega(Guid id);
    }
}
