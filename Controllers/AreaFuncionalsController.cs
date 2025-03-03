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
    public class AreaFuncionalsController : ControllerBase
    {
        private readonly gilgalContext _context;

        public AreaFuncionalsController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                //var areaFuncionalFuncionals = await _context.AreaFuncionals.ToListAsync();
                var areaFuncionals = _context.AreaFuncionals.AsQueryable();
                var queryString = Request.Query;
                string auto = queryString["$inlineCount"];
                if (queryString.Keys.Contains("$inlinecount"))
                {
                    StringValues Skip;
                    StringValues Take;
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : areaFuncionals.Count();
                    var count = areaFuncionals.Count();
                    return new { Items = areaFuncionals.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = areaFuncionals, Count = areaFuncionals.Count() };
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/<ValuesController1>

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllAreaFuncional()
        {
            try
            {
                var areaFuncionals = await _context.AreaFuncionals.ToListAsync();
                return Ok(areaFuncionals);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AreaFuncional areaFuncional)
        {
            try
            {
                _context.AreaFuncionals.Add(areaFuncional);
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
        public async Task<IActionResult> Put(long id, [FromBody] AreaFuncional areaFuncional)
        {
            try
            {
                _context.Entry(areaFuncional).State = EntityState.Modified;
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
                var areaFuncional = await _context.AreaFuncionals.FindAsync(id);
                if (areaFuncional != null)
                {
                    _context.Remove(areaFuncional);
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
