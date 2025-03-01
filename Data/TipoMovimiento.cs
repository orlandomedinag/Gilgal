using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace GilgalInventar.Data
{
    public class TipoMovimiento
    {
        [Key]
        public long IDTipoMovimiento { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public long IDFormato { get; set; }
    }
}
