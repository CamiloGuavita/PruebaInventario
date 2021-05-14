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
    public class BodegaUnitTest
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
                    services.AddScoped<IBodegaRepositorio, MockBodegaRepositorio>();
                });
            });
        }

        [TestMethod]
        public async Task ConsultaBodegasExistentesTest()
        {
            Console.WriteLine(_testContext.TestName);

            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/Bodega");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var result = await response.Content.ReadAsStringAsync();
            List<BodegaDto> resultado = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BodegaDto>>(result);
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2,resultado.Count);
            Assert.AreEqual(Guid.Parse("51ed6452-d6b5-4fa6-a1e4-1e1fbd16d72d"),resultado[0].Id);
            Assert.AreEqual(Guid.Parse("74a353ec-7b55-4a64-b919-bf6069e6308d"), resultado[1].Id);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _factory.Dispose();
        }

    }

    public class MockBodegaRepositorio : IBodegaRepositorio
    {
        private ListaDatosMock _datosMock;

        public MockBodegaRepositorio()
        {
            _datosMock = new ListaDatosMock();
        }

        public async Task<BodegaDto> BuscarBodega(Guid id)
        {
            Bodega resultado = _datosMock.listaBodegas.FirstOrDefault(f => f.Id == id);
            return await Task.FromResult(BodegaMapeo.Convertir(resultado));
        }

        public async Task<List<BodegaDto>> ObtenerBodegasExistentes()
        {
            return await Task.FromResult(BodegaMapeo.Convertir(_datosMock.listaBodegas));
        }
    }
}
