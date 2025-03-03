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
    public class RemisionDetalleEntregaEPPsController : ControllerBase
    {
        private readonly gilgalContext _context;

        public RemisionDetalleEntregaEPPsController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        [Route("GetAllRemiDis")]
        public async Task<object> GetAllRemiDis()
        {
            try
            {
                var remisionDetalleEntregaEPPs = await _context.RemisionDetalleEntregaEPPs.Where(r => r.Activo).ToListAsync();
                return new { Items = remisionDetalleEntregaEPPs, Count = remisionDetalleEntregaEPPs.Count() };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // GET: api/<ValuesController1>
        [HttpGet]
        [Route("Get/{id}")]
        public async Task<object> Get(long id)
        {
            try
            {
                //var remisionDetalleEntregaEPPs = Enumerable.Empty<RemisionDetalleEntregaEPP>().AsQueryable();
                var remisionDetalleEntregaEPPs = new List<RemisionDetalleEntregaEPP>();
                if (id > 0)
                    remisionDetalleEntregaEPPs = await _context.RemisionDetalleEntregaEPPs.Where(r => r.IDMovimiento == id && r.Activo).ToListAsync();
                else
                    remisionDetalleEntregaEPPs =  await _context.RemisionDetalleEntregaEPPs.Where(r => r.Activo).ToListAsync();

                return new { Items = remisionDetalleEntregaEPPs, Count = remisionDetalleEntregaEPPs.Count() };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RemisionDetalleEntregaEPP remisionDetalleEntregaEPP)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Validar si el campo del elemento es único
                    //Si es único agregar el registro y actualizar el Item
                    //Si no es único incrementar la cantidad de salida
                    var elemento = await _context.ElementoEquipos.FindAsync(remisionDetalleEntregaEPP.BarCode);
                    var elementodetalle = await _context.MovimientoDetalles.FirstOrDefaultAsync(r => r.IDMovimiento == remisionDetalleEntregaEPP.IDMovimiento && r.BarCode == remisionDetalleEntregaEPP.BarCode);
                    //Actualizar la cantidad ditribuida
                    if (elementodetalle != null)
                    {
                        elementodetalle.EntregadoCantidad = elementodetalle.EntregadoCantidad + remisionDetalleEntregaEPP.EntregaCantidad;
                        _context.Entry(elementodetalle).State = EntityState.Modified;
                    }
                    _context.RemisionDetalleEntregaEPPs.Add(remisionDetalleEntregaEPP);
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
        public async Task<IActionResult> Put(long id, [FromBody] RemisionDetalleEntregaEPP remisionDetalleEntregaEPP)
        {
            try
            {
                var elemento = await _context.ElementoEquipos.FindAsync(remisionDetalleEntregaEPP.BarCode);
                var movimiento = await _context.Movimientos.FindAsync(remisionDetalleEntregaEPP.IDMovimiento);
                int cantidadantes = 0;
                var distrilemento = await _context.MovimientoDetalles.FirstOrDefaultAsync(r => r.IDMovimiento == movimiento.IDMovimiento && r.BarCode == remisionDetalleEntregaEPP.BarCode && r.Activo);
                if (movimiento.FlagOut && remisionDetalleEntregaEPP.EntregaCantidad > 0)
                {
                    cantidadantes = GetAllRemisionDetalleEntregaEPPs().Where(r => r.IDRemisionDetalleEntregaEPP == remisionDetalleEntregaEPP.IDRemisionDetalleEntregaEPP).Select(r => r.EntregaCantidad).FirstOrDefault();
                    if (cantidadantes != remisionDetalleEntregaEPP.EntregaCantidad)
                    {
                        distrilemento.EntregadoCantidad = distrilemento.EntregadoCantidad - cantidadantes + remisionDetalleEntregaEPP.EntregaCantidad;
                        _context.Entry(distrilemento).State = EntityState.Modified;
                    }
                }
                _context.Entry(remisionDetalleEntregaEPP).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetAllRemisionDetalleEntregaEPPs")]
        public IQueryable<RemisionDetalleEntregaEPP> GetAllRemisionDetalleEntregaEPPs()
        {
            IQueryable<RemisionDetalleEntregaEPP> detalles = _context.RemisionDetalleEntregaEPPs;
            return detalles;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var remisionDetalleEntregaEPP = await _context.RemisionDetalleEntregaEPPs.FindAsync(id);
                if (remisionDetalleEntregaEPP != null)
                {
                    //_context.Remove(movimiento);
                    remisionDetalleEntregaEPP.Activo = false;
                    var movimiento = await _context.Movimientos.FindAsync(remisionDetalleEntregaEPP.IDMovimiento);
                    var distrilemento = await _context.MovimientoDetalles.FirstOrDefaultAsync(r => r.IDMovimiento == movimiento.IDMovimiento && r.BarCode == remisionDetalleEntregaEPP.BarCode && r.Activo);
                    if (distrilemento != null)
                    {
                        distrilemento.EntregadoCantidad = distrilemento.EntregadoCantidad - remisionDetalleEntregaEPP.EntregaCantidad;
                        _context.Entry(distrilemento).State = EntityState.Modified;
                    }
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
