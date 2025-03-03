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
    public class PersonalsController : ControllerBase
    {
        private readonly gilgalContext _context;

        public PersonalsController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                //var personals = await _context.Personals.ToListAsync();
                var personals = _context.Personals.AsQueryable();
                var queryString = Request.Query;
                string auto = queryString["$inlineCount"];
                if (queryString.Keys.Contains("$inlinecount"))
                {
                    StringValues Skip;
                    StringValues Take;
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : personals.Count();
                    var count = personals.Count();
                    return new { Items = personals.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = personals, Count = personals.Count() };
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/<ValuesController1>
        [HttpGet("{flag}", Name = "GetAllPersonals")]
        public async Task<IActionResult> GetAllPersonals(int flag)
        {
            try
            {
                var personals = await _context.Personals.ToListAsync();
                return Ok(personals);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("GetPersonal/{id}")]
        public async Task<IActionResult> GetElemento(long id)
        {
            var personal = new Personal();
            try
            {
                personal = await _context.Personals.FindAsync(id);
                if (personal != null)
                {
                    return Ok(personal);
                }
                else
                    return Ok(new Personal());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Personal personal)
        {
            try
            {
                _context.Personals.Add(personal);
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
        public async Task<IActionResult> Put(long id, [FromBody] Personal personal)
        {
            try
            {
                _context.Entry(personal).State = EntityState.Modified;
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
                var personal = await _context.Personals.FindAsync(id);
                if (personal != null)
                {
                    _context.Remove(personal);
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
