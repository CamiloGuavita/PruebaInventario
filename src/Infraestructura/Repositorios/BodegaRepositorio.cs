using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.Persistencia;
using Aplicacion.Interfaces;
using Aplicacion.Mapeos;
using Aplicacion.Dto;
using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios
{
    public class BodegaRepositorio : IBodegaRepositorio
    {
        private AplicacionDbContext _contexto;

        public BodegaRepositorio(AplicacionDbContext context)
        {
            this._contexto = context;
        }

        public async Task<List<BodegaDto>> ObtenerBodegasExistentes()
        {
            List<Bodega> listaBodegas = await _contexto.Bodegas.ToListAsync();
            if (listaBodegas == null)
                listaBodegas = new List<Bodega>();

            return BodegaMapeo.Convertir(listaBodegas);
        }

        public async Task<BodegaDto> BuscarBodega(Guid id)
        {
            Bodega bodega = await _contexto.Bodegas.FirstOrDefaultAsync(f => f.Id == id);
            return BodegaMapeo.Convertir(bodega);
        }
    }
}
