using GilgalInventar.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GilgalInventar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequisisionDetallesController : ControllerBase
    {
        private readonly gilgalContext _context;

        public RequisisionDetallesController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        [Route("GetAllRequisisionDetalles")]
        public async Task<object> GetAllRequisisionDetalles()
        {
            try
            {
                var requisisionDetalles = await _context.RequisisionDetalles.Where(r => r.Activo).ToListAsync();
                return Ok(requisisionDetalles);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // GET: api/<ValuesController1>
        [HttpPost]
        [Route("DetalleRequisision")]
        public async Task<object> DetalleRequisision([FromBody] List<RequisisionDetalle> requisisionDetalles)
        {
            try
            {
                foreach (var detalle in requisisionDetalles)
                    _context.Add(detalle);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<object> GetById(long id)
        {
            try
            {
                var queryString = Request.Query;
                string filter = queryString["$filter"];
                string auto = queryString["$inlineCount"];
                var requisisionDetalles = Enumerable.Empty<RequisisionDetalle>().AsQueryable();
                if (id > 0)
                    requisisionDetalles = (from detaremi in _context.RequisisionDetalles.AsQueryable()
                                          join elemento in _context.ElementoEquipos.AsQueryable()
                                          on detaremi.BarCode equals elemento.BarCode
                                          where detaremi.Activo && detaremi.IDRequisision == id
                                          select new RequisisionDetalle
                                          {
                                              IDRequisisionDetalle = detaremi.IDRequisisionDetalle,
                                              IDRequisision = detaremi.IDRequisision,
                                              ItemNo = detaremi.ItemNo,
                                              BarCode = detaremi.BarCode,
                                              Descripcion = elemento.Descripcion,
                                              IDUnidad = detaremi.IDUnidad,
                                              CantidadSolicitada = detaremi.CantidadSolicitada,
                                              EspecificacionesTecnicas = detaremi.EspecificacionesTecnicas,
                                              CantidadAlmacen = detaremi.CantidadAlmacen,
                                              CantidadAutorizada = detaremi.CantidadAutorizada,
                                              Activo = detaremi.Activo
                                          }
                                        ).OrderBy(r => r.ItemNo);
                else
                    requisisionDetalles = (from detaremi in _context.RequisisionDetalles.AsQueryable()
                                          join elemento in _context.ElementoEquipos.AsQueryable()
                                          on detaremi.BarCode equals elemento.BarCode
                                          where detaremi.Activo
                                           select new RequisisionDetalle
                                           {
                                               IDRequisisionDetalle = detaremi.IDRequisisionDetalle,
                                               IDRequisision = detaremi.IDRequisision,
                                               ItemNo = detaremi.ItemNo,
                                               BarCode = detaremi.BarCode,
                                               Descripcion = elemento.Descripcion,
                                               IDUnidad = detaremi.IDUnidad,
                                               CantidadSolicitada = detaremi.CantidadSolicitada,
                                               EspecificacionesTecnicas = detaremi.EspecificacionesTecnicas,
                                               CantidadAlmacen = detaremi.CantidadAlmacen,
                                               CantidadAutorizada = detaremi.CantidadAutorizada,
                                               Activo = detaremi.Activo
                                           }
                                        ).OrderBy(r => r.ItemNo);
                //return new { Items = movimientoDetalles, Count = movimientoDetalles.Count() };
                if (queryString.Keys.Contains("$inlinecount"))
                {
                    //StringValues Skip;
                    //StringValues Take;
                    if (!string.IsNullOrEmpty(filter))
                    {
                        string key;
                        if (filter.Contains("substring")) //searching 
                        {
                            key = filter.Split(new string[] { "'" }, StringSplitOptions.None)[1].ToUpper();
                            requisisionDetalles = requisisionDetalles.Where(fil => fil.BarCode.ToUpper().Contains(key.ToUpper()) || fil.Descripcion.ToUpper().Contains(key.ToUpper())).Distinct();
                        }
                    }
                    //int skip = (queryString.TryGetValue("$skip", out Skip)) ? Convert.ToInt32(Skip[0]) : 0;
                    //int top = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : movimientoDetalles.Count();
                    var count = requisisionDetalles.Count();
                    return new { Items = requisisionDetalles, Count = count };
                }
                else
                {
                    return new { Items = requisisionDetalles, Count = requisisionDetalles.Count() };
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RequisisionDetalle requisisionDetalle)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Validar si el campo del elemento es único
                    //Si es único agregar el registro y actualizar el Item
                    //Si no es único incrementar la cantidad de salida
                    _context.RequisisionDetalles.Add(requisisionDetalle);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
                //movimiento.Conceptos = string.Join(",", movimiento.lstConceptos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [HttpPut]
        public async Task<IActionResult> Put(long id, [FromBody] RequisisionDetalle requisisionDetalle)
        {
            try
            {
                _context.Entry(requisisionDetalle).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var requisisionDetalle = await _context.RequisisionDetalles.FindAsync(id);
                if (requisisionDetalle != null)
                {
                    //_context.Remove(movimiento);
                    requisisionDetalle.Activo = false;
                    _context.Entry(requisisionDetalle).State = EntityState.Modified;
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
