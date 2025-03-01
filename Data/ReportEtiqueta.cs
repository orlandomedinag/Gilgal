using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GilgalInventar.Data
{
    public class ReportEtiqueta
    {
        public string tipocodigo { get; set; }
        public string codigodesde { get; set; }
        public string codigohasta { get; set; }
        [Required(ErrorMessage = "Tipo es requerido.")]
        [DisplayName("Tipo")]
        public long IDTipoEquElem { get; set; }
    }
}
