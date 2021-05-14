using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Dto;
using Aplicacion.Interfaces;
using Aplicacion.Mapeos;
using Dominio.Entidades;
using Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios
{
    public class ProductosRepositorio : IProductosRepositorio
    {
        private AplicacionDbContext _contexto;
 
        public ProductosRepositorio(AplicacionDbContext context)
        {
            this._contexto = context;
        }

        public async Task<ProductoDto> BuscarProducto(Guid id)
        {
            Producto producto = await _contexto.Productos.FirstOrDefaultAsync(f => f.Id == id);
            return ProductoMapeo.Convertir(producto);
        }

        public async Task<List<ProductoDto>> ObtenerProductosExistentes()
        {
            List<Producto> listaProductos = await _contexto.Productos.ToListAsync();
            if (listaProductos == null)
                listaProductos = new List<Producto>();

            return ProductoMapeo.Convertir(listaProductos);
        }
    }
}
