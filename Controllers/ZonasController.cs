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
    public class ZonasController : ControllerBase
    {
        private readonly gilgalContext _context;

        public ZonasController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var zonas = await _context.Zonas.ToListAsync();
                return new { Items = zonas, Count = zonas.Count() };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllZona()
        {
            try
            {
                var zonas = await _context.Zonas.ToListAsync();
                return Ok(zonas);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Zona zona)
        {
            try
            {
                _context.Zonas.Add(zona);
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
        public async Task<IActionResult> Put(long id, [FromBody] Zona zona)
        {
            try
            {
                _context.Entry(zona).State = EntityState.Modified;
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
                var zona = await _context.Zonas.FindAsync(id);
                if (zona != null)
                {
                    _context.Remove(zona);
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
