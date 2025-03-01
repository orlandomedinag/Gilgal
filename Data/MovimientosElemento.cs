using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GilgalInventar.Data
{
    public class MovimientosElemento
    {
        [Key]
        public long IDMovimientoDetalle { get; set; }
        public string BarCode { get; set; }
        public string NombreElemento { get; set; }
        public string Area { get; set; }
        public string TipoMovi { get; set; }
        public string Prefijo { get; set; }
        public DateTime? FechaEntrada { get; set; }
        public int? EntradaCantidad { get; set; }
        public DateTime? FechaSalida { get; set; }
        public int? SalidaCantidad { get; set; }
        public string Recibido { get; set; }
    }
}
