using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GilgalInventar.Data
{
    public class ElementoEstado
    {
        [Key]
        public string BarCode { get; set; }
        public string Nombre { get; set; }
        public string Unidad { get; set; }
        public string Marca { get; set; }
        public string Serial { get; set; }
        public string Modelo { get; set; }
        public string Destino { get; set; }
        public DateTime? FechaRemisionEntrega { get; set; }
        public string Prefijo { get; set; }
        public string Estado { get; set; }
    }
}
