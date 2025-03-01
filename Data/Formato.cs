using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GilgalInventar.Data
{
    public class Formato
    {
        [Key]
        public long IDFormato { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public int Revision { get; set; }
        public DateTime Fecha { get; set; }
        public bool ControlPaginas { get; set; }
        public string Prefijo { get; set; }
        public long Consecutivo { get; set; }
    }
}
