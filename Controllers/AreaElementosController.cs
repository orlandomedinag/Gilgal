using GilgalInventar.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https:post//go.microsoft.com/fwlink/?LinkID=397860

namespace GilgalInventar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaElementosController : ControllerBase
    {
        private readonly gilgalContext _context;

        public AreaElementosController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var areaElementos = await _context.AreaElementos.Where(r => r.Activo).ToListAsync();
                return new { Items = areaElementos, Count = areaElementos.Count() };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/<ValuesController1>

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllAreaElemento()
        {
            try
            {
                var areaElementos = await _context.AreaElementos.Where(r => r.Activo).ToListAsync();
                return Ok(areaElementos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetUbicaciones/{codigo}")]
        public async Task<object> GetUbicaciones(string codigo)
        {
            try
            {
                var areaElementos = await _context.AreaElementos.Where(r => r.Activo).Where(r => r.BarCode == codigo).ToListAsync();
                return new { Items = areaElementos, Count = areaElementos.Count() };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AreaElemento areaElemento)
        {
            try
            {
                _context.AreaElementos.Add(areaElemento);
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
        public async Task<IActionResult> Put(long id, [FromBody] AreaElemento areaElemento)
        {
            try
            {
                _context.Entry(areaElemento).State = EntityState.Modified;
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
                var areaElemento = await _context.AreaElementos.FindAsync(id);
                if (areaElemento != null)
                {
                    _context.Remove(areaElemento);
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
