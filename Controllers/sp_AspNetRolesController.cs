
using GilgalInventar.Data;
using Microsoft.AspNetCore.Identity;
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
    public class sp_AspNetRolesController : ControllerBase
    {
        private readonly gilgalContext _context;

        public sp_AspNetRolesController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var sql = @"EXEC sp_AspNetRoles";
                var sp_roles = await _context.sp_AspNetRoles.FromSqlRaw(sql).ToListAsync();
                //return Ok(_context.sp_AspNetRoles.ToList());
                return new { Items = sp_roles, Count = sp_roles.Count() };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllsp_AspNetRoles()
        {
            try
            {
                var sql = @"EXEC sp_AspNetRoles";
                var sp_roles = await _context.sp_AspNetRoles.FromSqlRaw(sql).ToListAsync();
                return Ok(sp_roles);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // POST api/<ValuesController1>
        [HttpPost]
        public IActionResult Post([FromBody] sp_AspNetRoles sp_AspNetRoles)
        {
            try
            {
                _context.sp_AspNetRoles.Add(sp_AspNetRoles);
                _context.SaveChanges();
                return CreatedAtRoute("Getsp_AspNetRoles", new { id = sp_AspNetRoles.RoleId }, sp_AspNetRoles);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT api/<ValuesController1>/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] sp_AspNetRoles sp_AspNetRoles)
        {
            try
            {
                if (sp_AspNetRoles.RoleId == id)
                {
                    _context.Entry(sp_AspNetRoles).State = EntityState.Modified;
                    _context.SaveChanges();
                    return CreatedAtRoute("Getsp_AspNetRoles", new { id = sp_AspNetRoles.RoleId }, sp_AspNetRoles);
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
        public IActionResult Delete(string id)
        {
            try
            {
                var sp_AspNetRoles = _context.sp_AspNetRoles.Find(id);
                if (sp_AspNetRoles != null)
                {
                    _context.Remove(sp_AspNetRoles);
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
