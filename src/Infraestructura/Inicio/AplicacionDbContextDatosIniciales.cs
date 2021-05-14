using Dominio.Entidades;
using Infraestructura.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Inicio
{
    public class AplicacionDbContextDatosIniciales
    {
        public static async Task InsertDatosEjemplo(AplicacionDbContext context)
        {
            if (!context.Bodegas.Any())
            {
                List<Bodega> bodegasDefault = new List<Bodega>
                {
                    new Bodega
                    {
                        Id = Guid.Parse("40e05749-8b35-42c3-b68e-1232da850baf"),
                        Nombre = "Bodega 1",
                        Descripcion = "Bodega de prueba 1",
                        Ubicacion = "Ubicacion 1",
                        CapacidadMaxima = 100
                    },
                    new Bodega
                    {
                        Id = Guid.Parse("03d734ad-4bd8-444f-b9dd-d497d1fa3974"),
                        Nombre = "Bodega 2",
                        Descripcion = "Bodega de prueba 2",
                        Ubicacion = "Ubicacion 1",
                        CapacidadMaxima = 150
                    },
                    new Bodega
                    {
                        Id = Guid.Parse("460f54b0-0555-4ae5-a119-47cffd8b5d33"),
                        Nombre = "Bodega 3",
                        Descripcion = "Bodega de prueba 3",
                        Ubicacion = "Ubicacion 2",
                        CapacidadMaxima = 200
                    }
                };
                context.Bodegas.AddRange(bodegasDefault);
            }

            if (!context.Productos.Any())
            {
                List<Producto> productosDefault = new List<Producto>
                {
                    new Producto
                    {
                        Id = Guid.Parse("d73b1452-fc54-4920-a796-a10f25b48715"),
                        Nombre = "Producto de prueba 1",
                        Descripcion = "Esto es un producto de prueba 1",
                        sku = "PRO-PRU-1"
                    },
                    new Producto
                    {
                        Id = Guid.Parse("9ae171ea-c81a-454c-8dd8-c5d0ca433f5c"),
                        Nombre = "Producto de prueba 2",
                        Descripcion = "Esto es un producto de prueba 2",
                        sku = "PRO-PRU-2"
                    },
                    new Producto
                    {
                        Id = Guid.Parse("81eafc34-4146-468b-b830-30010ed840f6"),
                        Nombre = "Producto de prueba 3",
                        Descripcion = "Esto es un producto de prueba 3",
                        sku = "PRO-PRU-3"
                    }
                };
                context.Productos.AddRange(productosDefault);
            }

            await context.SaveChangesAsync();
        }
    }
}
