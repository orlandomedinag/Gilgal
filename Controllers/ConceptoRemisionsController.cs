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
    public class ConceptoRemisionsController : ControllerBase
    {
        private readonly gilgalContext _context;

        public ConceptoRemisionsController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                //var conceptoRemisions = await _context.ConceptoRemisions.ToListAsync();
                var conceptoRemisions = _context.ConceptoRemisions.AsQueryable();
                var queryString = Request.Query;
                string auto = queryString["$inlineCount"];
                if (queryString.Keys.Contains("$inlinecount"))
                {
                    StringValues Skip;
                    StringValues Take;
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : conceptoRemisions.Count();
                    var count = conceptoRemisions.Count();
                    return new { Items = conceptoRemisions.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = conceptoRemisions, Count = conceptoRemisions.Count() };
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        // GET: api/<ValuesController1>
        [HttpGet("{flag}", Name = "GetAllConceptos")]
        public async Task<IActionResult> GetAllConceptos(int flag)
        {
            try
            {
                var conceptos = await _context.ConceptoRemisions.ToListAsync();
                return Ok(conceptos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ConceptoRemision conceptoRemision)
        {
            try
            {
                _context.ConceptoRemisions.Add(conceptoRemision);
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
        public async Task<IActionResult> Put(long id, [FromBody] ConceptoRemision conceptoRemision)
        {
            try
            {
                _context.Entry(conceptoRemision).State = EntityState.Modified;
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
                var conceptoRemision = await _context.ConceptoRemisions.FindAsync(id);
                if (conceptoRemision != null)
                {
                    _context.Remove(conceptoRemision);
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
