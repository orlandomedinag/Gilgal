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
    public class CentroCostosController : ControllerBase
    {
        private readonly gilgalContext _context;

        public CentroCostosController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                //var centroCostos = await _context.CentroCostos.ToListAsync();
                var centroCostos = _context.CentroCostos.AsQueryable();
                var queryString = Request.Query;
                string auto = queryString["$inlineCount"];
                if (queryString.Keys.Contains("$inlinecount"))
                {
                    StringValues Skip;
                    StringValues Take;
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : centroCostos.Count();
                    var count = centroCostos.Count();
                    return new { Items = centroCostos.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = centroCostos, Count = centroCostos.Count() };
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // GET: api/<ValuesController1>
        [Route("[action]")]
        public async Task<IActionResult> GetAllCentro(int flag)
        {
            try
            {
                var centrocostos = await _context.CentroCostos.ToListAsync();
                return Ok(centrocostos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CentroCosto centroCosto)
        {
            try
            {
                _context.CentroCostos.Add(centroCosto);
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
        public async Task<IActionResult> Put(long id, [FromBody] CentroCosto centroCosto)
        {
            try
            {
                _context.Entry(centroCosto).State = EntityState.Modified;
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
                var centroCosto = await _context.CentroCostos.FindAsync(id);
                if (centroCosto != null)
                {
                    _context.Remove(centroCosto);
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
