using Aplicacion.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Comun.Modelos
{
    public class ResultadoInventarioProducto
    {
        public ProductoDto Producto { get; set; }
        public int CantidadTotal { get; set; }
        public decimal ValorTotal { get; set; }
        public List<InventarioDto> Detalle { get; set; }

    }
}
