using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Dto
{
    public class InventarioDto
    {
        public Guid Id { get; set; }
        public BodegaDto Bodega { get; set; }
        public ProductoDto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal ValorAcumulado { get; set; }

        public DateTime Creacion { get; set; }
        public DateTime? UltimaModificacion { get; set; }
    }
}
