using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GilgalInventar.Data
{
    public partial class CentroCosto
    {
        [Key]
        [Editable(false)]
        public long IDCentroCosto { get; set; }
        [StringLength(10)]
        [Required(ErrorMessage = "Código es requerido.")]
        [DisplayName("Código")]
        public string Codigo { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = "Nombre es requerido.")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
    }
}
