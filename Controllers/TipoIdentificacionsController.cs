using GilgalInventar.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GilgalInventar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoIdentificacionsController : ControllerBase
    {
        private readonly gilgalContext _context;

        public TipoIdentificacionsController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var tipoIdentificacions = await _context.TipoIdentificacions.ToListAsync();
                return new { Items = tipoIdentificacions, Count = tipoIdentificacions.Count() };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/<ValuesController1>
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllTipoIdentificacions()
        {
            try
            {
                var tipoIdentificacions = await _context.TipoIdentificacions.ToListAsync();
                return Ok(tipoIdentificacions);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TipoIdentificacion tipoIdentificacion)
        {
            try
            {
                _context.TipoIdentificacions.Add(tipoIdentificacion);
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
        public async Task<IActionResult> Put(long id, [FromBody] TipoIdentificacion tipoIdentificacion)
        {
            try
            {
                _context.Entry(tipoIdentificacion).State = EntityState.Modified;
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
                var tipoIdentificacion = await _context.TipoIdentificacions.FindAsync(id);
                if (tipoIdentificacion != null)
                {
                    _context.Remove(tipoIdentificacion);
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
