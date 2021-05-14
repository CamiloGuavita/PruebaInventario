using Aplicacion.Dto;
using Aplicacion.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WepApi;
using Dominio.Entidades;
using Aplicacion.Mapeos;
using Aplicacion.Comun.Modelos;
using Dominio.Enums;
using System.Net.Http;

namespace PruebaInventarioTest
{
    [TestClass]
    public class InventarioUnitTest
    {
        private static TestContext _testContext;
        private static WebApplicationFactory<Startup> _factory;

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
            _factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.UseSetting("https_port", "5001").UseEnvironment("Testing");
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<IInventarioRepositorio, MockInventarioRepositorio>();
                    services.AddScoped<IBodegaRepositorio, MockBodegaRepositorio>();
                    services.AddScoped<IProductosRepositorio, MockProductoRepositorio>();
                });
            });
        }

        [TestMethod]
        public async Task CargarProductoTest()
        {
            Console.WriteLine(_testContext.TestName);

            var client = _factory.CreateClient();
            MovimientoRequest movimiento = new MovimientoRequest
            {
                BodegaId = "51ed6452-d6b5-4fa6-a1e4-1e1fbd16d72d",
                ProductoId = "ea9acaa9-adaf-45bd-980d-a1bb8ab7abc3",
                Cantidad = 10,
                Valor = 25000
            };
            StringContent cuerpo = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(movimiento),Encoding.UTF8,"application/json");
            var response = await client.PostAsync("api/Inventario/CargarProducto", cuerpo);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var result = await response.Content.ReadAsStringAsync();
            Resultado<MovimientoDto> resultado = Newtonsoft.Json.JsonConvert.DeserializeObject<Resultado<MovimientoDto>>(result);
            Assert.IsNotNull(resultado);
            Assert.AreEqual(resultado.Correcto, true);
            Assert.IsNotNull(resultado.Datos);
            Assert.AreEqual(Guid.Parse("51ed6452-d6b5-4fa6-a1e4-1e1fbd16d72d"), resultado.Datos.Bodega.Id);
            Assert.AreEqual(Guid.Parse("ea9acaa9-adaf-45bd-980d-a1bb8ab7abc3"), resultado.Datos.Producto.Id);
            Assert.AreEqual(10, resultado.Datos.Cantidad);
            Assert.AreEqual(25000, resultado.Datos.Valor);
            Assert.AreEqual(TipoMovimiento.Ingreso, resultado.Datos.Tipo);
        }

        [TestMethod]
        public async Task CargarProductoSuperarMaximoTest()
        {
            Console.WriteLine(_testContext.TestName);

            var client = _factory.CreateClient();
            MovimientoRequest movimiento = new MovimientoRequest
            {
                BodegaId = "51ed6452-d6b5-4fa6-a1e4-1e1fbd16d72d",
                ProductoId = "ea9acaa9-adaf-45bd-980d-a1bb8ab7abc3",
                Cantidad = 100,
                Valor = 25000
            };
            StringContent cuerpo = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(movimiento), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Inventario/CargarProducto", cuerpo);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var result = await response.Content.ReadAsStringAsync();
            Resultado<MovimientoDto> resultado = Newtonsoft.Json.JsonConvert.DeserializeObject<Resultado<MovimientoDto>>(result);
            Assert.IsNotNull(resultado);
            Assert.AreEqual(false,resultado.Correcto);
            Assert.IsNull(resultado.Datos);
            Assert.AreEqual(1, resultado.Errores.Length);
            Assert.AreEqual("La cantidad de producto a ingresar supera el maximo de productos totales permitido en la bodega", resultado.Errores[0]);
        }

        [TestMethod]
        public async Task SacarProductoTest()
        {
            Console.WriteLine(_testContext.TestName);

            var client = _factory.CreateClient();
            MovimientoRequest movimiento = new MovimientoRequest
            {
                BodegaId = "51ed6452-d6b5-4fa6-a1e4-1e1fbd16d72d",
                ProductoId = "ea9acaa9-adaf-45bd-980d-a1bb8ab7abc3",
                Cantidad = 5,
                Valor = 50000
            };
            StringContent cuerpo = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(movimiento), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Inventario/SacarProducto", cuerpo);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var result = await response.Content.ReadAsStringAsync();
            Resultado<MovimientoDto> resultado = Newtonsoft.Json.JsonConvert.DeserializeObject<Resultado<MovimientoDto>>(result);
            Assert.IsNotNull(resultado);
            Assert.AreEqual(resultado.Correcto, true);
            Assert.IsNotNull(resultado.Datos);
            Assert.AreEqual(Guid.Parse("51ed6452-d6b5-4fa6-a1e4-1e1fbd16d72d"), resultado.Datos.Bodega.Id);
            Assert.AreEqual(Guid.Parse("ea9acaa9-adaf-45bd-980d-a1bb8ab7abc3"), resultado.Datos.Producto.Id);
            Assert.AreEqual(5, resultado.Datos.Cantidad);
            Assert.AreEqual(50000, resultado.Datos.Valor);
            Assert.AreEqual(TipoMovimiento.Salida, resultado.Datos.Tipo);
        }

        [TestMethod]
        public async Task SacarProductoSuperaDisponibleTest()
        {
            Console.WriteLine(_testContext.TestName);

            var client = _factory.CreateClient();
            MovimientoRequest movimiento = new MovimientoRequest
            {
                BodegaId = "51ed6452-d6b5-4fa6-a1e4-1e1fbd16d72d",
                ProductoId = "ea9acaa9-adaf-45bd-980d-a1bb8ab7abc3",
                Cantidad = 150,
                Valor = 50000
            };
            StringContent cuerpo = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(movimiento), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Inventario/SacarProducto", cuerpo);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var result = await response.Content.ReadAsStringAsync();
            Resultado<MovimientoDto> resultado = Newtonsoft.Json.JsonConvert.DeserializeObject<Resultado<MovimientoDto>>(result);
            Assert.IsNotNull(resultado);
            Assert.AreEqual(false, resultado.Correcto);
            Assert.IsNull(resultado.Datos);
            Assert.AreEqual(1, resultado.Errores.Length);
            Assert.AreEqual("La cantidad de producto a sacar supera la cantidad disponible en la bodega", resultado.Errores[0]);
        }

        [TestMethod]
        public async Task MoverProductoBodegasTest()
        {
            Console.WriteLine(_testContext.TestName);

            var client = _factory.CreateClient();
            MovimientoBodegasRequest movimiento = new MovimientoBodegasRequest
            {
                BodegaOrigenId = "51ed6452-d6b5-4fa6-a1e4-1e1fbd16d72d",
                BodegaDestinoId = "74a353ec-7b55-4a64-b919-bf6069e6308d",   
                ProductoId = "ea9acaa9-adaf-45bd-980d-a1bb8ab7abc3",
                Cantidad = 5,
                Valor = 50000
            };
            StringContent cuerpo = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(movimiento), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Inventario/MoverProductoBodegas", cuerpo);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var result = await response.Content.ReadAsStringAsync();
            Resultado<MovimientoDto> resultado = Newtonsoft.Json.JsonConvert.DeserializeObject<Resultado<MovimientoDto>>(result);
            Assert.IsNotNull(resultado);
            Assert.AreEqual(resultado.Correcto, true);
            Assert.IsNotNull(resultado.Datos);
            Assert.AreEqual(Guid.Parse("74a353ec-7b55-4a64-b919-bf6069e6308d"), resultado.Datos.Bodega.Id);
            Assert.AreEqual(Guid.Parse("ea9acaa9-adaf-45bd-980d-a1bb8ab7abc3"), resultado.Datos.Producto.Id);
            Assert.AreEqual(5, resultado.Datos.Cantidad);
            Assert.AreEqual(50000, resultado.Datos.Valor);
            Assert.AreEqual(TipoMovimiento.Ingreso, resultado.Datos.Tipo);
        }

        [TestMethod]
        public async Task MoverProductoBodegasSupererMaximoDestinoTest()
        {
            Console.WriteLine(_testContext.TestName);

            var client = _factory.CreateClient();
            MovimientoBodegasRequest movimiento = new MovimientoBodegasRequest
            {
                BodegaOrigenId = "51ed6452-d6b5-4fa6-a1e4-1e1fbd16d72d",
                BodegaDestinoId = "74a353ec-7b55-4a64-b919-bf6069e6308d",
                ProductoId = "ea9acaa9-adaf-45bd-980d-a1bb8ab7abc3",
                Cantidad = 15,
                Valor = 100000
            };
            StringContent cuerpo = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(movimiento), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Inventario/MoverProductoBodegas", cuerpo);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var result = await response.Content.ReadAsStringAsync();
            Resultado<MovimientoDto> resultado = Newtonsoft.Json.JsonConvert.DeserializeObject<Resultado<MovimientoDto>>(result);
            Assert.IsNotNull(resultado);
            Assert.AreEqual(false, resultado.Correcto);
            Assert.IsNull(resultado.Datos);
            Assert.AreEqual(1, resultado.Errores.Length);
            Assert.AreEqual("La cantidad de producto a sacar supera la cantidad disponible en la bodega de origen", resultado.Errores[0]);
        }

        [TestMethod]
        public async Task ObtenerInvetarioProductoTest()
        {
            Console.WriteLine(_testContext.TestName);

            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/Inventario/ObtenerInvetarioProducto?idProducto=ea9acaa9-adaf-45bd-980d-a1bb8ab7abc3");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var result = await response.Content.ReadAsStringAsync();
            Resultado<ResultadoInventarioProducto> resultado = Newtonsoft.Json.JsonConvert.DeserializeObject<Resultado<ResultadoInventarioProducto>>(result);
            Assert.IsNotNull(resultado);
            Assert.AreEqual(resultado.Correcto, true);
            Assert.IsNotNull(resultado.Datos);
            Assert.IsNotNull(resultado.Datos.Producto);
            Assert.AreEqual(Guid.Parse("ea9acaa9-adaf-45bd-980d-a1bb8ab7abc3"), resultado.Datos.Producto.Id);
            Assert.AreEqual(30, resultado.Datos.CantidadTotal);
            Assert.AreEqual(300000, resultado.Datos.ValorTotal);
            Assert.IsNotNull(resultado.Datos.Detalle);
            Assert.AreEqual(2, resultado.Datos.Detalle.Count);
            Assert.IsNotNull(resultado.Datos.Detalle[0]);
            Assert.IsNotNull(resultado.Datos.Detalle[1]);
            Assert.IsNotNull(resultado.Datos.Detalle[0].Bodega);
            Assert.IsNotNull(resultado.Datos.Detalle[1].Bodega);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _factory.Dispose();
        }
    }

    public class MockInventarioRepositorio : IInventarioRepositorio
    {
        private ListaDatosMock _datosMock;

        public MockInventarioRepositorio()
        {
            _datosMock = new ListaDatosMock();
        }

        public async Task ActualizarInvetario(MovimientoDto movimiento)
        {
            Inventario inventarioActualizar = _datosMock.InventarioLista.FirstOrDefault(f => f.BodegaId == movimiento.Bodega.Id && f.ProductoId == movimiento.Producto.Id);
            if (inventarioActualizar == null)
            {
                inventarioActualizar = new Inventario
                {
                    Id = Guid.NewGuid(),
                    BodegaId = movimiento.Bodega.Id,
                    ProductoId = movimiento.Producto.Id,
                    Cantidad = movimiento.Cantidad,
                    ValorAcumulado = movimiento.Valor,
                    Creacion = DateTime.Now,
                    UltimaModificacion = null
                };
                _datosMock.InventarioLista.Add(inventarioActualizar);
            }
            else
            {
                switch (movimiento.Tipo)
                {
                    case TipoMovimiento.Ingreso:
                        inventarioActualizar.Cantidad += movimiento.Cantidad;
                        inventarioActualizar.ValorAcumulado += movimiento.Valor;
                        break;
                    case TipoMovimiento.Salida:
                        inventarioActualizar.Cantidad -= movimiento.Cantidad;
                        inventarioActualizar.ValorAcumulado -= movimiento.Valor;
                        break;
                }
                inventarioActualizar.UltimaModificacion = DateTime.Now;
            }
        }

        public async Task<MovimientoDto> GuardarMovimiento(MovimientoDto movimiento)
        {
            Movimientos movimientoGuardar = new Movimientos
            {
                Id = movimiento.Id,
                BodegaId = movimiento.Bodega.Id,
                ProductoId = movimiento.Producto.Id,
                FechaMovimiento = movimiento.FechaMovimiento,
                Cantidad = movimiento.Cantidad,
                Tipo = movimiento.Tipo,
                Valor = movimiento.Valor
            };
            _datosMock.listaMovimientos.Add(movimientoGuardar);
            Movimientos movimientoCreado = _datosMock.listaMovimientos.FirstOrDefault(f => f.Id == movimiento.Id);
            movimientoCreado.Bodega = BodegaMapeo.Convertir(movimiento.Bodega);
            movimientoCreado.Producto = ProductoMapeo.Convertir(movimiento.Producto);
            return await Task.FromResult(MovimientoMapeo.Convertir(movimientoCreado));
        }

        public async Task<InventarioDto> ObtenerInventario(Guid bodegaId, Guid productoId)
        {
            Inventario invetarioconsulta = _datosMock.InventarioLista.FirstOrDefault(f => f.BodegaId == bodegaId && f.ProductoId == productoId);
            return await Task.FromResult(InventarioMapeo.Convertir(invetarioconsulta));
        }

        public async Task<List<InventarioDto>> ObtenerInvtarioProducto(Guid productoId)
        {
            List<Inventario> listaInventario = _datosMock.InventarioLista
                                                                .Join(_datosMock.listaBodegas, inv => inv.BodegaId, bod => bod.Id, (inv, bod) => new { inv, bod })
                                                                .Where(w => w.inv.ProductoId == productoId)
                                                                .Select(s => new Inventario
                                                                {
                                                                    Id = s.inv.Id,
                                                                    Bodega = new Bodega
                                                                    {
                                                                        Id = s.bod.Id,
                                                                        Nombre = s.bod.Nombre,
                                                                        Descripcion = s.bod.Descripcion,
                                                                        Ubicacion = s.bod.Ubicacion,
                                                                        CapacidadMaxima = s.bod.CapacidadMaxima
                                                                    },
                                                                    Cantidad = s.inv.Cantidad,
                                                                    Producto = null,
                                                                    ValorAcumulado = s.inv.ValorAcumulado,
                                                                    Creacion = s.inv.Creacion,
                                                                    UltimaModificacion = s.inv.UltimaModificacion
                                                                }).ToList();
            if (listaInventario == null)
                listaInventario = new List<Inventario>();

            return await Task.FromResult(InventarioMapeo.Convertir(listaInventario));
        }
    }
}
