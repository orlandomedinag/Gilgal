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
    public class sp_AspNetUsersController : ControllerBase
    {
        private readonly gilgalContext _context;

        public sp_AspNetUsersController(gilgalContext context)
        {
            _context = context;
        }

        // GET: api/<ValuesController1>
        [HttpGet]
        [Route("[action]")]
        public async Task<object> Getsp()
        {
            try
            {
                var sql = @"EXEC sp_AspNetUsers";
                var sp_usuarios = await _context.sp_AspNetUsers.FromSqlRaw(sql).ToListAsync();
                //return Ok(_context.sp_AspNetUsers.ToList());
                return new { Items = sp_usuarios, Count = sp_usuarios.Count() };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var usuarios = (from user in await _context.Users.ToListAsync()
                                join userrol in await _context.UserRoles.ToListAsync()
                                on user.Id equals userrol.UserId into rolesusuarios
                                from roleusuario in rolesusuarios.DefaultIfEmpty()
                                select new sp_AspNetUsers
                                {
                                    Id = user.Id,
                                    UserName = user.UserName,
                                    Email = user.Email,
                                    PhoneNumber = user.PhoneNumber,
                                    RoleId = (roleusuario != null) ? roleusuario.RoleId : string.Empty
                                }).ToList();
                //return Ok(_context.sp_AspNetUsers.ToList());
                return new { Items = usuarios, Count = usuarios.Count() };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllsp_AspNetUsers()
        {
            try
            {
                var sp_AspNetUsers = await _context.sp_AspNetUsers.ToListAsync();
                return Ok(sp_AspNetUsers);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAspNetUserRoles()
        {
            try
            {
                var listadoRoles = await (from user in _context.Users
                                     join userRoles in _context.UserRoles on user.Id equals userRoles.UserId
                                     join role in _context.Roles on userRoles.RoleId equals role.Id
                                     select new UsuariosRoles { UserId = user.Id, UserName = user.UserName, RoleId = role.Id, RoleName = role.NormalizedName })
                                        .ToListAsync();
                return Ok(listadoRoles);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // POST api/<ValuesController1>
        [HttpPost]
        public IActionResult Post([FromBody] sp_AspNetUsers sp_AspNetUsers)
        {
            try
            {
                _context.sp_AspNetUsers.Add(sp_AspNetUsers);
                _context.SaveChanges();
                return CreatedAtRoute("Getsp_AspNetUsers", new { id = sp_AspNetUsers.Id }, sp_AspNetUsers);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT api/<ValuesController1>/5
        [HttpPut]
        public IActionResult Put([FromBody] sp_AspNetUsers sp_AspNetUsers)
        {
            try
            {
                if (sp_AspNetUsers.Id != null)
                {
                    var sql = string.Format("EXEC sp_AspNetUserRoles @IdUser = '{0}', @IdRole = '{1}'", sp_AspNetUsers.Id, sp_AspNetUsers.RoleId);
                    var sp_AspNetUserRoles = _context.sp_AspNetUserRoles.FromSqlRaw(sql).ToListAsync();
                    _context.SaveChanges();
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

        // DELETE api/<ValuesController1>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var sp_AspNetUsers = _context.sp_AspNetUsers.Find(id);
                if (sp_AspNetUsers != null)
                {
                    _context.Remove(sp_AspNetUsers);
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
