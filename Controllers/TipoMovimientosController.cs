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
    public class TipoMovimientosController : ControllerBase
    {
        private readonly gilgalContext _context;

        public TipoMovimientosController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var tipoMovimientos = await _context.TipoMovimientos.ToListAsync();
                return new { Items = tipoMovimientos, Count = tipoMovimientos.Count() };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllTipoMovimiento()
        {
            try
            {
                var tipoMovimientos = await _context.TipoMovimientos.ToListAsync();
                return Ok(tipoMovimientos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TipoMovimiento tipoMovimiento)
        {
            try
            {
                _context.TipoMovimientos.Add(tipoMovimiento);
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
        public async Task<IActionResult> Put(long id, [FromBody] TipoMovimiento tipoMovimiento)
        {
            try
            {
                _context.Entry(tipoMovimiento).State = EntityState.Modified;
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
                var tipoMovimiento = await _context.TipoMovimientos.FindAsync(id);
                if (tipoMovimiento != null)
                {
                    _context.Remove(tipoMovimiento);
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
