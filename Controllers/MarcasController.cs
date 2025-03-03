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
    public class MarcasController : ControllerBase
    {
        private readonly gilgalContext _context;

        public MarcasController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                //var marcas = await _context.Marcas.ToListAsync();
                var queryString = Request.Query;
                string filter = queryString["$filter"];
                string auto = queryString["$inlineCount"];
                var marcas = _context.Marcas.OrderBy(m => m.Nombre).AsQueryable();
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
                            marcas = _context.Marcas.Where(fil => fil.Nombre.ToUpper().Contains(key)).Distinct().AsQueryable();
                        }
                    }
                    int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : marcas.Count();
                    var count = marcas.Count();
                    return new { Items = marcas.Skip(skip).Take(top), Count = count };
                }
                else
                {
                    return new { Items = marcas, Count = marcas.Count() };
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/<ValuesController1>
        [HttpGet("{flag}", Name = "GetAllMarcas")]
        public async Task<IActionResult> GetAllMarcas(int flag)
        {
            try
            {
                var marcas = await _context.Marcas.OrderBy(m => m.Nombre).ToListAsync();
                return Ok(marcas);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Marca marca)
        {
            try
            {
                _context.Marcas.Add(marca);
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
        public async Task<IActionResult> Put(long id, [FromBody] Marca marca)
        {
            try
            {
                _context.Entry(marca).State = EntityState.Modified;
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
                var marca = await _context.Marcas.FindAsync(id);
                if (marca != null)
                {
                    _context.Remove(marca);
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
