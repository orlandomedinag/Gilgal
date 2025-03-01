using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GilgalInventar.Data
{
    public class Kardex
    {
        public int Orden { get; set; }
        public string TipoELemento { get; set; }
        public string BarCode { get; set; }
        public string NombreElemento { get; set; }
        public int Stock { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoMovimiento { get; set; }
        public int EntradaCantidad { get; set; }
        public int SalidaCantidad { get; set; }
        public int Saldo { get; set; }
    }
}
