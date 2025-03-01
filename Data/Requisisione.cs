using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GilgalInventar.Data
{
    public class Requisisione
    {
        public Requisisione()
        {
            this.FechaSolicitud = DateTime.Now;
            this.FechaRequerida = DateTime.Now.AddDays(15);
            this.SolicitadoFecha = DateTime.Now;
            this.Activo = true;
            this.FlagIn = false;
        }
        [Key]
        [Editable(false)]
        public long IDRequisision { get; set; }
        public long? Consecutivo { get; set; }
        public string Prefijo { get; set; }
        [Required(ErrorMessage = "Centro de costo es requerido.")]
        public long IDCentroCosto { get; set; }
        [Required(ErrorMessage = "Area es requerido.")]
        public long IDArea { get; set; }
        [Required(ErrorMessage = "Fecha de Solicitud es requerido.")]
        public DateTime FechaSolicitud { get; set; }
        [Required(ErrorMessage = "Fecha Requerida es requerido.")]
        public DateTime FechaRequerida { get; set; }
        [Required(ErrorMessage = "Tipo Requisión es requerido.")]
        public string TipoRequisicion { get; set; }
        public string Justificacion { get; set; }
        public string Observaciones { get; set; }
        [Required(ErrorMessage = "Solicitado por es requerido.")]
        public long SolicitadoPor { get; set; }
        public DateTime SolicitadoFecha { get; set; }
        public string SolicitadoFirma { get; set; }
        public long? AprobadoPor { get; set; }
        public DateTime? AprobadoFecha { get; set; }
        public string AprobadoFirma { get; set; }
        public bool Activo { get; set; }
        public bool FlagIn { get; set; }
    }
}
