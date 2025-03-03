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
    public class SalidasController : ControllerBase
    {
        private readonly gilgalContext _context;

        public SalidasController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var movimientos = await _context.Movimientos.Where(r => r.Activo && r.IDTipoMovimiento == 4).ToListAsync();
                var salidas = (from movimiento in movimientos
                               select new Salida
                               {
                                   IDMovimiento = movimiento.IDMovimiento,
                                   Fecha = movimiento.Fecha,
                                   IDTipoMovimiento = movimiento.IDTipoMovimiento,
                                   Consecutivo = movimiento.Consecutivo,
                                   Prefijo = movimiento.Prefijo,
                                   NoRequisicion = (long)movimiento.NoRequisicion,
                                   NoOrdenCompra = movimiento.NoOrdenCompra,
                                   IDOrigen = (long)movimiento.IDOrigen,
                                   IDDestino = (long)movimiento.IDDestino,
                                   Conceptos = movimiento.Conceptos,
                                   OtroConcepto = movimiento.OtroConcepto,
                                   EntradaRecibidoPor = movimiento.EntradaRecibidoPor,
                                   EntradaFecha = movimiento.EntradaFecha,
                                   EntradaLugar = movimiento.EntradaLugar,
                                   EntradaResponsableRecepcion = movimiento.EntradaResponsableRecepcion,
                                   EntradaRecibidoFirma = movimiento.EntradaRecibidoFirma,
                                   DespachadoPor = (long)movimiento.DespachadoPor,
                                   DespachadoPorNombre = movimiento.DespachadoPorNombre,
                                   DespachadoPorCargo = movimiento.DespachadoPorCargo,
                                   DespachadoPorCC = movimiento.DespachadoPorCC,
                                   DespachadoFecha = (DateTime)movimiento.DespachadoFecha,
                                   DespachadoFirma = movimiento.DespachadoFirma,
                                   RecibidoPor = (long)movimiento.RecibidoPor,
                                   RecibidoPorNombre = movimiento.RecibidoPorNombre,
                                   RecibidoPorCargo = movimiento.RecibidoPorCargo,
                                   RecibidoPorCC = movimiento.RecibidoPorCC,
                                   RecibidoFecha = (DateTime)movimiento.RecibidoFecha,
                                   RecibidoFirma = movimiento.RecibidoFirma,
                                   TransportadoNombre = movimiento.TransportadoNombre,
                                   TransportadoCC = movimiento.TransportadoCC,
                                   TransportadoCargo = movimiento.TransportadoCargo,
                                   TransportadoPlaca = movimiento.TransportadoPlaca,
                                   TransportadoTipoVehiculo = movimiento.TransportadoTipoVehiculo,
                                   TransportadoFecha = (DateTime)movimiento.TransportadoFecha,
                                   Observaciones = movimiento.Observaciones,
                                   ArchivoDigital = movimiento.ArchivoDigital,
                                   EstadoMovimiento = movimiento.EstadoMovimiento,
                                   EntregaIDPersonal = movimiento.EntregaIDPersonal,
                                   EntregaNombre = movimiento.EntregaNombre,
                                   EntregaCargo = movimiento.EntregaCargo,
                                   EntregaCedulaNit = movimiento.EntregaCedulaNit,
                                   EntregaTelContacto = movimiento.EntregaTelContacto,
                                   IDCentroCosto = movimiento.IDCentroCosto,
                                   IDArea = movimiento.IDArea,
                                   Activo = movimiento.Activo,
                                   FlagIn = movimiento.FlagIn,
                                   FlagOut = movimiento.FlagOut
                               }).AsQueryable();
                var queryString = Request.Query;
                string filter = queryString["$filter"];
                string auto = queryString["$inlineCount"];
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
                            salidas = salidas.Where(fil => fil.Prefijo.ToUpper().Contains(key) || fil.Conceptos.ToUpper().Contains(key) || fil.Fecha.ToString().Contains(key)).Distinct();
                        }
                    }
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : salidas.Count();
                    var count = salidas.Count();
                    return new { Items = salidas.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = salidas, Count = salidas.Count() };
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
        public async Task<object> GetSalidasAlmacen()
        {
            try
            {
                var personalmov = await _context.Personals.Where(r => r.Activo && r.ResponsableInventario.Contains("Almacén")).Select(r => r.IDPersonal).ToListAsync();
                var movimientos = _context.Movimientos.ToList();
                foreach (var movimi in movimientos)
                {
                    if (movimi.IDTipoMovimiento == 4)
                    {
                        if (movimi.DespachadoPor == null)
                            movimi.Activo = false;
                        else
                        {
                            if (!personalmov.Contains((long)movimi.DespachadoPor))
                                if (movimi.EntradaRecibidoPor != null)
                                {
                                    if (!personalmov.Contains((long)movimi.EntradaRecibidoPor))
                                        movimi.Activo = false;
                                }
                                else
                                {
                                    movimi.Activo = false;
                                }
                        }
                    }
                }
                var salidas = (from movimiento in movimientos.Where(r => r.Activo)
                               select new Salida
                               {
                                   IDMovimiento = movimiento.IDMovimiento,
                                   Fecha = movimiento.Fecha,
                                   IDTipoMovimiento = movimiento.IDTipoMovimiento,
                                   Consecutivo = movimiento.Consecutivo,
                                   Prefijo = movimiento.Prefijo,
                                   NoRequisicion = (long)movimiento.NoRequisicion,
                                   NoOrdenCompra = movimiento.NoOrdenCompra,
                                   IDOrigen = (long)movimiento.IDOrigen,
                                   IDDestino = (long)movimiento.IDDestino,
                                   Conceptos = movimiento.Conceptos,
                                   OtroConcepto = movimiento.OtroConcepto,
                                   EntradaRecibidoPor = movimiento.EntradaRecibidoPor,
                                   EntradaFecha = movimiento.EntradaFecha,
                                   EntradaLugar = movimiento.EntradaLugar,
                                   EntradaResponsableRecepcion = movimiento.EntradaResponsableRecepcion,
                                   EntradaRecibidoFirma = movimiento.EntradaRecibidoFirma,
                                   DespachadoPor = (long)movimiento.DespachadoPor,
                                   DespachadoPorNombre = movimiento.DespachadoPorNombre,
                                   DespachadoPorCargo = movimiento.DespachadoPorCargo,
                                   DespachadoPorCC = movimiento.DespachadoPorCC,
                                   DespachadoFecha = (DateTime)movimiento.DespachadoFecha,
                                   DespachadoFirma = movimiento.DespachadoFirma,
                                   RecibidoPor = (long)movimiento.RecibidoPor,
                                   RecibidoPorNombre = movimiento.RecibidoPorNombre,
                                   RecibidoPorCargo = movimiento.RecibidoPorCargo,
                                   RecibidoPorCC = movimiento.RecibidoPorCC,
                                   RecibidoFecha = (DateTime)movimiento.RecibidoFecha,
                                   RecibidoFirma = movimiento.RecibidoFirma,
                                   TransportadoNombre = movimiento.TransportadoNombre,
                                   TransportadoCC = movimiento.TransportadoCC,
                                   TransportadoCargo = movimiento.TransportadoCargo,
                                   TransportadoPlaca = movimiento.TransportadoPlaca,
                                   TransportadoTipoVehiculo = movimiento.TransportadoTipoVehiculo,
                                   TransportadoFecha = (DateTime)movimiento.TransportadoFecha,
                                   Observaciones = movimiento.Observaciones,
                                   ArchivoDigital = movimiento.ArchivoDigital,
                                   EstadoMovimiento = movimiento.EstadoMovimiento,
                                   EntregaIDPersonal = movimiento.EntregaIDPersonal,
                                   EntregaNombre = movimiento.EntregaNombre,
                                   EntregaCargo = movimiento.EntregaCargo,
                                   EntregaCedulaNit = movimiento.EntregaCedulaNit,
                                   EntregaTelContacto = movimiento.EntregaTelContacto,
                                   IDCentroCosto = movimiento.IDCentroCosto,
                                   IDArea = movimiento.IDArea,
                                   Activo = movimiento.Activo,
                                   FlagIn = movimiento.FlagIn,
                                   FlagOut = movimiento.FlagOut
                               }).AsQueryable();
                var queryString = Request.Query;
                string filter = queryString["$filter"];
                string auto = queryString["$inlineCount"];
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
                            salidas = salidas.Where(fil => fil.Prefijo.ToUpper().Contains(key) || fil.Conceptos.ToUpper().Contains(key) || fil.Fecha.ToString().Contains(key)).Distinct();
                        }
                    }
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : salidas.Count();
                    var count = salidas.Count();
                    return new { Items = salidas.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = salidas, Count = salidas.Count() };
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
        public async Task<object> GetSalidasEPP()
        {
            try
            {
                var personalmov = await _context.Personals.Where(r => r.Activo && r.ResponsableInventario.Contains("EPP")).Select(r => r.IDPersonal).ToListAsync();
                var movimientos = await _context.Movimientos.Where(r => r.Activo && r.IDTipoMovimiento == 4).ToListAsync();
                foreach (var movimi in movimientos)
                {
                    if (movimi.IDTipoMovimiento == 4)
                    {
                        if (movimi.DespachadoPor == null)
                            movimi.Activo = false;
                        else
                        {
                            if (!personalmov.Contains((long)movimi.DespachadoPor))
                                if (movimi.EntradaRecibidoPor != null)
                                {
                                    if (!personalmov.Contains((long)movimi.EntradaRecibidoPor))
                                        movimi.Activo = false;
                                }
                                else
                                {
                                    movimi.Activo = false;
                                }
                        }
                    }
                }
                var salidas = (from movimiento in movimientos.Where(r => r.Activo)
                               select new Salida
                               {
                                   IDMovimiento = movimiento.IDMovimiento,
                                   Fecha = movimiento.Fecha,
                                   IDTipoMovimiento = movimiento.IDTipoMovimiento,
                                   Consecutivo = movimiento.Consecutivo,
                                   Prefijo = movimiento.Prefijo,
                                   NoRequisicion = (long)movimiento.NoRequisicion,
                                   NoOrdenCompra = movimiento.NoOrdenCompra,
                                   IDOrigen = (long)movimiento.IDOrigen,
                                   IDDestino = (long)movimiento.IDDestino,
                                   Conceptos = movimiento.Conceptos,
                                   OtroConcepto = movimiento.OtroConcepto,
                                   EntradaRecibidoPor = movimiento.EntradaRecibidoPor,
                                   EntradaFecha = movimiento.EntradaFecha,
                                   EntradaLugar = movimiento.EntradaLugar,
                                   EntradaResponsableRecepcion = movimiento.EntradaResponsableRecepcion,
                                   EntradaRecibidoFirma = movimiento.EntradaRecibidoFirma,
                                   DespachadoPor = (long)movimiento.DespachadoPor,
                                   DespachadoPorNombre = movimiento.DespachadoPorNombre,
                                   DespachadoPorCargo = movimiento.DespachadoPorCargo,
                                   DespachadoPorCC = movimiento.DespachadoPorCC,
                                   DespachadoFecha = (DateTime)movimiento.DespachadoFecha,
                                   DespachadoFirma = movimiento.DespachadoFirma,
                                   RecibidoPor = (long)movimiento.RecibidoPor,
                                   RecibidoPorNombre = movimiento.RecibidoPorNombre,
                                   RecibidoPorCargo = movimiento.RecibidoPorCargo,
                                   RecibidoPorCC = movimiento.RecibidoPorCC,
                                   RecibidoFecha = (DateTime)movimiento.RecibidoFecha,
                                   RecibidoFirma = movimiento.RecibidoFirma,
                                   TransportadoNombre = movimiento.TransportadoNombre,
                                   TransportadoCC = movimiento.TransportadoCC,
                                   TransportadoCargo = movimiento.TransportadoCargo,
                                   TransportadoPlaca = movimiento.TransportadoPlaca,
                                   TransportadoTipoVehiculo = movimiento.TransportadoTipoVehiculo,
                                   TransportadoFecha = (DateTime)movimiento.TransportadoFecha,
                                   Observaciones = movimiento.Observaciones,
                                   ArchivoDigital = movimiento.ArchivoDigital,
                                   EstadoMovimiento = movimiento.EstadoMovimiento,
                                   EntregaIDPersonal = movimiento.EntregaIDPersonal,
                                   EntregaNombre = movimiento.EntregaNombre,
                                   EntregaCargo = movimiento.EntregaCargo,
                                   EntregaCedulaNit = movimiento.EntregaCedulaNit,
                                   EntregaTelContacto = movimiento.EntregaTelContacto,
                                   IDCentroCosto = movimiento.IDCentroCosto,
                                   IDArea = movimiento.IDArea,
                                   Activo = movimiento.Activo,
                                   FlagIn = movimiento.FlagIn,
                                   FlagOut = movimiento.FlagOut
                               }).AsQueryable();
                var queryString = Request.Query;
                string filter = queryString["$filter"];
                string auto = queryString["$inlineCount"];
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
                            salidas = salidas.Where(fil => fil.Prefijo.ToUpper().Contains(key) || fil.Conceptos.ToUpper().Contains(key) || fil.Fecha.ToString().Contains(key)).Distinct();
                        }
                    }
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : salidas.Count();
                    var count = salidas.Count();
                    return new { Items = salidas.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = salidas, Count = salidas.Count() };
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // GET: api/<ValuesController1>
        [HttpGet("{id}", Name = "GetSalida")]
        public async Task<object> GetSalida(long id)
        {
            try
            {
                var movimiento = await _context.Movimientos.FindAsync(id);
                Salida salida = (Salida)movimiento;
                return Ok(salida);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Salida salida)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var formato = new Formato();
                    formato = await _context.Formatos.FirstOrDefaultAsync(f => f.Nombre == "REMISIÓN DE ALMACÉN");
                    Movimiento movimiento = (Movimiento)salida;
                    movimiento.EstadoMovimiento = "ABIERTO";
                    movimiento.Consecutivo = formato.Consecutivo + 1;
                    formato.Consecutivo = formato.Consecutivo + 1;
                    movimiento.Prefijo = string.Format("{0}-{1:0000}", formato.Prefijo, formato.Consecutivo);
                    _context.Formatos.Update(formato);
                    var personal = await _context.Personals.FindAsync(movimiento.DespachadoPor);
                    movimiento.DespachadoPorNombre = personal.NombreCompleto;
                    movimiento.DespachadoPorCC = personal.NoIdentificacion;
                    movimiento.DespachadoPorCargo = personal.Cargo;
                    personal = await _context.Personals.FindAsync(movimiento.RecibidoPor);
                    movimiento.RecibidoPorNombre = personal.NombreCompleto;
                    movimiento.RecibidoPorCC = personal.NoIdentificacion;
                    movimiento.RecibidoPorCargo = personal.Cargo;
                    _context.Movimientos.Add(movimiento);
                    await _context.SaveChangesAsync();
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

        // POST api/<ValuesController1>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> PostJson(MovimientoFirma movimientoFirma)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var movimiento = await _context.Movimientos.FindAsync(movimientoFirma.IDMovimiento);
                    movimiento.RecibidoFirma = (string.IsNullOrEmpty(movimientoFirma.RecibidoFirma) && movimientoFirma.RecibidoFirma.Length > 10) ? movimiento.RecibidoFirma : movimientoFirma.RecibidoFirma;
                    _context.Entry(movimiento).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
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



        // PUT api/<ValuesController1>/5
        [HttpPut]
        public async Task<IActionResult> Put(long id, [FromBody] Salida salida)
        {
            try
            {
                Movimiento movimiento = (Movimiento)salida;
                var personal = await _context.Personals.FindAsync(movimiento.DespachadoPor);
                movimiento.DespachadoPorNombre = personal.NombreCompleto;
                movimiento.DespachadoPorCC = personal.NoIdentificacion;
                movimiento.DespachadoPorCargo = personal.Cargo;
                personal = await _context.Personals.FindAsync(movimiento.RecibidoPor);
                movimiento.RecibidoPorNombre = personal.NombreCompleto;
                movimiento.RecibidoPorCC = personal.NoIdentificacion;
                movimiento.RecibidoPorCargo = personal.Cargo;
                if (salida.EntradaRecibidoPor != null)
                {
                    personal = await _context.Personals.FindAsync(movimiento.EntradaRecibidoPor);
                    movimiento.EntradaResponsableRecepcion = personal.NombreCompleto;

                }
                _context.Entry(movimiento).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        // DELETE api/<ValuesController1>/5
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Delete(MovimientoFirma movimientoFirma)
        {
            try
            {
                Movimiento movimiento = await _context.Movimientos.FindAsync(movimientoFirma.IDMovimiento);
                if (movimiento != null)
                {
                    string sql = @"EXEC sp_ActualizarMovimientoES @idMov = " + movimientoFirma.IDMovimiento;
                    List<Movimiento> sp_elementos = await _context.sp_ActualizarMovimientoES.FromSqlRaw(sql).ToListAsync();
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
                //return new { Items = result, Count = result.Count() };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
