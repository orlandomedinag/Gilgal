using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GilgalInventar.Data
{
    public class sp_ActualizarExistenciasElemento
    {
        [Key]
        [Editable(false)]
        public string BarCode { get; set; }
        public string Nombre { get; set; }
        public int? Stock { get; set; }
        public int? StockMinimo { get; set; }
    }
}
