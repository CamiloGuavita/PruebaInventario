using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Movimientos
    {
        public Guid Id { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public TipoMovimiento Tipo { get; set; }
        public Guid BodegaId { get; set; }
        public Guid ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Valor { get; set; }

        public Producto Producto { get; set; }
        public Bodega Bodega { get; set; }
    }
}
