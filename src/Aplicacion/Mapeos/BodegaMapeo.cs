using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entidades;
using Aplicacion.Dto;

namespace Aplicacion.Mapeos
{
    public class BodegaMapeo
    {
        public static BodegaDto Convertir(Bodega entidad)
        {
            if (entidad != null)
                return CrearDto(entidad);
            else
                return null;
        }

        public static List<BodegaDto> Convertir(List<Bodega> lista)
        {
            if (lista != null)
                return lista.Select(s => CrearDto(s)).ToList();
            else
                return null;
        }

        private static BodegaDto CrearDto(Bodega entidad)
        {
            return new BodegaDto
            {
                Id = entidad.Id,
                Nombre = entidad.Nombre,
                Descripcion = entidad.Descripcion,
                Ubicacion = entidad.Ubicacion,
                CapacidadMaxima = entidad.CapacidadMaxima
            };
        }

        public static Bodega Convertir(BodegaDto dto)
        {
            if (dto != null)
                return CrearEntidad(dto);
            else
                return null;
        }

        public static List<Bodega> Convertir(List<BodegaDto> lista)
        {
            if(lista != null )
                return lista.Select(s => CrearEntidad(s)).ToList();
            else
                return null;
        }

        private static Bodega CrearEntidad(BodegaDto dto)
        {
            return new Bodega
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Ubicacion = dto.Ubicacion,
                CapacidadMaxima = dto.CapacidadMaxima
            };
        }
    }
}
