using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Interfaces;
using Aplicacion.Dto;
using Aplicacion.Comun.Modelos;
using Dominio.Enums;

namespace Aplicacion.Servicios
{
    public class InventarioServicio : IInventarioServicio
    {
        private IInventarioRepositorio _repositorioInventario;
        private IBodegaRepositorio _repositorioBodega;
        private IProductosRepositorio _repositorioProducto;

        public InventarioServicio(IInventarioRepositorio repositorioInventario,IBodegaRepositorio repositorioBodega,IProductosRepositorio repositorioProducto)
        {
            this._repositorioInventario = repositorioInventario;
            this._repositorioBodega = repositorioBodega;
            this._repositorioProducto = repositorioProducto;
        }

        public async Task<Resultado<MovimientoDto>> CargarProducto(MovimientoRequest movimiento)
        {
            List<string> errores = new List<string>();
            Guid bodegaId;
            Guid productoId;
            BodegaDto bodega = null;
            ProductoDto producto = null;
            if (Guid.TryParse(movimiento.BodegaId, out bodegaId))
            {
                bodega = await _repositorioBodega.BuscarBodega(bodegaId);
                if (bodega == null)
                    errores.Add($"La bodega con Id {movimiento.BodegaId} no esta en el sistema");
            }
            else {
                errores.Add("La Id de la bodega no tiene el formato correcto");
            }
            if (Guid.TryParse(movimiento.ProductoId, out productoId))
            {
                producto = await _repositorioProducto.BuscarProducto(productoId);
                if (producto == null)
                    errores.Add($"El producto con Id {movimiento.ProductoId} no esta en el sistema");
            }
            else
            {
                errores.Add("La Id del producto no tiene el formato correcto");
            }

            if (movimiento.Cantidad <= 0)
            {
                errores.Add("La cantidad debe ser mayor a 0");
            }

            if (movimiento.Valor <= 0)
            {
                errores.Add("El valor debe ser mayor a 0");
            }

            if (errores.Count > 0)
            {
                return Resultado<MovimientoDto>.Failure(errores);
            }

            InventarioDto inventario = await _repositorioInventario.ObtenerInventario(bodega.Id, producto.Id);
            int capacidadDisponible = 0;
            if(inventario != null)
            {
                capacidadDisponible = bodega.CapacidadMaxima - inventario.Cantidad;
            }
            else
            {
                capacidadDisponible = bodega.CapacidadMaxima;
            }

            if (movimiento.Cantidad > capacidadDisponible) 
            {
                return Resultado<MovimientoDto>.Failure("La cantidad de producto a ingresar supera el maximo de productos totales permitido en la bodega");
            }

            MovimientoDto movimientoCrear = new MovimientoDto
            {
                Id = Guid.NewGuid(),
                FechaMovimiento = DateTime.Now,
                Bodega = bodega,
                Producto = producto,
                Cantidad = movimiento.Cantidad,
                Valor = movimiento.Valor,
                Tipo = TipoMovimiento.Ingreso
            };

            MovimientoDto movimientoCreado = await _repositorioInventario.GuardarMovimiento(movimientoCrear);
            
            if(movimientoCreado != null)
            {
                await _repositorioInventario.ActualizarInvetario(movimientoCreado);
                return Resultado<MovimientoDto>.Success(movimientoCreado);
            }
            else
            {
                errores.Add("Se ha presnetado un problema al guardar el movimiento");
                return Resultado<MovimientoDto>.Failure(errores);
            }
        }

        public async Task<Resultado<MovimientoDto>> SacarProducto(MovimientoRequest movimiento)
        {
            List<string> errores = new List<string>();
            Guid bodegaId;
            Guid productoId;
            BodegaDto bodega = null;
            ProductoDto producto = null;
            if (Guid.TryParse(movimiento.BodegaId, out bodegaId))
            {
                bodega = await _repositorioBodega.BuscarBodega(bodegaId);
                if (bodega == null)
                    errores.Add($"La bodega con Id {movimiento.BodegaId} no esta en el sistema");
            }
            else
            {
                errores.Add("La Id de la bodega no tiene el formato correcto");
            }
            if (Guid.TryParse(movimiento.ProductoId, out productoId))
            {
                producto = await _repositorioProducto.BuscarProducto(productoId);
                if (producto == null)
                    errores.Add($"El producto con Id {movimiento.ProductoId} no esta en el sistema");
            }
            else
            {
                errores.Add("La Id del producto no tiene el formato correcto");
            }

            if (movimiento.Cantidad <= 0)
            {
                errores.Add("La cantidad debe ser mayor a 0");
            }

            if (movimiento.Valor <= 0)
            {
                errores.Add("El valor debe ser mayor a 0");
            }

            if (errores.Count > 0)
            {
                return Resultado<MovimientoDto>.Failure(errores);
            }

            InventarioDto inventario = await _repositorioInventario.ObtenerInventario(bodega.Id, producto.Id);
            int capacidadDisponible = 0;
            if (inventario != null)
            {
                capacidadDisponible = inventario.Cantidad;
            }
            else 
            {
                capacidadDisponible = 0;            
            }

            if (movimiento.Cantidad > capacidadDisponible)
            {
                return Resultado<MovimientoDto>.Failure("La cantidad de producto a sacar supera la cantidad disponible en la bodega");
            }
            if (movimiento.Valor > inventario.ValorAcumulado)
            {
                return Resultado<MovimientoDto>.Failure("El valor del movimiento a sacar supera el valor acumulado para ese producto en la bodega");
            }

            MovimientoDto movimientoCrear = new MovimientoDto
            {
                Id = Guid.NewGuid(),
                FechaMovimiento = DateTime.Now,
                Bodega = bodega,
                Producto = producto,
                Cantidad = movimiento.Cantidad,
                Valor = movimiento.Valor,
                Tipo = TipoMovimiento.Salida
            };

            MovimientoDto movimientoCreado = await _repositorioInventario.GuardarMovimiento(movimientoCrear);

            if (movimientoCreado != null)
            {
                await _repositorioInventario.ActualizarInvetario(movimientoCreado);
                return Resultado<MovimientoDto>.Success(movimientoCreado);
            }
            else
            {
                errores.Add("Se ha presnetado un problema al guardar el movimiento");
                return Resultado<MovimientoDto>.Failure(errores);
            }
        }

        public async Task<Resultado<MovimientoDto>> MoverProductoBodegas(MovimientoBodegasRequest movimiento)
        {
            List<string> errores = new List<string>();
            Guid bodegaOrigenId;
            Guid bodegaDestinoId;
            Guid productoId;
            BodegaDto bodegaOrigen = null;
            BodegaDto bodegaDestino = null;
            ProductoDto producto = null;
            if (Guid.TryParse(movimiento.BodegaOrigenId, out bodegaOrigenId))
            {
                bodegaOrigen = await _repositorioBodega.BuscarBodega(bodegaOrigenId);
                if (bodegaOrigen == null)
                    errores.Add($"La bodega origen con Id {movimiento.BodegaOrigenId} no esta en el sistema");
            }
            else
            {
                errores.Add("La Id de la bodega origen no tiene el formato correcto");
            }

            if (Guid.TryParse(movimiento.BodegaDestinoId, out bodegaDestinoId))
            {
                bodegaDestino = await _repositorioBodega.BuscarBodega(bodegaDestinoId);
                if (bodegaDestino == null)
                    errores.Add($"La bodega destino con Id {movimiento.BodegaDestinoId} no esta en el sistema");
            }
            else
            {
                errores.Add("La Id de la bodega destino no tiene el formato correcto");
            }

            if (Guid.TryParse(movimiento.ProductoId, out productoId))
            {
                producto = await _repositorioProducto.BuscarProducto(productoId);
                if (producto == null)
                    errores.Add($"El producto con Id {movimiento.ProductoId} no esta en el sistema");
            }
            else
            {
                errores.Add("La Id del producto no tiene el formato correcto");
            }

            if (movimiento.Cantidad <= 0)
            {
                errores.Add("La cantidad debe ser mayor a 0");
            }

            if (movimiento.Valor <= 0)
            {
                errores.Add("El valor debe ser mayor a 0");
            }

            if (errores.Count > 0)
            {
                return Resultado<MovimientoDto>.Failure(errores);
            }

            InventarioDto inventarioOrigen = await _repositorioInventario.ObtenerInventario(bodegaOrigen.Id, producto.Id);
            InventarioDto inventarioDestino = await _repositorioInventario.ObtenerInventario(bodegaDestino.Id, producto.Id);
            int cantidadDisponibleOrigen = 0;
            if (inventarioOrigen != null)
            {
                cantidadDisponibleOrigen = inventarioOrigen.Cantidad;
            }
            else
            {
                cantidadDisponibleOrigen = 0;
            }

            if (movimiento.Cantidad > cantidadDisponibleOrigen)
            {
                errores.Add("La cantidad de producto a sacar supera la cantidad disponible en la bodega de origen");
            }
            if (movimiento.Valor > inventarioOrigen.ValorAcumulado)
            {
                errores.Add("El valor del movimiento a sacar supera el valor acumulado para ese producto en la bodega de origen");
            }
            int capacidadDisponibleDestino = 0;
            if (inventarioDestino != null) {
                capacidadDisponibleDestino = bodegaDestino.CapacidadMaxima - inventarioDestino.Cantidad;
            }
            else
            {
                capacidadDisponibleDestino = bodegaDestino.CapacidadMaxima;
            }
            if(movimiento.Cantidad > capacidadDisponibleDestino)
            {
                errores.Add("La cantidad de producto a ingresar supera el maximo de productos totales permitido en la bodega de destino");
            }
            if (errores.Count > 0)
            {
                return Resultado<MovimientoDto>.Failure(errores);
            }

            MovimientoDto movimientoSacar = new MovimientoDto
            {
                Id = Guid.NewGuid(),
                FechaMovimiento = DateTime.Now,
                Bodega = bodegaOrigen,
                Producto = producto,
                Cantidad = movimiento.Cantidad,
                Valor = movimiento.Valor,
                Tipo = TipoMovimiento.Salida
            };

            MovimientoDto movimientoCargar = new MovimientoDto
            {
                Id = Guid.NewGuid(),
                FechaMovimiento = DateTime.Now,
                Bodega = bodegaDestino,
                Producto = producto,
                Cantidad = movimiento.Cantidad,
                Valor = movimiento.Valor,
                Tipo = TipoMovimiento.Ingreso
            };
            MovimientoDto movimientoSacarCreado = await _repositorioInventario.GuardarMovimiento(movimientoSacar);

            if (movimientoSacarCreado != null)
            {
                await _repositorioInventario.ActualizarInvetario(movimientoSacarCreado);
                MovimientoDto movimientoCargarCreado = await _repositorioInventario.GuardarMovimiento(movimientoCargar);
                if (movimientoCargarCreado != null)
                {
                    await _repositorioInventario.ActualizarInvetario(movimientoCargarCreado);
                    return Resultado<MovimientoDto>.Success(movimientoCargarCreado);
                }
                else
                {
                    errores.Add("Se ha presnetado un problema al guardar el movimiento");   
                }
            }
            else
            {
                errores.Add("Se ha presnetado un problema al guardar el movimiento");
            }
            return Resultado<MovimientoDto>.Failure(errores);
        }

        public async Task<Resultado<ResultadoInventarioProducto>> ObtenerInvetarioProducto(string productoId)
        {
            List<string> errores = new List<string>();
            Guid productoGuid;
            List<InventarioDto> listaInventario = null;
            ProductoDto producto = null;
            if (Guid.TryParse(productoId, out productoGuid))
            {
                producto = await _repositorioProducto.BuscarProducto(productoGuid);
                if (producto == null)
                    errores.Add($"El producto con Id {productoId} no esta en el sistema");
            }
            else
            {
                errores.Add("El Id del producto no tiene el formato correcto");
            }
            if (errores.Count > 0)
            {
                return Resultado<ResultadoInventarioProducto>.Failure(errores);
            }

            listaInventario = await _repositorioInventario.ObtenerInvtarioProducto(productoGuid);

            int cantidadTotal = listaInventario.Sum(s => s.Cantidad);
            decimal valorTotal = listaInventario.Sum(s => s.ValorAcumulado);
            ResultadoInventarioProducto respuesta = new ResultadoInventarioProducto()
            {
                Producto = producto,
                CantidadTotal = cantidadTotal,
                ValorTotal = valorTotal,
                Detalle = listaInventario
            };
            return Resultado<ResultadoInventarioProducto>.Success(respuesta);
        }
    }
}
