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
    public class EntregasController : ControllerBase
    {
        private readonly gilgalContext _context;

        public EntregasController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var movimientos = await _context.Movimientos.Where(r => r.Activo && r.IDTipoMovimiento == 5).ToListAsync();
                var entregas = (from movimiento in movimientos
                                          select new Entrega
                                          {
                                              IDMovimiento = movimiento.IDMovimiento,
                                              Fecha = movimiento.Fecha,
                                              IDTipoMovimiento = movimiento.IDTipoMovimiento,
                                              Consecutivo = movimiento.Consecutivo,
                                              Prefijo = movimiento.Prefijo,
                                              NoRequisicion = movimiento.NoRequisicion,
                                              NoOrdenCompra = movimiento.NoOrdenCompra,
                                              IDOrigen = movimiento.IDOrigen,
                                              IDDestino = movimiento.IDDestino,
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
                                              RecibidoPor = movimiento.RecibidoPor,
                                              RecibidoPorNombre = movimiento.RecibidoPorNombre,
                                              RecibidoPorCargo = movimiento.RecibidoPorCargo,
                                              RecibidoPorCC = movimiento.RecibidoPorCC,
                                              RecibidoFecha = movimiento.RecibidoFecha,
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
                                              EntregaIDPersonal = (long)movimiento.EntregaIDPersonal,
                                              EntregaNombre = movimiento.EntregaNombre,
                                              EntregaCargo = movimiento.EntregaCargo,
                                              IDAreaFuncional = movimiento.IDAreaFuncional,
                                              EntregaCedulaNit = movimiento.EntregaCedulaNit,
                                              EntregaTelContacto = movimiento.EntregaTelContacto,
                                              EntregaRecibidoFirma = movimiento.EntregaRecibidoFirma,
                                              EntregaFecha = (DateTime)movimiento.EntregaFecha,
                                              IDCentroCosto = (long)movimiento.IDCentroCosto,
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
                            entregas = entregas.Where(fil => fil.Prefijo.ToUpper().Contains(key) || fil.EntregaNombre.ToUpper().Contains(key) || fil.EntregaCedulaNit.ToUpper().Contains(key) || fil.Fecha.ToString().Contains(key)).Distinct();
                        }
                    }
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : entregas.Count();
                    var count = entregas.Count();
                    return new { Items = entregas.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = entregas, Count = entregas.Count() };
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // GET: api/<ValuesController1>
        [HttpGet("{id}", Name = "GetEntrega")]
        public async Task<object> GetEntrega(long id)
        {
            try
            {
                var movimiento = await _context.Movimientos.FindAsync(id);
                Entrega entrega = (Entrega)movimiento;
                return Ok(entrega);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Entrega entrega)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var formato = new Formato();
                    formato = await _context.Formatos.FirstOrDefaultAsync(f => f.Nombre == "CONTROL ENTREGA DE MATERIAL Y SUMINISTROS");
                    Movimiento movimiento = (Movimiento)entrega;
                    movimiento.EstadoMovimiento = "ABIERTO";
                    movimiento.Consecutivo = formato.Consecutivo + 1;
                    formato.Consecutivo = formato.Consecutivo + 1;
                    movimiento.Prefijo = string.Format("{0}-{1:0000}", formato.Prefijo, formato.Consecutivo);
                    _context.Formatos.Update(formato);
                    if (movimiento.DespachadoPor != null)
                    {
                        var personal = await _context.Personals.FindAsync(movimiento.DespachadoPor);
                        movimiento.DespachadoPorNombre = personal.NombreCompleto;
                        movimiento.DespachadoPorCC = personal.NoIdentificacion;
                        movimiento.DespachadoPorCargo = personal.Cargo;
                    }
                    if (movimiento.EntregaIDPersonal != null)
                    {
                        var personal = await _context.Personals.FindAsync(movimiento.EntregaIDPersonal);
                        if (personal.TipoPersonal == "Externo")
                            movimiento.EntregaNombre = string.Format("{0}/{1}", personal.Empresa, personal.NombreCompleto);
                        else
                            movimiento.EntregaNombre = personal.NombreCompleto;
                        movimiento.EntregaCedulaNit = personal.NoIdentificacion;
                        movimiento.EntregaTelContacto = personal.TelefonoMovil;
                        movimiento.EntregaCargo = personal.Cargo;
                    }
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



        // | api/<ValuesController1>/5
        [HttpPut]
        public async Task<IActionResult> Put(long id, [FromBody] Entrega entrega)
        {
            try
            {
                Movimiento movimiento = (Movimiento)entrega;
                if (movimiento.DespachadoPor != null)
                {
                    var personal = await _context.Personals.FindAsync(movimiento.DespachadoPor);
                    movimiento.DespachadoPorNombre = personal.NombreCompleto;
                    movimiento.DespachadoPorCC = personal.NoIdentificacion;
                    movimiento.DespachadoPorCargo = personal.Cargo;
                }
                if (movimiento.EntregaIDPersonal != null)
                {
                    var personal = await _context.Personals.FindAsync(movimiento.EntregaIDPersonal);
                    movimiento.EntregaNombre = string.Empty;
                    movimiento.EntregaCedulaNit = personal.NoIdentificacion;
                    movimiento.EntregaTelContacto = personal.TelefonoMovil;
                    movimiento.EntregaCargo = personal.Cargo;
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var movimiento = await _context.Movimientos.FindAsync(id);
                if (movimiento != null)
                {
                    //_context.Remove(movimiento);
                    movimiento.Activo = false;
                    _context.Entry(movimiento).State = EntityState.Modified;
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
