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
    public class TipoEquipoElementosController : ControllerBase
    {
        private readonly gilgalContext _context;

        public TipoEquipoElementosController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                //var tipoEquipoElementos = await _context.TipoEquipoElementos.ToListAsync();
                var tipoEquipoElementos = _context.TipoEquipoElementos.AsQueryable();
                var queryString = Request.Query;
                string auto = queryString["$inlineCount"];
                if (queryString.Keys.Contains("$inlinecount"))
                {
                    StringValues Skip;
                    StringValues Take;
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : tipoEquipoElementos.Count();
                    var count = tipoEquipoElementos.Count();
                    return new { Items = tipoEquipoElementos.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = tipoEquipoElementos, Count = tipoEquipoElementos.Count() };
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/<ValuesController1>
        [HttpGet("{flag}", Name = "GetAllTipos")]
        public async Task<IActionResult> GetAllTipos(int flag)
        {
            try
            {
                var tipoEquipoElementos = await _context.TipoEquipoElementos.ToListAsync();
                return Ok(tipoEquipoElementos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TipoEquipoElemento tipoEquipoElemento)
        {
            try
            {
                _context.TipoEquipoElementos.Add(tipoEquipoElemento);
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
        public async Task<IActionResult> Put(long id, [FromBody] TipoEquipoElemento tipoEquipoElemento)
        {
            try
            {
                _context.Entry(tipoEquipoElemento).State = EntityState.Modified;
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
                var tipoEquipoElemento = await _context.TipoEquipoElementos.FindAsync(id);
                if (tipoEquipoElemento != null)
                {
                    _context.Remove(tipoEquipoElemento);
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
