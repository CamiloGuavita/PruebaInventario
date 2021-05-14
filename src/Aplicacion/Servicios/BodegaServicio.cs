using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entidades;
using Aplicacion.Interfaces;
using Aplicacion.Dto;

namespace Aplicacion.Servicios
{
    public class BodegaServicio : IBodegaServicio
    {
        private IBodegaRepositorio _repositorioBodega;

        public BodegaServicio(IBodegaRepositorio repositorioBodega)
        {
            this._repositorioBodega = repositorioBodega;
        }

        public async Task<List<BodegaDto>> ObtenerBodegasExistentes()
        {
            return await this._repositorioBodega.ObtenerBodegasExistentes();
        }
    }
}
