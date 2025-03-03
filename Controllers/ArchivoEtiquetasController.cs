
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
    public class ArchivoEtiquetasController : ControllerBase
    {
        private readonly gilgalContext _context;

        public ArchivoEtiquetasController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                //var archivoEtiquetas = await _context.ArchivoEtiquetas.ToListAsync();
                var archivoEtiqueta = await _context.ArchivoEtiquetas.Where(r => r.Estado == false).OrderBy(r => r.ID).LastOrDefaultAsync();
                return Ok(archivoEtiqueta);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
  


        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ArchivoEtiqueta archivoEtiqueta)
        {
            try
            {
                _context.ArchivoEtiquetas.Add(archivoEtiqueta);
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
        public async Task<IActionResult> Put(long id, [FromBody] ArchivoEtiqueta archivoEtiqueta)
        {
            try
            {
                _context.Entry(archivoEtiqueta).State = EntityState.Modified;
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
                var archivoEtiqueta = await _context.ArchivoEtiquetas.FindAsync(id);
                if (archivoEtiqueta != null)
                {
                    _context.Remove(archivoEtiqueta);
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
