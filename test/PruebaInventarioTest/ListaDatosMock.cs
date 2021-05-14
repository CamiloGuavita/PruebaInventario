using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaInventarioTest
{
    public class ListaDatosMock
    {
        public List<Bodega> listaBodegas = new List<Bodega>
        {
            new Bodega
            {
                Id = Guid.Parse("51ed6452-d6b5-4fa6-a1e4-1e1fbd16d72d"),
                Nombre = "Bodega Mock 1",
                Descripcion = "Esta es la bodega Mock 1",
                CapacidadMaxima = 100,
                Ubicacion = "Ubicacion mock 1"
            },
            new Bodega
            {
                Id = Guid.Parse("74a353ec-7b55-4a64-b919-bf6069e6308d"),
                Nombre = "Bodega Mock 2",
                Descripcion = "Esta es la bodega Mock 2",
                CapacidadMaxima = 50,
                Ubicacion = "Ubicacion mock 1"
            }
        };

        public List<Producto> productos = new List<Producto>()
        {
            new Producto
            {
                Id = Guid.Parse("ea9acaa9-adaf-45bd-980d-a1bb8ab7abc3"),
                Nombre = "Producto Mock",
                Descripcion = "Esto es un producto Mock",
                sku = "MOCK-PRO-1"
            }
        };

        public List<Inventario> InventarioLista = new List<Inventario>
        {
            new Inventario
            {
                Id = Guid.Parse("5963605e-71db-4b58-bfee-4bd5eea1f3c3"),
                BodegaId = Guid.Parse("51ed6452-d6b5-4fa6-a1e4-1e1fbd16d72d"),
                ProductoId = Guid.Parse("ea9acaa9-adaf-45bd-980d-a1bb8ab7abc3"),
                Cantidad = 10,
                ValorAcumulado = 100000,
                Creacion = new DateTime(2021,05,3,18,25,0),
                UltimaModificacion = null
            },
            new Inventario
            {
                Id = Guid.Parse("e4108706-6e16-4f5b-b69e-6c094d73b87e"),
                BodegaId = Guid.Parse("74a353ec-7b55-4a64-b919-bf6069e6308d"),
                ProductoId = Guid.Parse("ea9acaa9-adaf-45bd-980d-a1bb8ab7abc3"),
                Cantidad = 20,
                ValorAcumulado = 200000,
                Creacion = new DateTime(2021,05,3,16,15,0),
                UltimaModificacion = new DateTime(2021,05,3,17,15,0),
            }
        };

        public List<Movimientos> listaMovimientos = new List<Movimientos>();
    }
}
