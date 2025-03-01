using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GilgalInventar.Data
{
    public partial class ConceptoRemision
    {
        [Key]
        [StringLength(1, ErrorMessage = "El ID no puede exceder un caracter. ")]
        public string IDConceptoRemision { get; set; }
        public string Nombre { get; set; }
    }
}
