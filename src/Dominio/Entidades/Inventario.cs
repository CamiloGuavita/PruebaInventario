using Dominio.Compartido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Inventario : Auditoria
    {
        public Guid Id { get; set; }
        public Guid BodegaId { get; set; }
        public Guid ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal ValorAcumulado { get; set; }

        public Bodega Bodega { get; set; }
        public Producto Producto { get; set; }
    }
}
