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

namespace PruebaInventarioTest
{
    [TestClass]
    public class ProductoUnitTest
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
                    services.AddScoped<IProductosRepositorio, MockProductoRepositorio>();
                });
            });
        }

        [TestMethod]
        public async Task ConsultaProductoExistenteTest()
        {
            Console.WriteLine(_testContext.TestName);

            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/Productos/ea9acaa9-adaf-45bd-980d-a1bb8ab7abc3");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var result = await response.Content.ReadAsStringAsync();
            Resultado<ProductoDto> resultado = Newtonsoft.Json.JsonConvert.DeserializeObject<Resultado<ProductoDto>>(result);
            Assert.IsNotNull(resultado);
            Assert.AreEqual(resultado.Correcto, true);
            Assert.IsNotNull(resultado.Datos);
            Assert.AreEqual(Guid.Parse("ea9acaa9-adaf-45bd-980d-a1bb8ab7abc3"), resultado.Datos.Id);
        }

        [TestMethod]
        public async Task ConsultaProductoNoExistenteTest()
        {
            Console.WriteLine(_testContext.TestName);

            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/Productos/7d50ad77-d5ec-44b7-9c2e-6e9cfe1191bc");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var result = await response.Content.ReadAsStringAsync();
            Resultado<ProductoDto> resultado = Newtonsoft.Json.JsonConvert.DeserializeObject<Resultado<ProductoDto>>(result);
            Assert.IsNotNull(resultado);
            Assert.AreEqual(resultado.Correcto, false);
            Assert.IsNull(resultado.Datos);
            Assert.AreEqual("Id no encontrado",resultado.Errores[0]);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _factory.Dispose();
        }
    }

    public class MockProductoRepositorio : IProductosRepositorio
    {
        private ListaDatosMock _datosMock;

        public MockProductoRepositorio()
        {
            _datosMock = new ListaDatosMock();
        }

        public async Task<ProductoDto> BuscarProducto(Guid id)
        {  
            Producto producto = _datosMock.productos.FirstOrDefault(f => f.Id == id);
            return await Task.FromResult(ProductoMapeo.Convertir(producto));
        }

        public Task<List<ProductoDto>> ObtenerProductosExistentes()
        {
            throw new NotImplementedException();
        }
    }
}
