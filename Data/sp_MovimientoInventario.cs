using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GilgalInventar.Data
{
    public class sp_MovimientoInventario
    {
        [Key]
        public long IDMovimiento { get; set; }
        public DateTime Fecha { get; set; }
        public long IDTipoMovimiento { get; set; }
        public string TipoMovimiento { get; set; }
        public long? Consecutivo { get; set; }
        public string Prefijo { get; set; }
        public long? NoRequisicion { get; set; }
        public string NoOrdenCompra { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string Conceptos { get; set; }
        public string OtroConcepto { get; set; }
        public DateTime? EntradaFecha { get; set; }
        public string EntradaLugar { get; set; }
        public string EntradaRecibidoPor { get; set; }
        public string EntradaResponsableRecepcion { get; set; }
        public string DespachadoPor { get; set; }
        public DateTime? DespachadoFecha { get; set; }
        public long? RecibidoPor { get; set; }
        public string RecibidoPorNombre { get; set; }
        public string RecibidoPorCC { get; set; }
        public string RecibidoPorCargo { get; set; }
        public DateTime? RecibidoFecha { get; set; }
        public string Observaciones { get; set; }
        public string EntregaNombre { get; set; }
        public string EntregaCargo { get; set; }
        public string EntregaCedulaNit { get; set; }
        public DateTime? EntregaFecha { get; set; }
        public string CentroCosto { get; set; }
        public string Area { get; set; }
        public string FlagIn { get; set; }
        public string FlagOut { get; set; }
    }
}
