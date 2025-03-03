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
    public class AreasController : ControllerBase
    {
        private readonly gilgalContext _context;

        public AreasController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                //var areas = await _context.Areas.ToListAsync();
                var areas = _context.Areas.AsQueryable();
                var queryString = Request.Query;
                string auto = queryString["$inlineCount"];
                if (queryString.Keys.Contains("$inlinecount"))
                {
                    StringValues Skip;
                    StringValues Take;
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : areas.Count();
                    var count = areas.Count();
                    return new { Items = areas.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = areas, Count = areas.Count() };
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
        public async Task<IActionResult> GetAllArea()
        {
            try
            {
                var areas = await _context.Areas.ToListAsync();
                return Ok(areas);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Area area)
        {
            try
            {
                _context.Areas.Add(area);
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
        public async Task<IActionResult> Put(long id, [FromBody] Area area)
        {
            try
            {
                _context.Entry(area).State = EntityState.Modified;
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
                var area = await _context.Areas.FindAsync(id);
                if (area != null)
                {
                    _context.Remove(area);
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
