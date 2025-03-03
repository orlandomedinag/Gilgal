using GilgalInventar.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GilgalInventar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientoDetallesController : ControllerBase
    {
        private readonly gilgalContext _context;

        public MovimientoDetallesController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet(Name = "GetAll")]
        public async Task<object> GetAll()
        {
            try
            {
                var movimientoDetalles = await _context.MovimientoDetalles.Where(r => r.Activo).ToListAsync();
                return new { Items = movimientoDetalles, Count = movimientoDetalles.Count() };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // GET: api/<ValuesController1>
        [HttpGet("{id}", Name = "Get")]
        public async Task<object> Get(long id)
        {
            try
            {
                var queryString = Request.Query;
                string filter = queryString["$filter"];
                string auto = queryString["$inlineCount"];
                var movimientoDetalles = Enumerable.Empty<MovimientoDetalle>().AsQueryable();
                if (id > 0)
                    movimientoDetalles = (from detaremi in _context.MovimientoDetalles.AsQueryable()
                                          join elemento in _context.ElementoEquipos.Where(r => r.Activo).AsQueryable()
                                          on detaremi.BarCode equals elemento.BarCode
                                          join unidad in _context.Unidades.AsQueryable()
                                          on elemento.IDUnidad equals unidad.IDUnidad
                                          join marca in _context.Marcas.AsQueryable()
                                          on elemento.IDMarca equals marca.IDMarca
                                          where detaremi.Activo && detaremi.IDMovimiento == id
                                          select new MovimientoDetalle
                                          {
                                              IDMovimientoDetalle = detaremi.IDMovimientoDetalle,
                                              IDMovimiento = detaremi.IDMovimiento,
                                              ItemNo = detaremi.ItemNo,
                                              BarCode = detaremi.BarCode,
                                              Descripcion = marca.Nombre,
                                              EntradaCantidad = detaremi.EntradaCantidad,
                                              EntradaIDEstado = detaremi.EntradaIDEstado,
                                              Unidad = unidad.Nombre,
                                              EntradaObservaciones = detaremi.EntradaObservaciones,
                                              SalidaCantidad = detaremi.SalidaCantidad,
                                              SalidaIDEstado = detaremi.SalidaIDEstado,
                                              SalidaObservaciones = detaremi.SalidaObservaciones,
                                              IDUnidad = detaremi.IDUnidad,
                                              EntregadoCantidad = detaremi.EntregadoCantidad,
                                              Activo = detaremi.Activo,
                                              IDArea = detaremi.IDArea
                                          }
                                        ).OrderBy(r => r.ItemNo);
                else
                    movimientoDetalles = (from detaremi in _context.MovimientoDetalles.AsQueryable()
                                          join elemento in _context.ElementoEquipos.Where(r => r.Activo).AsQueryable()
                                          on detaremi.BarCode equals elemento.BarCode
                                          join unidad in _context.Unidades.AsQueryable()
                                          on elemento.IDUnidad equals unidad.IDUnidad
                                          where detaremi.Activo
                                          select new MovimientoDetalle
                                          {
                                              IDMovimientoDetalle = detaremi.IDMovimientoDetalle,
                                              IDMovimiento = detaremi.IDMovimiento,
                                              ItemNo = detaremi.ItemNo,
                                              BarCode = detaremi.BarCode,
                                              Descripcion = elemento.Descripcion,
                                              EntradaCantidad = detaremi.EntradaCantidad,
                                              EntradaIDEstado = detaremi.EntradaIDEstado,
                                              Unidad = unidad.Nombre,
                                              EntradaObservaciones = detaremi.EntradaObservaciones,
                                              SalidaCantidad = detaremi.SalidaCantidad,
                                              SalidaIDEstado = detaremi.SalidaIDEstado,
                                              SalidaObservaciones = detaremi.SalidaObservaciones,
                                              IDUnidad = detaremi.IDUnidad,
                                              EntregadoCantidad = detaremi.EntregadoCantidad,
                                              Activo = detaremi.Activo,
                                              IDArea = detaremi.IDArea
                                          }
                                        ).OrderBy(r => r.ItemNo);
                //return new { Items = movimientoDetalles, Count = movimientoDetalles.Count() };
                if (queryString.Keys.Contains("$inlinecount"))
                {
                    //StringValues Skip;
                    //StringValues Take;
                    if (!string.IsNullOrEmpty(filter))
                    {
                        string key;
                        if (filter.Contains("substring")) //searching 
                        {
                            key = filter.Split(new string[] { "'" }, StringSplitOptions.None)[1].ToUpper();
                            movimientoDetalles = movimientoDetalles.Where(fil => fil.BarCode.ToUpper().Contains(key.ToUpper()) || fil.Descripcion.ToUpper().Contains(key.ToUpper())).Distinct();
                        }
                    }
                    //int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    //int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : movimientoDetalles.Count();
                    var count = await movimientoDetalles.CountAsync();
                    return new { Items = movimientoDetalles, Count = count };
                }
                else
                {
                    return new { Items = movimientoDetalles, Count = await movimientoDetalles.CountAsync() };
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MovimientoDetalle movimientoDetalle)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Validar si el campo del elemento es único
                    //Si es único agregar el registro y actualizar el Item
                    //Si no es único incrementar la cantidad de salida
                    var elemento = await _context.ElementoEquipos.FindAsync(movimientoDetalle.BarCode);
                    var tipoele = await _context.TipoEquipoElementos.FindAsync(elemento.IDTipoEquElem);
                    var movimiento = await _context.Movimientos.FindAsync(movimientoDetalle.IDMovimiento);
                    AreaElemento areaelemento = new AreaElemento();
                    if (movimientoDetalle.IDArea == 0 || movimientoDetalle.IDArea==null)
                    {
                        areaelemento = await _context.AreaElementos.FirstOrDefaultAsync(r => r.BarCode == elemento.BarCode);
                    }
                    else
                    {
                        areaelemento = await _context.AreaElementos.FirstOrDefaultAsync(r => r.IDArea == movimientoDetalle.IDArea && r.BarCode == elemento.BarCode);
                    }
                    //areaelemento = await _context.AreaElementos.FirstOrDefaultAsync(r => r.IDArea == movimientoDetalle.IDArea && r.BarCode == elemento.BarCode);
                    //if (tipoele.CodigoUnico)
                    //    areaelemento = await _context.AreaElementos.FirstOrDefaultAsync(r => r.BarCode == elemento.BarCode);
                    //else
                    //    areaelemento = await _context.AreaElementos.FirstOrDefaultAsync(r => r.IDArea == movimiento.IDArea && r.BarCode == elemento.BarCode);
                    var detaremi = new MovimientoDetalle();
                    //contar el numero de items de la movimiento
                    var items = _context.MovimientoDetalles.Where(r => r.IDMovimiento == movimientoDetalle.IDMovimiento && r.Activo).Count();
                    //Actualizar existencias del elemento
                    if (string.IsNullOrEmpty(movimientoDetalle.Descripcion))
                        movimientoDetalle.Descripcion = elemento.Nombre;
                    if (areaelemento == null && (movimiento.IDTipoMovimiento == 2 || movimiento.IDTipoMovimiento == 3))
                    {
                        areaelemento = new AreaElemento
                        {
                            IDArea = movimiento.IDArea,
                            BarCode = movimientoDetalle.BarCode,
                            Stock = movimientoDetalle.EntradaCantidad
                        };
                        _context.AreaElementos.Add(areaelemento);
                    } else
                    {
                        movimientoDetalle.IDArea = areaelemento.IDArea;
                    }
                    if (tipoele.CodigoUnico)
                    {
                        elemento.EstadoAlmacen = "EN BODEGA";
                        if (movimiento.IDTipoMovimiento == 4)
                            elemento.EstadoAlmacen = "REMITIDO";
                        if (movimiento.IDTipoMovimiento == 5)
                            elemento.EstadoAlmacen = "ENTREGADO";
                        movimientoDetalle.ItemNo = items + 1;
                        _context.MovimientoDetalles.Add(movimientoDetalle);
                    }
                    else
                    {
                        //buscar el detalle
                        detaremi = _context.MovimientoDetalles.FirstOrDefault(r => r.IDMovimiento == movimientoDetalle.IDMovimiento && r.BarCode == movimientoDetalle.BarCode && r.Activo);
                        if (detaremi != null)
                        {
                            if ((new List<long>() { 4, 5, 7 }.Contains(movimiento.IDTipoMovimiento)) && movimientoDetalle.SalidaCantidad > 0)
                            {
                                detaremi.SalidaCantidad = detaremi.SalidaCantidad + movimientoDetalle.SalidaCantidad;
                            }
                            if ((new List<long>() { 2, 3, 6 }.Contains(movimiento.IDTipoMovimiento)) && movimientoDetalle.EntradaCantidad > 0)
                            {
                                detaremi.EntradaCantidad = detaremi.EntradaCantidad + movimientoDetalle.EntradaCantidad;
                            }
                            _context.MovimientoDetalles.Update(detaremi);
                        }
                        else
                        {
                            movimientoDetalle.ItemNo = items + 1;
                            _context.MovimientoDetalles.Add(movimientoDetalle);
                        }
                    } 
                    _context.ElementoEquipos.Update(elemento);
                    await _context.SaveChangesAsync();
                    var sql = @"EXEC sp_ActualizarExistenciasElemento @BarCode = N'" + movimientoDetalle.BarCode + "'";
                    var sp_elementos = await _context.sp_ActualizarExistenciasElemento.FromSqlRaw(sql).ToListAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
                //movimiento.Conceptos = string.Join(",", movimiento.lstConceptos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [HttpPut]
        public async Task<IActionResult> Put(long id, [FromBody] MovimientoDetalle movimientoDetalle)
        {
            try
            {
                _context.Entry(movimientoDetalle).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                var sql = @"EXEC sp_ActualizarExistenciasElemento @BarCode = N'" + movimientoDetalle.BarCode + "'";
                var sp_elementos = await _context.sp_ActualizarExistenciasElemento.FromSqlRaw(sql).ToListAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPut]
        [Route("PutEntrada")]
        public async Task<IActionResult> PutEntrada(long id, [FromBody] MovimientoDetalle movimientoDetalle)
        {
            try
            {
                var elemento = await _context.ElementoEquipos.FindAsync(movimientoDetalle.BarCode);
                var tipoele = await _context.TipoEquipoElementos.FindAsync(elemento.IDTipoEquElem);
                var movimiento = await _context.Movimientos.FindAsync(movimientoDetalle.IDMovimiento);
                var cantidadantes = GetAllMovimientoDetalles().Where(r => r.IDMovimientoDetalle == movimientoDetalle.IDMovimientoDetalle).Select(r => r.EntradaCantidad).FirstOrDefault();
                if (cantidadantes != movimientoDetalle.EntradaCantidad)
                    elemento.Stock = elemento.Stock - cantidadantes + movimientoDetalle.EntradaCantidad;
                if (tipoele.CodigoUnico)
                    elemento.EstadoAlmacen = "EN BODEGA";
                _context.ElementoEquipos.Update(elemento);
                _context.MovimientoDetalles.Update(movimientoDetalle);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetMovimientoByID/{id}")]
        public async Task<object> GetMovimientoByID(long id)
        {
            try
            {
                //Calcular stok elementos
                var detamovi = await _context.MovimientoDetalles.FindAsync(id);
                return Ok(detamovi);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [HttpGet]
        [Route("[action]/{idremi}/{barcode}")]
        public async Task<object> GetMovimientoBarcode(long idremi, string barcode)
        {
            try
            {
                //Calcular stok elementos
                var detamovi = await _context.MovimientoDetalles.FirstOrDefaultAsync(r => r.IDMovimiento == idremi && r.BarCode == barcode && r.Activo);
                return Ok(detamovi);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("[action]/{idremi}")]
        public async Task<object> GetAllElementosRemi(long idremi)
        {
            try
            {
                //Calcular stok elementos
                var detamovis = (from deta in await _context.MovimientoDetalles.Where(r => r.IDMovimiento == idremi && r.Activo).ToListAsync()
                                 join elemento in _context.ElementoEquipos.Where(r => r.Activo).AsQueryable()
                                 on deta.BarCode equals elemento.BarCode
                                 select elemento
                                 ).ToList();
                return Ok(detamovis);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("GetAllMovimientoDetalles")]
        public IQueryable<MovimientoDetalle> GetAllMovimientoDetalles()
        {
            IQueryable<MovimientoDetalle> detalles = _context.MovimientoDetalles;
            return detalles;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var movimientoDetalle = await _context.MovimientoDetalles.FindAsync(id);
                if (movimientoDetalle != null)
                {
                    //_context.Remove(movimiento);
                    movimientoDetalle.Activo = false;
                    var elemento = await _context.ElementoEquipos.FindAsync(movimientoDetalle.BarCode);
                    var tipoele = await _context.TipoEquipoElementos.FindAsync(elemento.IDTipoEquElem);
                    var movimiento = await _context.Movimientos.FindAsync(movimientoDetalle.IDMovimiento);
                    if (tipoele.CodigoUnico)
                    {
                        if (movimiento.IDTipoMovimiento == 2)
                            elemento.EstadoAlmacen = "N/A";
                        if (movimiento.IDTipoMovimiento == 4 || movimiento.IDTipoMovimiento == 5)
                            elemento.EstadoAlmacen = "EN BODEGA";
                        _context.ElementoEquipos.Update(elemento);
                    }
                    _context.Entry(movimientoDetalle).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    var sql = @"EXEC sp_ActualizarExistenciasElemento @BarCode = N'" + movimientoDetalle.BarCode + "'";
                    var sp_elementos = await _context.sp_ActualizarExistenciasElemento.FromSqlRaw(sql).ToListAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
