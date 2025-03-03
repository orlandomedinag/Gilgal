using GilgalInventar.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GilgalInventar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientosController : ControllerBase
    {
        private readonly gilgalContext _context;

        public MovimientosController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var movimientos = await _context.Movimientos.Where(r => r.Activo && r.IDTipoMovimiento != 5).ToListAsync();
                return new { Items = movimientos, Count = movimientos.Count() };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("GetMovimientosAlmacenOK")]
        public async Task<object> GetMovimientosAlmacenOK()
        {
            try
            {
                var personalmov = await _context.Personals.Where(r => r.Activo && r.ResponsableInventario.Contains("Almacén")).Select(r => r.IDPersonal).ToListAsync();
                var movimientos = await _context.Movimientos.Where(r => r.Activo && (r.IDTipoMovimiento ==2 || r.IDTipoMovimiento == 4 || r.IDTipoMovimiento == 5)).ToListAsync();
                foreach (var movimi in movimientos)
                {
                    if (movimi.IDTipoMovimiento == 2 )
                    {
                        if (movimi.RecibidoPor == null)
                            movimi.Activo = false;
                        else
                        {
                            if (!personalmov.Contains((long)movimi.RecibidoPor))
                                movimi.Activo = false;
                        }
                    }
                    if (movimi.IDTipoMovimiento == 4 || movimi.IDTipoMovimiento == 5)
                    {
                        if (movimi.DespachadoPor == null)
                            movimi.Activo = false;
                        else
                        {
                            if (!personalmov.Contains((long)movimi.DespachadoPor))
                                if (movimi.EntradaRecibidoPor != null)
                                    if (!personalmov.Contains((long)movimi.EntradaRecibidoPor))
                                        movimi.Activo = false;
                        }
                    }
                }
                movimientos = movimientos.Where(r => r.Activo).ToList();
                return new { Items = movimientos, Count = movimientos.Count() };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("GetMovimientosAlmacen")]
        public async Task<object> GetMovimientosAlmacen()
        {
            try
            {
                var sp_MovimientoInventario = await  _context.sp_MovimientoInventario.FromSqlRaw("EXEC sp_MovimientoInventario @Responsable = {0}", "%Almacén%").ToListAsync();
                return Ok(sp_MovimientoInventario);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("GetMovimientosDotacionEPP")]
        public async Task<object> GetMovimientosDotacionEPP()
        {
            try
            {
                var sp_MovimientoInventario = await _context.sp_MovimientoInventario.FromSqlRaw("EXEC sp_MovimientoInventario @Responsable = {0}", "%EPP%").ToListAsync();
                return Ok(sp_MovimientoInventario);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [HttpGet]
        [Route("ConsultaKardex/{tipoele}/{fechaini}/{fechahasta}/{barcodeini}/{barcodehasta}")]
        public async Task<object> ConsultaKardex( int tipoele, string fechaini, string fechahasta, string barcodeini, string barcodehasta)
        {
            try
            {
                var consultaKardex = await _context.sp_ConsultaKardexInventario.FromSqlRaw("EXEC sp_ConsultaKardexInventario @IDTipoEquElem = {0}, @FechaDesdeC = {1}, @FechaHastaC = {2}, @BarCodeDesde = {3}, @BarCodeHasta = {4}", tipoele.ToString(), fechaini, fechahasta, barcodeini, barcodehasta).ToListAsync();
                return Ok(consultaKardex);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [HttpGet]
        [Route("GetMovimientosDotacionEPPOK")]
        public async Task<object> GetMovimientosDotacionEPPOK()
        {
            try
            {
                var personalmov = await _context.Personals.Where(r => r.Activo && r.ResponsableInventario.Contains("EPP")).Select(r => r.IDPersonal).ToListAsync();
                var movimientos = await _context.Movimientos.Where(r => r.Activo && (r.IDTipoMovimiento == 2 || r.IDTipoMovimiento == 4 || r.IDTipoMovimiento == 7)).ToListAsync();
                foreach (var movimi in movimientos)
                {
                    if (movimi.IDTipoMovimiento == 2)
                    {
                        if (movimi.RecibidoPor == null)
                            movimi.Activo = false;
                        else
                        {
                            if (!personalmov.Contains((long)movimi.RecibidoPor))
                                movimi.Activo = false;
                        }
                    }
                    if (movimi.IDTipoMovimiento == 4 || movimi.IDTipoMovimiento == 5)
                    {
                        if (movimi.DespachadoPor == null)
                            movimi.Activo = false;
                        else
                        {
                            if (!personalmov.Contains((long)movimi.DespachadoPor))
                                if (movimi.EntradaRecibidoPor != null)
                                    if (!personalmov.Contains((long)movimi.EntradaRecibidoPor))
                                        movimi.Activo = false;
                        }
                    }
                }
                return new { Items = movimientos, Count = movimientos.Count() };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // GET: api/<ValuesController1>
        [HttpGet("{id}", Name = "GetMovimiento")]
        public async Task<object> GetMovimiento(long id)
        {
            try
            {
                var movimiento = await _context.Movimientos.FindAsync(id);
                return Ok(movimiento);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/<ValuesController1>
        [HttpGet]
        [Route("GetEntradasAbierto/{idarea}")]
        public async Task<IActionResult> GetEntradasAbierto(long idarea)
        {
            try
            {
                var haymovis = await _context.Movimientos.AnyAsync(m => m.Activo && m.IDTipoMovimiento == 2 && m.IDArea == idarea && (!m.FlagIn || !m.FlagOut));
                return Ok(haymovis);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Movimiento movimiento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tipomovi = await _context.TipoMovimientos.FirstOrDefaultAsync( r => r.IDTipoMovimiento == movimiento.IDTipoMovimiento);
                    var formato = new Formato();
                    movimiento.EstadoMovimiento = "ABIERTO";
                    switch (movimiento.IDTipoMovimiento)
                    {
                        case 2:
                        case 4:
                        case 5:
                            formato = await _context.Formatos.FindAsync(tipomovi.IDFormato);
                            formato.Consecutivo = formato.Consecutivo + 1;
                            movimiento.Consecutivo = formato.Consecutivo;
                            movimiento.Prefijo = string.Format("{0}-{1:0000}", formato.Prefijo, formato.Consecutivo);
                            _context.Formatos.Update(formato);
                            break;
                        default:
                            break;
                    }
                    _context.Movimientos.Add(movimiento);
                    await _context.SaveChangesAsync();
                    return Ok();
                } else
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
        public async Task<IActionResult> PostEntrada([FromBody] Movimiento movimiento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var formato = new Formato();
                    movimiento.IDTipoMovimiento = 2;
                    formato = await _context.Formatos.FindAsync(movimiento.IDTipoMovimiento);
                    formato.Consecutivo = formato.Consecutivo + 1;
                    movimiento.Consecutivo = formato.Consecutivo;
                    movimiento.Prefijo = string.Format("{0}-{1:0000}", formato.Prefijo, formato.Consecutivo);
                    _context.Formatos.Update(formato);
                    movimiento.EstadoMovimiento = "ABIERTO";
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
                    movimiento.DespachadoFirma = (string.IsNullOrEmpty(movimientoFirma.DespachadoFirma) && movimientoFirma.DespachadoFirma.Length > 10) ? movimiento.DespachadoFirma : movimientoFirma.DespachadoFirma;
                    movimiento.ArchivoDigital = (string.IsNullOrEmpty(movimientoFirma.UploadFile)) ? movimiento.ArchivoDigital : movimientoFirma.UploadFile;
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
        public async Task<IActionResult> Put(long id, [FromBody] Movimiento movimiento)
        {
            try
            {
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
                    var sql = @"EXEC sp_ActualizarMovimientoES @idMov = '" + id + "'";
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
