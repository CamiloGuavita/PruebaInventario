using Aplicacion.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Comun.Modelos
{
    public class MovimientoRequest
    {
        public string BodegaId { get; set; }
        public string ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Valor { get; set; }
    }
}
