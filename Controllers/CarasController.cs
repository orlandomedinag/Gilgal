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
    public class CarasController : ControllerBase
    {
        private readonly gilgalContext _context;

        public CarasController(gilgalContext context)
        {
            _context = context;
        }
        // GET: api/<ValuesController1>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_context.Caras.ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllCara()
        {
            try
            {
                var caras = await _context.Caras.ToListAsync();
                return Ok(caras);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // POST api/<ValuesController1>
        [HttpPost]
        public IActionResult Post([FromBody] Cara cara)
        {
            try
            {
                _context.Caras.Add(cara);
                _context.SaveChanges();
                return CreatedAtRoute("GetCara", new { id = cara.IDCara}, cara);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT api/<ValuesController1>/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] Cara cara)
        {
            try
            {
                if (cara.IDCara == id)
                {
                    _context.Entry(cara).State = EntityState.Modified;
                    _context.SaveChanges();
                    return CreatedAtRoute("GetCara", new { id = cara.IDCara }, cara);
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

        // DELETE api/<ValuesController1>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                var cara = _context.Caras.Find(id);
                if (cara != null)
                {
                    _context.Remove(cara);
                    _context.SaveChanges();
                    return Ok(id);
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
