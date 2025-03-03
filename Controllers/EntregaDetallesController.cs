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
    public class EntregaDetallesController : ControllerBase
    {
        private readonly gilgalContext _context;

        public EntregaDetallesController(gilgalContext context)
        {
            _context = context;
        }


        // GET: api/<ValuesController1>
        [HttpGet(Name = "GetAllDetalleEntregas")]
        public async Task<object> GetAllDetalleEntregas()
        {
            try
            {
                var entregaDetalles = await _context.EntregaDetalles.Where(r => r.Activo).ToListAsync();
                return new { Items = entregaDetalles, Count = entregaDetalles.Count() };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // GET: api/<ValuesController1>
        [HttpGet("{id}", Name = "GetDetalleEntrega")]
        public async Task<object> GetDetalleEntrega(long id)
        {
            try
            {
                var entregaDetalles = await _context.EntregaDetalles.Where(r => r.Activo).ToListAsync();
                entregaDetalles = (from detaremi in entregaDetalles
                                    join elemento in await _context.ElementoEquipos.ToListAsync()
                                    on detaremi.BarCode equals elemento.BarCode
                                    join unidad in await _context.Unidades.ToListAsync()
                                    on elemento.IDUnidad equals unidad.IDUnidad
                                    select new EntregaDetalle
                                    {
                                        IDEntregaDetalle = detaremi.IDEntregaDetalle,
                                        IDEntrega = detaremi.IDEntrega,
                                        ItemNo = detaremi.ItemNo,
                                        BarCode = detaremi.BarCode,
                                        Descripcion = elemento.Descripcion,
                                        EntradaCantidad = detaremi.EntradaCantidad,
                                        EntradaIDEstado = detaremi.EntradaIDEstado,
                                        Unidad = unidad.Nombre,
                                        EntradaObservaciones = detaremi.EntradaObservaciones,
                                        SalidaCantidad = detaremi.SalidaCantidad,
                                        SalidaIDEstado = detaremi.SalidaIDEstado,
                                        SalidaObservaciones = detaremi.SalidaObservaciones,
                                        Activo = detaremi.Activo
                                    }
                                    ).OrderBy(r => r.ItemNo).ToList();
                if (id > 0)
                    entregaDetalles = entregaDetalles.Where(r => r.IDEntrega == id && r.Activo).ToList();
                return new { Items = entregaDetalles, Count = entregaDetalles.Count() };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EntregaDetalle entregaDetalle)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Validar si el campo del elemento es único
                    //Si es único agregar el registro y actualizar el Item
                    //Si no es único incrementar la cantidad de salida
                    var elemento = await _context.ElementoEquipos.FindAsync(entregaDetalle.BarCode);
                    var tipoele = await _context.TipoEquipoElementos.FindAsync(elemento.IDTipoEquElem);
                    var movimiento = await _context.Movimientos.FindAsync(entregaDetalle.IDEntrega);
                    var detaremi = new EntregaDetalle();
                    //contar el numero de items de la remision
                    var items = _context.EntregaDetalles.Where(r => r.IDEntrega == entregaDetalle.IDEntrega).Count();
                    if ((movimiento.IDTipoMovimiento == 4 || movimiento.IDTipoMovimiento == 5) && entregaDetalle.SalidaCantidad > 0)
                        elemento.Stock = elemento.Stock - entregaDetalle.SalidaCantidad;
                    if ((movimiento.IDTipoMovimiento == 2 || movimiento.IDTipoMovimiento == 3 || movimiento.IDTipoMovimiento == 6) && entregaDetalle.EntradaCantidad > 0)
                        elemento.Stock = elemento.Stock + entregaDetalle.EntradaCantidad;
                    if (tipoele.CodigoUnico)
                    {
                        elemento.EstadoAlmacen = (new List<long>() { 4, 5 }.Contains(movimiento.IDTipoMovimiento)) ? "REMITIDO" : "EN BODEGA";
                        entregaDetalle.ItemNo = items + 1;
                        _context.EntregaDetalles.Add(entregaDetalle);
                    }
                    else
                    {
                        //buscar el detalle
                        detaremi = _context.EntregaDetalles.FirstOrDefault(r => r.IDEntrega == entregaDetalle.IDEntrega && r.BarCode == entregaDetalle.BarCode);
                        if (detaremi != null)
                        {
                            detaremi.SalidaCantidad = detaremi.SalidaCantidad + entregaDetalle.SalidaCantidad;
                            _context.EntregaDetalles.Update(detaremi);
                        }
                        else
                        {
                            entregaDetalle.ItemNo = items + 1;
                            _context.EntregaDetalles.Add(entregaDetalle);
                        }
                    }
                    _context.ElementoEquipos.Update(elemento);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
                //remision.Conceptos = string.Join(",", remision.lstConceptos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        // PUT api/<ValuesController1>/5
        [HttpPut]
        public async Task<IActionResult> Put(long id, [FromBody] EntregaDetalle entregaDetalle)
        {
            try
            {
                _context.Entry(entregaDetalle).State = EntityState.Modified;
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
                var entregaDetalle = await _context.EntregaDetalles.FindAsync(id);
                if (entregaDetalle != null)
                {
                    //_context.Remove(remision);
                    entregaDetalle.Activo = false;
                    _context.Entry(entregaDetalle).State = EntityState.Modified;
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
