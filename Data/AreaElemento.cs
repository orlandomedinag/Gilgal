using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GilgalInventar.Data
{
    public class AreaElemento
    {
        public AreaElemento()
        {
            this.Stock = 0;
            this.Activo = true;
        }
        [Key]
        public long IDElementoArea { get; set; }
        [Required]
        public long IDArea { get; set; }
        [Required]
        public string BarCode { get; set; }
        public int Stock { get; set; }
        public long? IDZona { get; set; }
        public long? IDEstante { get; set; }
        public long? IDNivel { get; set; }
        public long? IDCara { get; set; }
        public bool Activo { get; set; }
    }
}
