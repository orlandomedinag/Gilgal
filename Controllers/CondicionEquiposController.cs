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
    public class CondicionEquiposController : ControllerBase
    {
        private readonly gilgalContext _context;

        public CondicionEquiposController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var condicionEquipos = await _context.CondicionEquipos.ToListAsync();
                return new { Items = condicionEquipos, Count = condicionEquipos.Count() };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/<ValuesController1>
        [HttpGet("{flag}", Name = "GetAllCondicionEquipos")]
        public async Task<IActionResult> GetAllCondicionEquipos(int flag)
        {
            try
            {
                var condicionEquipos = await _context.CondicionEquipos.ToListAsync();
                return Ok(condicionEquipos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CondicionEquipo condicionEquipo)
        {
            try
            {
                _context.CondicionEquipos.Add(condicionEquipo);
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
        public async Task<IActionResult> Put(long id, [FromBody] CondicionEquipo condicionEquipo)
        {
            try
            {
                _context.Entry(condicionEquipo).State = EntityState.Modified;
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
                var condicionEquipo = await _context.CondicionEquipos.FindAsync(id);
                if (condicionEquipo != null)
                {
                    _context.Remove(condicionEquipo);
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
