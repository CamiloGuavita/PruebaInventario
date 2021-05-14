using Aplicacion.Dto;
using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Mapeos
{
    public class InventarioMapeo
    {
        public static InventarioDto Convertir(Inventario entidad)
        {
            if (entidad != null)
                return CrearDto(entidad);
            else
                return null;
        }

        public static List<InventarioDto> Convertir(List<Inventario> lista)
        {
            if (lista != null)
                return lista.Select(s => CrearDto(s)).ToList();
            else
                return null;
        }

        private static InventarioDto CrearDto(Inventario entidad)
        {
            return new InventarioDto
            {
                Id = entidad.Id,
                Bodega = BodegaMapeo.Convertir(entidad.Bodega),
                Producto = ProductoMapeo.Convertir(entidad.Producto),
                Cantidad = entidad.Cantidad,
                ValorAcumulado = entidad.ValorAcumulado,
                Creacion = entidad.Creacion,
                UltimaModificacion = entidad.UltimaModificacion
            };
        }

        public static Inventario Convertir(InventarioDto dto)
        {
            if (dto != null)
                return CrearEntidad(dto);
            else
                return null;
        }

        public static List<Inventario> Convertir(List<InventarioDto> lista)
        {
            if (lista != null)
                return lista.Select(s => CrearEntidad(s)).ToList();
            else
                return null;
        }

        private static Inventario CrearEntidad(InventarioDto dto)
        {
            return new Inventario
            {
                Id = dto.Id,
                BodegaId = dto.Bodega.Id,
                Bodega = BodegaMapeo.Convertir(dto.Bodega),
                ProductoId = dto.Producto.Id,
                Producto = ProductoMapeo.Convertir(dto.Producto),
                Cantidad = dto.Cantidad,
                ValorAcumulado = dto.ValorAcumulado,
                Creacion = dto.Creacion,
                UltimaModificacion = dto.UltimaModificacion
            };
        }
    }
}
