using Aplicacion.Dto;
using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Mapeos
{
    public class ProductoMapeo
    {
        public static ProductoDto Convertir(Producto entidad)
        {
            if (entidad != null)
                return CrearDto(entidad);
            else
                return null;
        }

        public static List<ProductoDto> Convertir(List<Producto> lista)
        {
            if (lista != null)
                return lista.Select(s => CrearDto(s)).ToList();
            else
                return null;
        }

        private static ProductoDto CrearDto(Producto entidad)
        {
            return new ProductoDto
            {
                Id = entidad.Id,
                Nombre = entidad.Nombre,
                Descripcion = entidad.Descripcion,
                sku = entidad.sku
            };
        }


        public static Producto Convertir(ProductoDto dto)
        {
            if (dto != null)
                return CrearEntidad(dto);
            else
                return null;
        }

        public static List<Producto> Convertir(List<ProductoDto> lista)
        {
            if (lista != null)
                return lista.Select(s => CrearEntidad(s)).ToList();
            else
                return null;
        }

        private static Producto CrearEntidad(ProductoDto dto)
        {
            return new Producto
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                sku = dto.sku
            };
        }


    }
}
