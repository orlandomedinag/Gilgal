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
    public class RequisisionesController : ControllerBase
    {
        private readonly gilgalContext _context;

        public RequisisionesController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var requisisiones = _context.Requisisiones.Where(r => r.Activo).AsQueryable();
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
                            requisisiones = requisisiones.Where(fil => fil.Prefijo.ToUpper().Contains(key) || fil.TipoRequisicion.ToUpper().Contains(key) || fil.FechaSolicitud.ToString().Contains(key)).Distinct();
                        }
                    }
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : requisisiones.Count();
                    var count = requisisiones.Count();
                    return new { Items = requisisiones.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = requisisiones, Count = requisisiones.Count() };
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
                var movimientos = await _context.Requisisiones.Where(r => r.Activo).OrderByDescending(r => r.IDRequisision).ToListAsync();
                foreach (var movimi in movimientos)
                {
                        if (!personalmov.Contains((long)movimi.SolicitadoPor))
                            movimi.Activo = false;
                }
                var requisisiones = movimientos.Where(r => r.Activo).AsQueryable();
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
                            requisisiones = requisisiones.Where(fil => fil.Prefijo.ToUpper().Contains(key) || fil.TipoRequisicion.ToUpper().Contains(key) || fil.FechaSolicitud.ToString().Contains(key)).Distinct();
                        }
                    }
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : requisisiones.Count();
                    var count = requisisiones.Count();
                    return new { Items = requisisiones.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = requisisiones, Count = requisisiones.Count() };
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetRequisisionesAbierto/{idarea}")]
        public async Task<IActionResult> GetRequisisionesAbierto(long idarea)
        {
            try
            {
                var haymovis = await _context.Requisisiones.AnyAsync(m => m.Activo && m.IDArea == idarea && !m.FlagIn);
                return Ok(haymovis);
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
                var movimientos = await _context.Requisisiones.Where(r => r.Activo).ToListAsync();
                foreach (var movimi in movimientos)
                {
                    if (!personalmov.Contains((long)movimi.SolicitadoPor))
                        movimi.Activo = false;
                }
                var requisisiones = movimientos.Where(r => r.Activo).AsQueryable();
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
                            requisisiones = requisisiones.Where(fil => fil.Prefijo.ToUpper().Contains(key) || fil.TipoRequisicion.ToUpper().Contains(key) || fil.FechaSolicitud.ToString().Contains(key)).Distinct();
                        }
                    }
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : requisisiones.Count();
                    var count = requisisiones.Count();
                    return new { Items = requisisiones.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = requisisiones, Count = requisisiones.Count() };
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/<ValuesController1>
        [HttpGet("{id}", Name = "GetRequisisione")]
        public async Task<object> GetRequisisione(long id)
        {
            try
            {
                var requisisione = await _context.Requisisiones.FindAsync(id);
                return Ok(requisisione);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Requisisione requisisione)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var formato = new Formato();
                    formato = await _context.Formatos.FirstOrDefaultAsync(f => f.Nombre == "REQUISICIÓN DE MATERIALES, EQUIPOS Y SERVICIOS");
                    requisisione.Consecutivo = formato.Consecutivo + 1;
                    formato.Consecutivo = formato.Consecutivo + 1;
                    requisisione.Prefijo = string.Format("{0}-{1:0000}", formato.Prefijo, formato.Consecutivo);
                    _context.Formatos.Update(formato);
                    _context.Requisisiones.Add(requisisione);
                    await _context.SaveChangesAsync();
                    return Ok(requisisione);
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
        public async Task<IActionResult> Create([FromBody] Requisisione requisisione)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var formato = new Formato();
                    formato = await _context.Formatos.FirstOrDefaultAsync(f => f.Nombre == "REQUISICIÓN DE MATERIALES, EQUIPOS Y SERVICIOS");
                    requisisione.Consecutivo = formato.Consecutivo + 1;
                    formato.Consecutivo = formato.Consecutivo + 1;
                    requisisione.Prefijo = string.Format("{0}-{1:0000}", formato.Prefijo, formato.Consecutivo);
                    _context.Formatos.Update(formato);
                    _context.Requisisiones.Add(requisisione);
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
        public async Task<IActionResult> CerrarRequisisione(MovimientoFirma movimientoFirma)
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
        public async Task<IActionResult> Put(long id, [FromBody] Requisisione requisisione)
        {
            try
            {
                _context.Entry(requisisione).State = EntityState.Modified;
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
                var requisisione = await _context.Requisisiones.FindAsync(id);
                if (requisisione != null)
                {
                    //_context.Remove(movimiento);
                    requisisione.Activo = false;
                    _context.Entry(requisisione).State = EntityState.Modified;
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
