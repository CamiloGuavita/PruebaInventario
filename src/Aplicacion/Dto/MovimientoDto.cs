using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Dto
{
    public class MovimientoDto
    {
        public Guid Id { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public TipoMovimiento Tipo { get; set; }
        public BodegaDto Bodega { get; set; }
        public ProductoDto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Valor { get; set; }
    }
}
