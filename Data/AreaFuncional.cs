using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GilgalInventar.Data
{
    public class AreaFuncional
    {
        [Key]
        [Editable(false)]
        public long IDAreaFuncional { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = "Nombre es requerido.")]
        [DisplayName("Nombre")]
        public string NombreArea { get; set; }
    }
}
