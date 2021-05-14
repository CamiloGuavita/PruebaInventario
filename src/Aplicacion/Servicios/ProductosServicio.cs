using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Dto;
using Aplicacion.Interfaces;

namespace Aplicacion.Servicios
{
    public class ProductosServicio : IProductosServicio
    {
        private IProductosRepositorio _repositorio;
        public ProductosServicio(IProductosRepositorio repositorio)
        {
            this._repositorio = repositorio;
        }

        public async Task<ProductoDto> ObtenerProducto(Guid id)
        {
            ProductoDto producto = await _repositorio.BuscarProducto(id);
            return producto;
        }

        public async Task<List<ProductoDto>> ObtenerProductosExistentes()
        {
            List<ProductoDto> lista = await _repositorio.ObtenerProductosExistentes();
            return lista;
        }
    }
}
