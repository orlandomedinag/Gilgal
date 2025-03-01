using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GilgalInventar.Data
{
    public partial class CondicionEquipo
    {
        [Key]
        [Editable(false)]
        public long IDCondicionEquipo { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = "Nombre es requerido.")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
    }
}
