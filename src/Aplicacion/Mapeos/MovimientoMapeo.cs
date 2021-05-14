using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entidades;
using Aplicacion.Dto;

namespace Aplicacion.Mapeos
{
    public class MovimientoMapeo
    {
        public static MovimientoDto Convertir(Movimientos entidad)
        {
            return CrearDto(entidad);
        }

        private static MovimientoDto CrearDto(Movimientos entidad)
        {
            return new MovimientoDto {
                Id = entidad.Id,
                Bodega = BodegaMapeo.Convertir(entidad.Bodega),
                Producto = ProductoMapeo.Convertir(entidad.Producto),
                FechaMovimiento = entidad.FechaMovimiento,
                Cantidad = entidad.Cantidad,
                Tipo = entidad.Tipo,
                Valor = entidad.Valor
            };
        }

        public static Movimientos Convertir(MovimientoDto entidad)
        {
            return CrearEntidad(entidad);
        }

        private static Movimientos CrearEntidad(MovimientoDto dto)
        {
            return new Movimientos
            {
                Id = dto.Id,
                Bodega = BodegaMapeo.Convertir(dto.Bodega),
                Producto = ProductoMapeo.Convertir(dto.Producto),
                FechaMovimiento = dto.FechaMovimiento,
                Cantidad = dto.Cantidad,
                Tipo = dto.Tipo,
                Valor = dto.Valor
            };
        }
    }
}
