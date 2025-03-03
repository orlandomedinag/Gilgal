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
    public class EntradasController : ControllerBase
    {
        private readonly gilgalContext _context;

        public EntradasController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var movimientos = await _context.Movimientos.Where(r => r.Activo && r.IDTipoMovimiento == 2).ToListAsync();
                var entradas = (from movimiento in movimientos
                                select new Entrada
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
                                    DespachadoPor = movimiento.DespachadoPor,
                                    DespachadoPorNombre = movimiento.DespachadoPorNombre,
                                    DespachadoPorCargo = movimiento.DespachadoPorCargo,
                                    IDAreaFuncional = movimiento.IDAreaFuncional,
                                    DespachadoPorCC = movimiento.DespachadoPorCC,
                                    DespachadoFecha = (DateTime)movimiento.DespachadoFecha,
                                    DespachadoFirma = movimiento.DespachadoFirma,
                                    RecibidoPor = (long)movimiento.RecibidoPor,
                                    RecibidoFecha = (DateTime)movimiento.RecibidoFecha,
                                    RecibidoFirma = movimiento.RecibidoFirma,
                                    TransportadoNombre = movimiento.TransportadoNombre,
                                    TransportadoCC = movimiento.TransportadoCC,
                                    TransportadoCargo = movimiento.TransportadoCargo,
                                    TransportadoPlaca = movimiento.TransportadoPlaca,
                                    TransportadoTipoVehiculo = movimiento.TransportadoTipoVehiculo,
                                    TransportadoFecha = movimiento.TransportadoFecha,
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
                            entradas = entradas.Where(fil => fil.Prefijo.ToUpper().Contains(key) || fil.Conceptos.ToUpper().Contains(key) || fil.Fecha.ToString().Contains(key)).Distinct();
                        }
                    }
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : entradas.Count();
                    var count = entradas.Count();
                    return new { Items = entradas.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = entradas, Count = entradas.Count() };
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
        public async Task<object> GetAlmacen()
        {
            try
            {
                var personalmov = await _context.Personals.Where(r => r.Activo && r.ResponsableInventario.Contains("Almacén")).Select(r => r.IDPersonal).ToListAsync();
                var movimientos = await _context.Movimientos.Where(r => r.Activo && r.IDTipoMovimiento == 2).ToListAsync();
                foreach (var movimi in movimientos)
                {
                    if (movimi.RecibidoPor == null)
                        movimi.Activo = false;
                    else
                    {
                        if (!personalmov.Contains((long)movimi.RecibidoPor))
                            movimi.Activo = false;
                    }
                }
                movimientos = movimientos.Where(r => r.Activo).ToList();
                var entradas = (from movimiento in movimientos
                                select new Entrada
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
                                    DespachadoPor = movimiento.DespachadoPor,
                                    DespachadoPorNombre = movimiento.DespachadoPorNombre,
                                    DespachadoPorCargo = movimiento.DespachadoPorCargo,
                                    IDAreaFuncional = movimiento.IDAreaFuncional,
                                    DespachadoPorCC = movimiento.DespachadoPorCC,
                                    DespachadoFecha = (DateTime)movimiento.DespachadoFecha,
                                    DespachadoFirma = movimiento.DespachadoFirma,
                                    RecibidoPor = (long)movimiento.RecibidoPor,
                                    RecibidoFecha = (DateTime)movimiento.RecibidoFecha,
                                    RecibidoFirma = movimiento.RecibidoFirma,
                                    TransportadoNombre = movimiento.TransportadoNombre,
                                    TransportadoCC = movimiento.TransportadoCC,
                                    TransportadoCargo = movimiento.TransportadoCargo,
                                    TransportadoPlaca = movimiento.TransportadoPlaca,
                                    TransportadoTipoVehiculo = movimiento.TransportadoTipoVehiculo,
                                    TransportadoFecha = movimiento.TransportadoFecha,
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
                            entradas = entradas.Where(fil => fil.Prefijo.ToUpper().Contains(key) || fil.Conceptos.ToUpper().Contains(key) || fil.Fecha.ToString().Contains(key)).Distinct();
                        }
                    }
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : entradas.Count();
                    var count = entradas.Count();
                    return new { Items = entradas.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = entradas, Count = entradas.Count() };
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
        public async Task<object> GetEPP()
        {
            try
            {
                var personalmov = await _context.Personals.Where(r => r.Activo && r.ResponsableInventario.Contains("EPP")).Select(r => r.IDPersonal).ToListAsync();
                var movimientos = await _context.Movimientos.Where(r => r.Activo && r.IDTipoMovimiento == 2).ToListAsync();
                foreach (var movimi in movimientos)
                {
                    if (movimi.RecibidoPor == null)
                        movimi.Activo = false;
                    else
                    {
                        if (!personalmov.Contains((long)movimi.RecibidoPor))
                            movimi.Activo = false;
                    }
                }
                var entradas = (from movimiento in movimientos.Where(r => r.Activo)
                                select new Entrada
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
                                    DespachadoPor = movimiento.DespachadoPor,
                                    DespachadoPorNombre = movimiento.DespachadoPorNombre,
                                    DespachadoPorCargo = movimiento.DespachadoPorCargo,
                                    IDAreaFuncional = movimiento.IDAreaFuncional,
                                    DespachadoPorCC = movimiento.DespachadoPorCC,
                                    DespachadoFecha = (DateTime)movimiento.DespachadoFecha,
                                    DespachadoFirma = movimiento.DespachadoFirma,
                                    RecibidoPor = (long)movimiento.RecibidoPor,
                                    RecibidoFecha = (DateTime)movimiento.RecibidoFecha,
                                    RecibidoFirma = movimiento.RecibidoFirma,
                                    TransportadoNombre = movimiento.TransportadoNombre,
                                    TransportadoCC = movimiento.TransportadoCC,
                                    TransportadoCargo = movimiento.TransportadoCargo,
                                    TransportadoPlaca = movimiento.TransportadoPlaca,
                                    TransportadoTipoVehiculo = movimiento.TransportadoTipoVehiculo,
                                    TransportadoFecha = movimiento.TransportadoFecha,
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
                            entradas = entradas.Where(fil => fil.Prefijo.ToUpper().Contains(key) || fil.Conceptos.ToUpper().Contains(key) || fil.Fecha.ToString().Contains(key)).Distinct();
                        }
                    }
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : entradas.Count();
                    var count = entradas.Count();
                    return new { Items = entradas.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = entradas, Count = entradas.Count() };
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/<ValuesController1>
        [HttpGet("{id}", Name = "GetEntrada")]
        public async Task<object> GetEntrada(long id)
        {
            try
            {
                var movimiento = await _context.Movimientos.FindAsync(id);
                Entrada entrada = (Entrada)movimiento;
                return Ok(entrada);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Entrada entrada)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var formato = new Formato();
                    formato = await _context.Formatos.FirstOrDefaultAsync(f => f.Nombre == "REMISIÓN DE ALMACÉN");
                    Movimiento movimiento = (Movimiento)entrada;
                    movimiento.EstadoMovimiento = "ABIERTO";
                    movimiento.Consecutivo = formato.Consecutivo + 1;
                    formato.Consecutivo = formato.Consecutivo + 1;
                    movimiento.Prefijo = string.Format("{0}-{1:0000}", formato.Prefijo, formato.Consecutivo);
                    _context.Formatos.Update(formato);
                    var personal = await _context.Personals.FindAsync(movimiento.RecibidoPor);
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


        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CerrarEntrada(MovimientoFirma movimientoFirma)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var movimiento = await _context.Movimientos.FindAsync(movimientoFirma.IDMovimiento);
                    movimiento.FlagIn = true;
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
        public async Task<IActionResult> Put(long id, [FromBody] Entrada entrada)
        {
            try
            {
                Movimiento movimiento = (Movimiento)entrada;
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
                var movimiento = await _context.Movimientos.FindAsync(movimientoFirma.IDMovimiento);
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
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
