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
    public class UnidadesController : ControllerBase
    {
        private readonly gilgalContext _context;

        public UnidadesController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                //var unidades = await _context.Unidades.ToListAsync();
                var unidades = _context.Unidades.AsQueryable();
                var queryString = Request.Query;
                string auto = queryString["$inlineCount"];
                if (queryString.Keys.Contains("$inlinecount"))
                {
                    StringValues Skip;
                    StringValues Take;
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : unidades.Count();
                    var count = unidades.Count();
                    return new { Items = unidades.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = unidades, Count = unidades.Count() };
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/<ValuesController1>
        [HttpGet("{flag}", Name = "GetAllUnidades")]
        public async Task<IActionResult> GetAllUnidades(int flag)
        {
            try
            {
                var unidades = await _context.Unidades.ToListAsync();
                return Ok(unidades);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Unidade unidade)
        {
            try
            {
                _context.Unidades.Add(unidade);
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
        public async Task<IActionResult> Put(long id, [FromBody] Unidade unidade)
        {
            try
            {
                _context.Entry(unidade).State = EntityState.Modified;
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
                var unidade = await _context.Unidades.FindAsync(id);
                if (unidade != null)
                {
                    _context.Remove(unidade);
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
