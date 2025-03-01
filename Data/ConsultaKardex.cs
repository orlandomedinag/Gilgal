using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;

namespace GilgalInventar.Data
{
    public class ConsultaKardex
    {
        public ConsultaKardex()
        {
            this.FechaIni = DateTime.Now.Date;
            this.FechaFin = DateTime.Now.Date;
            this.ID = new Guid();
        }

        [Key]
        public Guid ID {get; set;}
        [DisplayName("Desde")]
        public DateTime FechaIni { get; set; }
        [DisplayName("Hasta")]
        public DateTime FechaFin { get; set; }
        [Required(ErrorMessage = "Tipo es requerido.")]
        [DisplayName("Tipo")]
        public long IDTipoEquElem { get; set; }
        [DisplayName("Desde")]
        public string BarcodeDesde { get; set; }
        [DisplayName("Hasta")]
        public string BarcodeHasta { get; set; }
        public List<sp_ConsultaKardexInventario> ListadoKardex { get; set; }
    }
}
