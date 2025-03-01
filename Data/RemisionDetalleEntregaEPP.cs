using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GilgalInventar.Data
{
    public class RemisionDetalleEntregaEPP
    {
        public RemisionDetalleEntregaEPP()
        {
            this.Activo = true;
            this.FechaEntregaEPP = DateTime.Now;
        }
        [Key]
        [Editable(false)]
        public long IDRemisionDetalleEntregaEPP { get; set; }
        public long IDMovimiento { get; set; }
        [Required(ErrorMessage = "Fecha es requerido.")]
        public DateTime FechaEntregaEPP { get; set; }
        [Required(ErrorMessage = "Personal es requerido.")]
        public long IDPersonal { get; set; }
        [Required(ErrorMessage = "Elemento es requerido.")]
        public string BarCode { get; set; }
        [Required(ErrorMessage = "Cantidad es requerido.")]
        public int EntregaCantidad { get; set; }
        [Required(ErrorMessage = "Centro de costo es requerido.")]
        public long IDCentroCosto { get; set; }
        public string EntregaRecibidoFirma { get; set; }
        public bool Activo { get; set; }
    }
}
