using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Dto;
using Aplicacion.Interfaces;
using Aplicacion.Mapeos;
using Infraestructura.Persistencia;
using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Dominio.Enums;

namespace Infraestructura.Repositorios
{
    public class InventarioRepositorio : IInventarioRepositorio
    {
        private AplicacionDbContext _contexto;

        public InventarioRepositorio(AplicacionDbContext context)
        {
            this._contexto = context;
        }

        public async Task<InventarioDto> ObtenerInventario(Guid bodegaId, Guid productoId)
        {
            Inventario invetarioconsulta = await _contexto.Inventarios.FirstOrDefaultAsync(f => f.BodegaId == bodegaId && f.ProductoId == productoId);
            return InventarioMapeo.Convertir(invetarioconsulta);
        }

        public async Task<MovimientoDto> GuardarMovimiento(MovimientoDto movimiento)
        {
            Movimientos movimientoGuardar = new Movimientos
            {
                Id = movimiento.Id,
                BodegaId = movimiento.Bodega.Id,
                ProductoId = movimiento.Producto.Id,
                FechaMovimiento = movimiento.FechaMovimiento,
                Cantidad = movimiento.Cantidad,
                Tipo = movimiento.Tipo,
                Valor = movimiento.Valor
            };
            _contexto.Movimientos.Add(movimientoGuardar);
            await _contexto.SaveChangesAsync();
            Movimientos movimientoCreado = await _contexto.Movimientos.FirstOrDefaultAsync(f => f.Id == movimiento.Id);
            return MovimientoMapeo.Convertir(movimientoCreado);
        }

        public async Task ActualizarInvetario(MovimientoDto movimiento)
        {
            Inventario inventarioActualizar = await _contexto.Inventarios.FirstOrDefaultAsync(f => f.BodegaId == movimiento.Bodega.Id && f.ProductoId == movimiento.Producto.Id);
            if (inventarioActualizar == null) {
                inventarioActualizar = new Inventario
                {
                    Id = Guid.NewGuid(),
                    BodegaId = movimiento.Bodega.Id,
                    ProductoId = movimiento.Producto.Id,
                    Cantidad = movimiento.Cantidad,
                    ValorAcumulado = movimiento.Valor,
                    Creacion = DateTime.Now,
                    UltimaModificacion = null
                };
                _contexto.Inventarios.Add(inventarioActualizar);
            }
            else
            {
                switch(movimiento.Tipo)
                {
                    case TipoMovimiento.Ingreso:
                        inventarioActualizar.Cantidad += movimiento.Cantidad;
                        inventarioActualizar.ValorAcumulado += movimiento.Valor;
                        break;
                    case TipoMovimiento.Salida:
                        inventarioActualizar.Cantidad -= movimiento.Cantidad;
                        inventarioActualizar.ValorAcumulado -= movimiento.Valor;
                        break;
                }
                inventarioActualizar.UltimaModificacion = DateTime.Now;
            }
            await _contexto.SaveChangesAsync();
        }

        public async Task<List<InventarioDto>> ObtenerInvtarioProducto(Guid productoId)
        {
            List<Inventario> listaInventario = await _contexto.Inventarios
                                                                .Join(_contexto.Bodegas,inv => inv.BodegaId,bod => bod.Id,(inv,bod)=> new {inv , bod })
                                                                .Where(w => w.inv.ProductoId == productoId)
                                                                .Select(s=> new Inventario
                                                                {
                                                                    Id = s.inv.Id,
                                                                    Bodega = new Bodega
                                                                    {
                                                                        Id = s.bod.Id,
                                                                        Nombre = s.bod.Nombre,
                                                                        Descripcion = s.bod.Descripcion,
                                                                        Ubicacion = s.bod.Ubicacion,
                                                                        CapacidadMaxima = s.bod.CapacidadMaxima
                                                                    },
                                                                    Cantidad = s.inv.Cantidad,
                                                                    Producto = null,
                                                                    ValorAcumulado = s.inv.ValorAcumulado,
                                                                    Creacion = s.inv.Creacion,
                                                                    UltimaModificacion =s.inv.UltimaModificacion
                                                                }).ToListAsync();
            if (listaInventario == null)
                listaInventario = new List<Inventario>();

            return InventarioMapeo.Convertir(listaInventario);
        }
    }
}
