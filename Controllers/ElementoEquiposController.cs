using GilgalInventar.Data;
using Microsoft.AspNetCore.Http;
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
    public class ElementoEquiposController : ControllerBase
    {
        private readonly gilgalContext _context;

        public ElementoEquiposController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                //var elementoEquipos = await _context.ElementoEquipos.Where(r => r.Activo).ToListAsync();
                var queryString = Request.Query;
                string filter = queryString["$filter"];
                string auto = queryString["$inlineCount"];
                var areasEle = _context.AreaElementos.Where(r => r.Activo).AsQueryable();
                var empresa = await _context.Empresas.FirstOrDefaultAsync();
                var elementos = await _context.ElementoEquipos.Where(r => r.Activo).ToListAsync();
                sp_ConsultaKardexInventario ultimaRemision = new sp_ConsultaKardexInventario();
                List<OrigenDestino> origenes = await _context.OrigenDestinos.ToListAsync();
                double holguraStock = empresa.AlertaStock / 100.00;
                //foreach (var ele in elementos)
                //{
                //    ele.Stock = areasEle.Where(r => r.BarCode == ele.BarCode).Sum(r => r.Stock);
                //    _context.ElementoEquipos.Update(ele);
                //}
                //await _context.SaveChangesAsync();
                //Calcular stok elementos
                //var sql = @"EXEC sp_ActualizarExistencias";
                //var sp_elementos = await _context.sp_ActualizarExistencias.FromSqlRaw(sql).ToListAsync();

                var elementoEquipos = _context.ElementoEquipos.Where(r => r.Activo).AsQueryable();
                if (queryString.Keys.Contains("$inlinecount"))
                {
                    StringValues Skip;
                    StringValues Take;
                    if (!string.IsNullOrEmpty(filter))
                    {
                        string key;
                        if (filter.Contains("substring")) //searching 
                        {
                            key = filter.Split(new string[] { "'" }, StringSplitOptions.None)[1].ToUpper();
                            elementoEquipos = _context.ElementoEquipos.Where(r => r.Activo).Where(fil => fil.BarCode.ToUpper().Contains(key) || fil.Nombre.ToUpper().Contains(key) || fil.Serial.ToUpper().Contains(key) || fil.Modelo.ToUpper().Contains(key) || fil.Descripcion.ToUpper().Contains(key) || fil.EstadoAlmacen.ToUpper().Contains(key)).Distinct().AsQueryable();
                        }
                    }
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : elementoEquipos.Count();
                    var count = elementoEquipos.Count();
                    foreach (ElementoEquipo elemento in elementoEquipos.Where(r => r.EstadoAlmacen == "REMITIDO").ToList())
                    {
                        ultimaRemision = new sp_ConsultaKardexInventario();
                        ultimaRemision = ConsultarUltimaRemision(elemento) as sp_ConsultaKardexInventario;
                        if (ultimaRemision != null)
                        {
                            elemento.Prefijo = ultimaRemision.Prefijo;
                            elemento.OrigenDestino = ultimaRemision.Destino;
                            elemento.FechaUltRemision = (DateTime)ultimaRemision.Fecha;
                        } else
                        {
                            elemento.Prefijo = string.Empty;
                            elemento.OrigenDestino = string.Empty;
                            elemento.FechaUltRemision = null;

                        }
                    }
                    return new { Items = elementoEquipos.OrderBy(e => e.Nombre).Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = elementoEquipos, Count = elementoEquipos.Count() };
                }
                //var elementosSinQR = elementoEquipos.Where(r => r.QRCode == null || r.QRCode == string.Empty);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        public sp_ConsultaKardexInventario ConsultarUltimaRemision(ElementoEquipo elemento)
        {
            List<sp_ConsultaKardexInventario> consultaKardex = _context.sp_ConsultaKardexInventario.FromSqlRaw("EXEC sp_ConsultaKardexInventarioElemento @IDTipoEquElem = {0}, @FechaDesdeC = {1}, @FechaHastaC = {2}, @BarCodeElemento = {3}", elemento.IDTipoEquElem.ToString(), null, null, elemento.BarCode).ToList();
            return consultaKardex.FirstOrDefault();
        }
        // GET: api/<ValuesController1>
        [HttpGet]
        [Route("[action]")]
        public async Task<object> GetAllElementos()
        {
            try
            {
                var elementoEquipos = await _context.ElementoEquipos.Where(r => r.Activo).OrderBy(r => r.BarCode).ToListAsync();
                return Ok(elementoEquipos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/<ValuesController1>
        [HttpGet]
        [Route("[action]")]
        public async Task<object> GetElementosStockMin()
        {
            try
            {
                //Calcular stok elementos
                var empresa = await _context.Empresas.FirstOrDefaultAsync();
                var areasEle = _context.AreaElementos.Where(r => r.Activo).AsQueryable();
                var elementos = await _context.ElementoEquipos.Where(r => r.Activo).ToListAsync();
                double holguraStock = empresa.AlertaStock / 100.00;
                //var sql = @"EXEC sp_ActualizarExistencias";
                //var sp_elementos = await _context.sp_ActualizarExistencias.FromSqlRaw(sql).ToListAsync();
                //incluir cantidad a pedir
                //SOLO LOS QUE TENGAN MENORES AL STOK MINIMO 
                //foreach (var ele in elementos)
                //{
                //    ele.Stock = areasEle.Where(r => r.BarCode == ele.BarCode).Sum(r => r.Stock);
                //    _context.ElementoEquipos.Update(ele);
                //}
                //await _context.SaveChangesAsync();
                // var elementoEquipos = _context.ElementoEquipos.Where(r => r.Activo).AsQueryable();
                var tipos = await _context.TipoEquipoElementos.Where(r => r.CodigoUnico == false).Select(r => r.IDTipoEquElem).ToArrayAsync();
                var elementoEquipos = _context.ElementoEquipos.Where(r => r.Activo && r.IDTipoEquElem != 1 && r.Stock < (r.StockMinimo)).ToList();
                var ElementosStockMins = (from eleq in elementoEquipos
                                          join tipo in await _context.TipoEquipoElementos.ToListAsync()
                                          on eleq.IDTipoEquElem equals tipo.IDTipoEquElem
                                          join unidad in await _context.Unidades.ToListAsync()
                                          on eleq.IDUnidad equals unidad.IDUnidad
                                          select new ElementosStockMin
                                          {
                                              BarCode = eleq.BarCode,
                                              Tipo = tipo.NombreTipo,
                                              Nombre = eleq.Nombre,
                                              Descripcion = eleq.Descripcion,
                                              Unidad = unidad.Nombre,
                                              Talla = (eleq.Talla == null) ? string.Empty : eleq.Talla,
                                              StockMin = eleq.StockMinimo,
                                              Stock = eleq.Stock,
                                              CantPedir = ((eleq.StockMinimo - eleq.Stock) > 0 ? (eleq.StockMinimo - eleq.Stock) : 0)
                                          }
                                          ).ToList();

                return Ok(ElementosStockMins);
                //var queryString = Request.Query;
                //string filter = queryString["$filter"];
                //string auto = queryString["$inlineCount"];
                ////elementos = await _context.ElementoEquipos.Where(r => r.Activo).ToListAsync();
                //if (queryString.Keys.Contains("$inlinecount"))
                //{
                //    StringValues Skip;
                //    StringValues Take;
                //    if (!string.IsNullOrEmpty(filter))
                //    {
                //        string key;
                //        if (filter.Contains("substring")) //searching 
                //        {
                //            key = filter.Split(new string[] { "'" }, StringSplitOptions.None)[1].ToUpper();
                //            elementoEquipos = elementoEquipos.Where(fil => fil.BarCode.ToUpper().Contains(key) || fil.Nombre.ToUpper().Contains(key) || fil.Serial.ToUpper().Contains(key) || fil.Modelo.ToUpper().Contains(key) || fil.Descripcion.ToUpper().Contains(key)).Distinct();
                //        }
                //    }
                //    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                //    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : elementoEquipos.Count();
                //    var count = elementoEquipos.Count();

                //    return new { Items = elementoEquipos.Skip(skip).Take(top), Count = count };
                //}
                //else
                //{
                //    return new { Items = elementoEquipos, Count = elementoEquipos.Count() };
                //}
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        [Route("[action]")]
        public async Task<object> GetElementosEstados()
        {
            try
            {
                //Calcular stok elementos
                var empresa = await _context.Empresas.FirstOrDefaultAsync();
                var areasEle = _context.AreaElementos.Where(r => r.Activo).AsQueryable();
                var elementos = await _context.ElementoEquipos.Where(r => r.Activo && r.IDTipoEquElem == 1).ToListAsync();
                var movimientos = (from movimi in await _context.Movimientos.Where(r => r.Activo).ToListAsync()
                                   join detamovi in await _context.MovimientoDetalles.Where(r => r.Activo).ToListAsync()
                                   on movimi.IDMovimiento equals detamovi.IDMovimiento
                                   select new MovimientosElemento
                                   {
                                       IDMovimientoDetalle = detamovi.IDMovimientoDetalle,
                                       BarCode = detamovi.BarCode,
                                       Prefijo = movimi.Prefijo,
                                       FechaEntrada = movimi.EntradaFecha,
                                       FechaSalida = movimi.DespachadoFecha
                                   }).ToList();
                double holguraStock = empresa.AlertaStock / 100.00;
                //incluir cantidad a pedir
                //SOLO LOS QUE TENGAN MENORES AL STOK MINIMO 
                //foreach (var ele in elementos)
                //{
                //    ele.Stock = areasEle.Where(r => r.BarCode == ele.BarCode).Sum(r => r.Stock);
                //    _context.ElementoEquipos.Update(ele);
                //}
                //await _context.SaveChangesAsync();
                // var elementoEquipos = _context.ElementoEquipos.Where(r => r.Activo).AsQueryable();
                var tipos = await _context.TipoEquipoElementos.Where(r => r.CodigoUnico == false).Select(r => r.IDTipoEquElem).ToArrayAsync();
                var ElementosStockMins = (from eleq in elementos
                                          join unidad in await _context.Unidades.ToListAsync()
                                          on eleq.IDUnidad equals unidad.IDUnidad
                                          join marca in await _context.Marcas.ToListAsync()
                                          on eleq.IDMarca equals marca.IDMarca
                                          select new ElementoEstado
                                          {
                                              Estado = eleq.EstadoAlmacen,
                                              BarCode = eleq.BarCode,
                                              Nombre = eleq.Nombre,
                                              Unidad = unidad.Nombre,
                                              Serial = eleq.Serial,
                                              Marca = marca.Nombre,
                                              Modelo = eleq.Modelo
                                          }
                                          ).ToList();

                return Ok(ElementosStockMins);
                //var queryString = Request.Query;
                //string filter = queryString["$filter"];
                //string auto = queryString["$inlineCount"];
                ////elementos = await _context.ElementoEquipos.Where(r => r.Activo).ToListAsync();
                //if (queryString.Keys.Contains("$inlinecount"))
                //{
                //    StringValues Skip;
                //    StringValues Take;
                //    if (!string.IsNullOrEmpty(filter))
                //    {
                //        string key;
                //        if (filter.Contains("substring")) //searching 
                //        {
                //            key = filter.Split(new string[] { "'" }, StringSplitOptions.None)[1].ToUpper();
                //            elementoEquipos = elementoEquipos.Where(fil => fil.BarCode.ToUpper().Contains(key) || fil.Nombre.ToUpper().Contains(key) || fil.Serial.ToUpper().Contains(key) || fil.Modelo.ToUpper().Contains(key) || fil.Descripcion.ToUpper().Contains(key)).Distinct();
                //        }
                //    }
                //    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                //    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : elementoEquipos.Count();
                //    var count = elementoEquipos.Count();

                //    return new { Items = elementoEquipos.Skip(skip).Take(top), Count = count };
                //}
                //else
                //{
                //    return new { Items = elementoEquipos, Count = elementoEquipos.Count() };
                //}
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        //GetElementosXArea
        // GET: api/<ValuesController1>
        [HttpGet]
        [Route("[action]/{idarea}")]
        public async Task<object> GetElementosXArea(long idarea)
        {
            try
            {
                //Calcular stok elementos
                var empresa = await _context.Empresas.FirstOrDefaultAsync();
                var queryString = Request.Query;
                string filter = queryString["$filter"];
                string auto = queryString["$inlineCount"];
                //elementos = await _context.ElementoEquipos.Where(r => r.Activo).ToListAsync();
                //var tipos = await _context.TipoEquipoElementos.Where(r => r.CodigoUnico == false).Select(r => r.IDTipoEquElem).ToArrayAsync();
                var elementoEquipos = (from area in _context.AreaElementos.Where(r => r.Activo && r.IDArea == idarea).AsQueryable()
                                       join elemento in _context.ElementoEquipos.Where(r => r.Activo && r.IDTipoEquElem != 1).AsQueryable()
                                       on area.BarCode equals elemento.BarCode
                                       select elemento);
                if (queryString.Keys.Contains("$inlinecount"))
                {
                    StringValues Skip;
                    StringValues Take;
                    if (!string.IsNullOrEmpty(filter))
                    {
                        string key;
                        if (filter.Contains("substring")) //searching 
                        {
                            key = filter.Split(new string[] { "'" }, StringSplitOptions.None)[1].ToUpper();
                            elementoEquipos = elementoEquipos.Where(fil => fil.BarCode.ToUpper().Contains(key) || fil.Nombre.ToUpper().Contains(key) || fil.Serial.ToUpper().Contains(key) || fil.Modelo.ToUpper().Contains(key) || fil.Descripcion.ToUpper().Contains(key)).Distinct();
                        }
                    }
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : elementoEquipos.Count();
                    var count = elementoEquipos.Count();
                    return new { Items = elementoEquipos.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = elementoEquipos, Count = elementoEquipos.Count() };
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [HttpGet]
        [Route("[action]/{idtipo}")]
        public async Task<object> GetElementosXTipo(long idtipo)
        {
            try
            {
                var elementos = await _context.ElementoEquipos.Where(r => r.Activo && r.IDTipoEquElem == idtipo).ToListAsync();
                return Ok(elementos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        [Route("[action]")]
        public async Task<object> GetElementosMovimientos()
        {
            try
            {
                //Calcular stok elementos
                var empresa = await _context.Empresas.FirstOrDefaultAsync();
                var personals = _context.Personals.AsQueryable();
                var movimientosElementos = (from movimiento in _context.Movimientos.Where(m => m.Activo).AsQueryable()
                                            join detamovi in _context.MovimientoDetalles.Where(m => m.Activo).AsQueryable()
                                            on movimiento.IDMovimiento equals detamovi.IDMovimiento
                                            join tipomovi in _context.TipoMovimientos.AsQueryable()
                                            on movimiento.IDTipoMovimiento equals tipomovi.IDTipoMovimiento
                                            join area in _context.Areas.AsQueryable()
                                            on movimiento.IDArea equals area.IDArea
                                            join elemento in _context.ElementoEquipos.Where(r => r.Activo).AsQueryable()
                                            on detamovi.BarCode equals elemento.BarCode
                                            select new MovimientosElemento
                                            {
                                                IDMovimientoDetalle = detamovi.IDMovimientoDetalle,
                                                BarCode = detamovi.BarCode,
                                                NombreElemento = elemento.Nombre,
                                                Area = area.Nombre,
                                                TipoMovi = tipomovi.Nombre,
                                                Prefijo = movimiento.Prefijo,
                                                FechaEntrada = movimiento.EntradaFecha,
                                                EntradaCantidad = detamovi.EntradaCantidad,
                                                FechaSalida = movimiento.DespachadoFecha,
                                                SalidaCantidad = detamovi.SalidaCantidad,
                                                Recibido = (movimiento.IDTipoMovimiento == 5 || movimiento.IDTipoMovimiento == 5) ? ((movimiento.EntregaIDPersonal! == null) ? (personals.FirstOrDefault(p => p.IDPersonal == movimiento.EntregaIDPersonal).NombreCompleto) : string.Empty) : ((movimiento.RecibidoPor! == null) ? (personals.FirstOrDefault(p => p.IDPersonal == movimiento.RecibidoPor).NombreCompleto) : string.Empty)
                                            }
                                            ).AsQueryable();
                var eqs = from eq in movimientosElementos where (eq.SalidaCantidad % 2 == 0) select eq;
                return Ok(movimientosElementos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        // GET: api/<ValuesController1>
        [HttpGet]
        [Route("[action]")]
        public async Task<object> GetElementosMovimientosOk()
        {
            try
            {
                //Calcular stok elementos
                var empresa = await _context.Empresas.FirstOrDefaultAsync();
                var queryString = Request.Query;
                string filter = queryString["$filter"];
                string auto = queryString["$inlineCount"];
                var movimientosElementos = (from movimiento in _context.Movimientos.Where(m => m.Activo).AsQueryable()
                                            join detamovi in _context.MovimientoDetalles.Where(m => m.Activo).AsQueryable()
                                            on movimiento.IDMovimiento equals detamovi.IDMovimiento
                                            join tipomovi in _context.TipoMovimientos.AsQueryable()
                                            on movimiento.IDTipoMovimiento equals tipomovi.IDTipoMovimiento
                                            join area in _context.Areas.AsQueryable()
                                            on movimiento.IDArea equals area.IDArea
                                            join elemento in _context.ElementoEquipos.Where(r => r.Activo).AsQueryable()
                                            on detamovi.BarCode equals elemento.BarCode
                                            select new MovimientosElemento
                                            {
                                                IDMovimientoDetalle = detamovi.IDMovimientoDetalle,
                                                BarCode = detamovi.BarCode,
                                                NombreElemento = elemento.Nombre,
                                                Area = area.Nombre,
                                                TipoMovi = tipomovi.Nombre,
                                                Prefijo = movimiento.Prefijo,
                                                FechaEntrada = movimiento.EntradaFecha,
                                                EntradaCantidad = detamovi.EntradaCantidad,
                                                FechaSalida = movimiento.DespachadoFecha,
                                                SalidaCantidad = detamovi.SalidaCantidad
                                            }
                                            ).AsQueryable();
                if (!string.IsNullOrEmpty(filter))
                {
                    if (filter.Contains("substring")) //searching 
                    {
                        string key;
                        key = filter.Split(new string[] { "'" }, StringSplitOptions.None)[1].ToUpper();
                        movimientosElementos = movimientosElementos.Where(fil => fil.BarCode.ToUpper().Contains(key)
                                                || fil.NombreElemento.ToUpper().Contains(key)
                                                || fil.Area.ToUpper().Contains(key)
                                                || fil.TipoMovi.ToUpper().Contains(key)
                                                || fil.Prefijo.ToUpper().Contains(key)
                                                || fil.Prefijo.ToUpper().Contains(key)
                                                || fil.FechaEntrada.ToString().Contains(key)
                                                || fil.EntradaCantidad.ToString().Contains(key)
                                                || fil.FechaSalida.ToString().Contains(key)
                                                || fil.SalidaCantidad.ToString().Contains(key)
                                                );
                    }
                    else
                    {
                        var newfiltersplits = filter;
                        var filtersplits = newfiltersplits.Split('(', ')', ' ');
                        var filterfield = "";
                        var filtervalue = "";

                        if (filtersplits.Length == 7)
                        {
                            filterfield = filtersplits[1];
                            filtervalue = filtersplits[3];
                            if (filtersplits[2] == "tolower")
                            {
                                filterfield = filter.Split('(', ')', '\'')[3];
                                filtervalue = filter.Split('(', ')', '\'')[5];
                            }
                        }
                        else if (filtersplits.Length == 13)
                        {
                            filterfield = filtersplits[9];
                            filtervalue = filtersplits[11];
                        }
                        else if (filtersplits.Length == 5)
                        {
                            filterfield = filtersplits[1];
                            filtervalue = filtersplits[3];
                        }
                        switch (filterfield)
                        {
                            case "BarCode":
                                movimientosElementos = (from cust in movimientosElementos
                                                        where cust.BarCode == filtervalue.ToString()
                                                        select cust);
                                break;
                            case "NombreElemento":
                                movimientosElementos = (from cust in movimientosElementos
                                                        where cust.NombreElemento.ToLower().StartsWith(filtervalue.ToLower().ToString())
                                                        select cust);
                                break;
                            case "Area":
                                movimientosElementos = (from cust in movimientosElementos
                                                        where cust.Area.ToLower().StartsWith(filtervalue.ToLower().ToString())
                                                        select cust);
                                break;
                            case "TipoMovi":
                                movimientosElementos = (from cust in movimientosElementos
                                                        where cust.TipoMovi.ToLower().StartsWith(filtervalue.ToLower().ToString())
                                                        select cust);
                                break;
                            case "Prefijo":
                                movimientosElementos = (from cust in movimientosElementos
                                                        where cust.Prefijo.ToLower().StartsWith(filtervalue.ToLower().ToString())
                                                        select cust);
                                break;
                            case "FechaEntrada":
                                movimientosElementos = (from cust in movimientosElementos
                                                        where cust.FechaEntrada.ToString().StartsWith(filtervalue.ToLower().ToString())
                                                        select cust);
                                break;
                            case "FechaSalida":
                                movimientosElementos = (from cust in movimientosElementos
                                                        where cust.FechaSalida.ToString().StartsWith(filtervalue.ToLower().ToString())
                                                        select cust);
                                break;
                            case "EntradaCantidad":
                                movimientosElementos = (from cust in movimientosElementos
                                                        where cust.EntradaCantidad.ToString().StartsWith(filtervalue.ToLower().ToString())
                                                        select cust);
                                break;
                            case "SalidaCantidad":
                                movimientosElementos = (from cust in movimientosElementos
                                                        where cust.SalidaCantidad.ToString().StartsWith(filtervalue.ToLower().ToString())
                                                        select cust);
                                break;
                        }
                    }
                }
                if (queryString.Keys.Contains("$inlinecount"))
                {
                    StringValues Skip;
                    StringValues Take;
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : movimientosElementos.Count();
                    var count = movimientosElementos.Count();
                    return new { Items = movimientosElementos.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = movimientosElementos, Count = movimientosElementos.Count() };
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/<ValuesController1>
        [HttpGet]
        [Route("[action]")]
        public async Task<object> GetEntregasEPPxPersonal()
        {
            try
            {
                //Calcular stok elementos
                var sql = @"EXEC sp_getEntregasEPP";
                //var sp_elementos = _context.sp_getEntregasEPP.FromSqlRaw(sql).AsQueryable();
                var sp_elementos = await _context.sp_getEntregasEPP.FromSqlRaw(sql).ToListAsync();
                //return new { Items = sp_elementos, Count = sp_elementos.Count() };
                return Ok(sp_elementos);
                //var queryString = Request.Query;
                //string filter = queryString["$filter"];
                //string auto = queryString["$inlineCount"];
                //if (!string.IsNullOrEmpty(filter))
                //{
                //    if (filter.Contains("substring")) //searching 
                //    {
                //        string key;
                //        key = filter.Split(new string[] { "'" }, StringSplitOptions.None)[1].ToUpper();
                //        sp_elementos = sp_elementos.Where(fil => fil.BarCode.ToUpper().Contains(key)
                //                                || fil.Nombre.ToUpper().Contains(key)
                //                                || fil.NombreTipo.ToUpper().Contains(key)
                //                                || fil.NombreCompleto.ToUpper().Contains(key)
                //                                || fil.EntregaFecha.ToString().Contains(key)
                //                                );
                //    }
                //    else
                //    {
                //        var newfiltersplits = filter;
                //        var filtersplits = newfiltersplits.Split('(', ')', ' ');
                //        var filterfield = "";
                //        var filtervalue = "";

                //        if (filtersplits.Length == 7)
                //        {
                //            filterfield = filtersplits[1];
                //            filtervalue = filtersplits[3];
                //            if (filtersplits[2] == "tolower")
                //            {
                //                filterfield = filter.Split('(', ')', '\'')[3];
                //                filtervalue = filter.Split('(', ')', '\'')[5];
                //            }
                //        }
                //        else if (filtersplits.Length == 13)
                //        {
                //            filterfield = filtersplits[9];
                //            filtervalue = filtersplits[11];
                //        }
                //        else if (filtersplits.Length == 5)
                //        {
                //            filterfield = filtersplits[1];
                //            filtervalue = filtersplits[3];
                //        }
                //        switch (filterfield)
                //        {
                //            case "BarCode":
                //                sp_elementos = (from cust in sp_elementos
                //                                where cust.BarCode == filtervalue.ToString()
                //                                        select cust);
                //                break;
                //            case "Nombre":
                //                sp_elementos = (from cust in sp_elementos
                //                                where cust.Nombre.ToLower().StartsWith(filtervalue.ToLower().ToString())
                //                                        select cust);
                //                break;
                //            case "NombreTipo":
                //                sp_elementos = (from cust in sp_elementos
                //                                where cust.NombreTipo.ToLower().StartsWith(filtervalue.ToLower().ToString())
                //                                        select cust);
                //                break;
                //            case "NombreCompleto":
                //                sp_elementos = (from cust in sp_elementos
                //                                where cust.NombreCompleto.ToLower().StartsWith(filtervalue.ToLower().ToString())
                //                                        select cust);
                //                break;
                //            case "EntregaFecha":
                //                sp_elementos = (from cust in sp_elementos
                //                                where cust.EntregaFecha.ToString().StartsWith(filtervalue.ToLower().ToString())
                //                                        select cust);
                //                break;
                //        }
                //    }
                //}
                //if (queryString.Keys.Contains("$inlinecount"))
                //{
                //    StringValues Skip;
                //    StringValues Take;
                //    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                //    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : sp_elementos.Count();
                //    var count = sp_elementos.AsEnumerable().Count();
                //    return new { Items = sp_elementos.Skip(skip).Take(top), Count = count };
                //}
                //else
                //{
                //    return new { Items = sp_elementos, Count = sp_elementos.Count() };
                //}
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        // GET: api/<ValuesController1>
        [HttpGet]
        [Route("[action]")]
        public async Task<object> GetEntregasMyS()
        {
            try
            {
                //Calcular stok elementos
                var sql = @"EXEC sp_getEntregasMyS";
                //var sp_elementos = _context.sp_getEntregasEPP.FromSqlRaw(sql).AsQueryable();
                var sp_elementos = await _context.sp_getEntregasEPP.FromSqlRaw(sql).ToListAsync();
                //return new { Items = sp_elementos, Count = sp_elementos.Count() };
                return Ok(sp_elementos);
                //var queryString = Request.Query;
                //string filter = queryString["$filter"];
                //string auto = queryString["$inlineCount"];
                //if (!string.IsNullOrEmpty(filter))
                //{
                //    if (filter.Contains("substring")) //searching 
                //    {
                //        string key;
                //        key = filter.Split(new string[] { "'" }, StringSplitOptions.None)[1].ToUpper();
                //        sp_elementos = sp_elementos.Where(fil => fil.BarCode.ToUpper().Contains(key)
                //                                || fil.Nombre.ToUpper().Contains(key)
                //                                || fil.NombreTipo.ToUpper().Contains(key)
                //                                || fil.NombreCompleto.ToUpper().Contains(key)
                //                                || fil.EntregaFecha.ToString().Contains(key)
                //                                );
                //    }
                //    else
                //    {
                //        var newfiltersplits = filter;
                //        var filtersplits = newfiltersplits.Split('(', ')', ' ');
                //        var filterfield = "";
                //        var filtervalue = "";

                //        if (filtersplits.Length == 7)
                //        {
                //            filterfield = filtersplits[1];
                //            filtervalue = filtersplits[3];
                //            if (filtersplits[2] == "tolower")
                //            {
                //                filterfield = filter.Split('(', ')', '\'')[3];
                //                filtervalue = filter.Split('(', ')', '\'')[5];
                //            }
                //        }
                //        else if (filtersplits.Length == 13)
                //        {
                //            filterfield = filtersplits[9];
                //            filtervalue = filtersplits[11];
                //        }
                //        else if (filtersplits.Length == 5)
                //        {
                //            filterfield = filtersplits[1];
                //            filtervalue = filtersplits[3];
                //        }
                //        switch (filterfield)
                //        {
                //            case "BarCode":
                //                sp_elementos = (from cust in sp_elementos
                //                                where cust.BarCode == filtervalue.ToString()
                //                                        select cust);
                //                break;
                //            case "Nombre":
                //                sp_elementos = (from cust in sp_elementos
                //                                where cust.Nombre.ToLower().StartsWith(filtervalue.ToLower().ToString())
                //                                        select cust);
                //                break;
                //            case "NombreTipo":
                //                sp_elementos = (from cust in sp_elementos
                //                                where cust.NombreTipo.ToLower().StartsWith(filtervalue.ToLower().ToString())
                //                                        select cust);
                //                break;
                //            case "NombreCompleto":
                //                sp_elementos = (from cust in sp_elementos
                //                                where cust.NombreCompleto.ToLower().StartsWith(filtervalue.ToLower().ToString())
                //                                        select cust);
                //                break;
                //            case "EntregaFecha":
                //                sp_elementos = (from cust in sp_elementos
                //                                where cust.EntregaFecha.ToString().StartsWith(filtervalue.ToLower().ToString())
                //                                        select cust);
                //                break;
                //        }
                //    }
                //}
                //if (queryString.Keys.Contains("$inlinecount"))
                //{
                //    StringValues Skip;
                //    StringValues Take;
                //    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                //    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : sp_elementos.Count();
                //    var count = sp_elementos.AsEnumerable().Count();
                //    return new { Items = sp_elementos.Skip(skip).Take(top), Count = count };
                //}
                //else
                //{
                //    return new { Items = sp_elementos, Count = sp_elementos.Count() };
                //}
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [HttpGet]
        [Route("GetElemento/{codigo}")]
        public async Task<IActionResult> GetElemento(string codigo)
        {
            var elemento = new ElementoEquipo();
            try
            {
                elemento = await _context.ElementoEquipos.FindAsync(codigo);
                if (elemento != null)
                {
                    if (_context.MovimientoDetalles.Any(r => r.BarCode == codigo && r.Activo) || _context.AreaElementos.Any(r => r.BarCode == codigo && r.Activo))
                        elemento.flag = true;
                    else
                        elemento.flag = false;
                    return Ok(elemento);
                }
                else
                    return Ok(new ElementoEquipo());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("GetExistenciasArea/{codigo}/{idarea}")]
        public async Task<IActionResult> GetExistenciasArea(string codigo, long idarea)
        {
            var elemento = new ElementoEquipo();
            try
            {
                elemento = await _context.ElementoEquipos.FindAsync(codigo);
                var arealemento = new AreaElemento();
                TipoEquipoElemento tipoele = await _context.TipoEquipoElementos.FindAsync(elemento.IDTipoEquElem);
                if (elemento != null)
                {
                    //Se sacan los elementos de cualquier área
                    if (tipoele.CodigoUnico)
                    {
                        arealemento = await _context.AreaElementos.FirstOrDefaultAsync(r => r.BarCode == codigo && r.Activo);
                    }
                    else
                    {
                        arealemento = await _context.AreaElementos.FirstOrDefaultAsync(r => r.BarCode == codigo && r.Activo);
                    }
                    //arealemento = await _context.AreaElementos.FirstOrDefaultAsync(r => r.BarCode == codigo && r.IDArea == idarea && r.Activo);
                    if (arealemento == null)
                    {
                        arealemento = new AreaElemento { BarCode ="N/A", IDArea=99};
                    }
                    return Ok(arealemento);
                }
                else
                    return Ok(new ElementoEquipo());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ElementoEquipo elementoEquipo)
        {
            try
            {
                var marcas = await _context.Marcas.ToListAsync();
                var tipos = await _context.TipoEquipoElementos.ToListAsync();
                var condicionEqs = await _context.CondicionEquipos.ToListAsync();
                var marca = await _context.Marcas.FindAsync(elementoEquipo.IDMarca);
                var tipo = await _context.TipoEquipoElementos.FindAsync(elementoEquipo.IDTipoEquElem);
                var condicionEq = await _context.CondicionEquipos.FindAsync(elementoEquipo.IDCondicionEquipo);
                var strmarca = string.Empty;
                var strtipo = string.Empty;
                var strserial = string.Empty;
                if (marca != null)
                    strmarca = marca.Nombre;
                else
                {
                    marca = marcas.FirstOrDefault();
                    strmarca = marca.Nombre;
                    elementoEquipo.IDMarca = marca.IDMarca;
                }
                if (condicionEqs == null)
                    elementoEquipo.IDCondicionEquipo = condicionEqs.FirstOrDefault().IDCondicionEquipo;
                //elementoEquipo.Nombre = elementoEquipo.Nombre.ToUpper();
                //elementoEquipo.Serial = elementoEquipo.Serial.ToUpper();
                //elementoEquipo.Descripcion = elementoEquipo.Descripcion.ToUpper();
                strtipo = tipo.NombreTipo;
                elementoEquipo.EstadoAlmacen = "EN BODEGA";
                string strqr = "Código: " + elementoEquipo.BarCode + "\r\n" +
                    "Nombre: " + elementoEquipo.Nombre + "\r\n";
                strtipo = (string.IsNullOrEmpty(strtipo)) ? strtipo : ("Tipo: " + strtipo);
                strmarca = (string.IsNullOrEmpty(strmarca)) ? strmarca : ("Marca: " + strmarca);
                strserial = (string.IsNullOrEmpty(strserial)) ? strserial : ("Marca: " + elementoEquipo.Serial);
                strqr = strqr + strtipo + strmarca + strserial;
                elementoEquipo.QRCode = strqr;
                _context.ElementoEquipos.Add(elementoEquipo);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // PUT api/<ValuesController1>/5
        [HttpPut]
        public async Task<IActionResult> Put(long id, [FromBody] ElementoEquipo elementoEquipo)
        {
            try
            {
                var marca = await _context.Marcas.FindAsync(elementoEquipo.IDMarca);
                var tipo = await _context.TipoEquipoElementos.FindAsync(elementoEquipo.IDTipoEquElem);
                var strmarca = string.Empty;
                var strtipo = string.Empty;
                var strserial = string.Empty;
                //elementoEquipo.Nombre = elementoEquipo.Nombre.ToUpper();
                //elementoEquipo.Serial = elementoEquipo.Serial.ToUpper();
                //elementoEquipo.Descripcion = elementoEquipo.Descripcion.ToUpper();
                if (marca != null)
                    strmarca = marca.Nombre;
                if (tipo != null)
                    strtipo = tipo.NombreTipo;
                if (!string.IsNullOrEmpty(elementoEquipo.Serial))
                    strserial = elementoEquipo.Serial;
                string strqr = "Código: " + elementoEquipo.BarCode + "\r\n" +
                    "Nombre: " + elementoEquipo.Nombre + "\r\n";
                strtipo = (string.IsNullOrEmpty(strtipo)) ? strtipo : ("Tipo: " + strtipo);
                strmarca = (string.IsNullOrEmpty(strmarca)) ? strmarca : ("Marca: " + strmarca);
                strserial = (string.IsNullOrEmpty(strserial)) ? strserial : ("Serial: " + elementoEquipo.Serial);
                strqr = strqr + strtipo + "\r\n" + strmarca + "\r\n" + strserial;
                elementoEquipo.QRCode = strqr;
                _context.Entry(elementoEquipo).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // DELETE api/<ValuesController1>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var elementoEquipo = await _context.ElementoEquipos.FindAsync(id);
                if (elementoEquipo != null)
                {
                    //_context.Remove(elementoEquipo);
                    elementoEquipo.Activo = false;
                    _context.Entry(elementoEquipo).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
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
