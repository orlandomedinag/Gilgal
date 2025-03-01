using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GilgalInventar.Data
{
    public partial class MovimientoDetalle
    {
        public MovimientoDetalle()
        {
            this.EntradaCantidad = 0;
            this.SalidaCantidad = 0;
            this.EntradaIDEstado = "Bueno";
            this.SalidaIDEstado = "Bueno";
            this.Activo = true;
            this.IDArea = 0;
            this.IDUnidad = 1;
        }

        [Key]
        [Editable(false)]
        public long IDMovimientoDetalle { get; set; }
        [Editable(false)]
        [Required]
        public long IDMovimiento { get; set; }
        public int? ItemNo { get; set; }
        [Required(ErrorMessage = "Código es requerido.")]
        [DisplayName("Código")]
        public string BarCode { get; set; }
        [Editable(false)]
        public string Descripcion { get; set; }
        [Required]
        public int EntradaCantidad { get; set; }
        [Editable(false)]
        public string Unidad { get; set; }
        public long IDUnidad { get; set; }
        public string EntradaIDEstado { get; set; }
        public string EntradaObservaciones { get; set; }
        [Required]
        public int SalidaCantidad { get; set; }
        public string SalidaIDEstado { get; set; }
        public string SalidaObservaciones { get; set; }
        [Required]
        public bool Activo { get; set; }
        public int? EntregadoCantidad { get; set; }
        public long? IDArea { get; set; }
    }
}
