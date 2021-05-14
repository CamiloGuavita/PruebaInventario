using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Bodega
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Ubicacion { get; set; }
        public int CapacidadMaxima { get; set; }

        public ICollection<Movimientos> Movimientos { get; set; }
        public ICollection<Inventario> Inventario { get; set; }
    }
}
