using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GilgalInventar.Data
{
    public partial class Marca
    {
        [Key]
        [Editable(false)]
        public long IDMarca { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = "Nombre es requerido.")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
    }
}
