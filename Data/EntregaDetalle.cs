using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GilgalInventar.Data
{
    public partial class EntregaDetalle
    {
        public EntregaDetalle()
        {
            this.EntradaCantidad = 0;
            this.SalidaCantidad = 0;
            this.Activo = true;
            this.EntradaIDEstado = "Bueno";
            this.SalidaIDEstado = "Bueno";
        }
        [Key]
        public long IDEntregaDetalle { get; set; }
        public long IDEntrega { get; set; }
        public int? ItemNo { get; set; }
        public string BarCode { get; set; }
        [Editable(false)]
        public string Descripcion { get; set; }
        public int SalidaCantidad { get; set; }
        public string Unidad { get; set; }
        public string SalidaIDEstado { get; set; }
        public string SalidaObservaciones { get; set; }
        public DateTime? SalidaFecha { get; set; }
        public int EntradaCantidad { get; set; }
        public string EntradaIDEstado { get; set; }
        public string EntradaObservaciones { get; set; }
        public DateTime? EntradaFecha { get; set; }
        public long? EntradaRecibidoPor { get; set; }
        public bool Activo { get; set; }
    }
}
