using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GilgalInventar.Data
{
    public class sp_getEntregasEPP
    {
        [Key]
        [Editable(false)]
        public long IDMovimientoDetalle { get; set; }
        public long IDPersonal { get; set; }
        public string NoIdentificacion { get; set; }
        public string Cargo { get; set; }
        public long IDAreaFuncional { get; set; }
        public DateTime? EntregaFecha { get; set; }
        public string BarCode { get; set; }
        public string Nombre { get; set; }
        public string NombreTipo { get; set; }
        public long IDCentroCosto { get; set; }
        public string NombreCC { get; set; }
        public int EntregaCantidad { get; set; }
        public string Talla { get; set; }
        public string NombreCompleto { get; set; }

    }
}
