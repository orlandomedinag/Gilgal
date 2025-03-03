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
    public class OrigenDestinosController : ControllerBase
    {
        private readonly gilgalContext _context;

        public OrigenDestinosController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                //var origenDestinos = await _context.OrigenDestinos.ToListAsync();
                var queryString = Request.Query;
                string filter = queryString["$filter"];
                string auto = queryString["$inlineCount"];
                var origenDestinos = _context.OrigenDestinos.AsQueryable();
                if (queryString.Keys.Contains("$inlinecount"))
                {
                    StringValues Skip;
                    StringValues Take;
                    if (!string.IsNullOrEmpty(filter))
                    {
                        string key;
                        if (filter.Contains("substring")) //searching 
                        {
                            key = filter.Split(new string[] { "'" }, StringSplitOptions.None)[1].ToUpper();
                            origenDestinos = _context.OrigenDestinos.Where(fil => fil.Nombre.ToUpper().Contains(key)).Distinct().AsQueryable();
                        }
                    }
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : origenDestinos.Count();
                    var count = origenDestinos.Count();
                    return new { Items = origenDestinos.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = origenDestinos, Count = origenDestinos.Count() };
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/<ValuesController1>
        [HttpGet("{flag}", Name = "GetAllOrigenDestinos")]
        public async Task<IActionResult> GetAllOrigenDestinos(int flag)
        {
            try
            {
                var origenDestinos = await _context.OrigenDestinos.ToListAsync();
                return Ok(origenDestinos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrigenDestino origenDestino)
        {
            try
            {
                _context.OrigenDestinos.Add(origenDestino);
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
        public async Task<IActionResult> Put(long id, [FromBody] OrigenDestino origenDestino)
        {
            try
            {
                _context.Entry(origenDestino).State = EntityState.Modified;
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
                var origenDestino = await _context.OrigenDestinos.FindAsync(id);
                if (origenDestino != null)
                {
                    _context.Remove(origenDestino);
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
