using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GilgalInventar.Data
{
    public partial class TipoEquipoElemento
    {
        [Key]
        [Editable(false)]
        public long IDTipoEquElem { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = "Nombre es requerido.")]
        [DisplayName("Nombre")]
        public string NombreTipo { get; set; }
        [Required(ErrorMessage = "Es Codigo único es requerido.")]
        [DisplayName("Es Codigo único")]
        public bool CodigoUnico { get; set; }
    }
}
