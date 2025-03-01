using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GilgalInventar.Data
{
    public partial class EstadoEquElem
    {
        [Key]
        public long IdestadoEquElem { get; set; }
        public string Nombre { get; set; }
    }
}
