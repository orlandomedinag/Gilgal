using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace GilgalInventar.Data
{
    public partial class ElementoEquipo
    {
        public ElementoEquipo()
        {
            this.IDTipoMovimiento = 1;
            this.Activo = true;
            this.flag = true;
            this.Stock = 0;
        }

        [Key]
        [Required(ErrorMessage = "Código es requerido.")]
        [DisplayName("Código")]
        public string BarCode { get; set; }
        [Required(ErrorMessage = "Tipo es requerido.")]
        [DisplayName("Tipo")]
        public long IDTipoEquElem { get; set; }
        [DisplayName("QR")]
        public string QRCode { get; set; }
        [StringLength(40)]
        [Required(ErrorMessage = "Nombre es requerido.")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Descripción es requerido.")]
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        [StringLength(3)]
        [DisplayName("Talla")]
        public string Talla { get; set; }
        [Required(ErrorMessage = "Unidad es requerido.")]
        [DisplayName("Unidad")]
        public long IDUnidad { get; set; }
        [DisplayName("Marca")]
        public long? IDMarca { get; set; }
        [DisplayName("Serial")]
        public string Serial { get; set; }
        [DisplayName("Modelo")]
        public string Modelo { get; set; }
        [Required(ErrorMessage = "Stock Mínimo es requerido.")]
        [DisplayName("Stock Mínimo")]
        public int StockMinimo { get; set; }
        [DisplayName("Stock")]
        public int? Stock { get; set; }
        public string FichaTecnica { get; set; }
        public string ManualEquipo { get; set; }
        public string CertificadoCalibracion { get; set; }
        public string ImagenEquipo { get; set; }
        [DisplayName("Condición")]
        public long? IDCondicionEquipo { get; set; }
        [DisplayName("Estado en almacén")]
        public string EstadoAlmacen { get; set; }
        public long? IDBodega { get; set; }
        public long? IDZona { get; set; }
        public long? IDEstante { get; set; }
        public long? IDNivel { get; set; }
        public long? IDCara { get; set; }
        public long? IDTipoMovimiento { get; set; }
        public bool Activo { get; set; }
        public bool flag { get; set; }
        [DisplayName("Fecha adquisición")]
        [DataType(DataType.Date, ErrorMessage = "No tiene el formato correcto")]
        public DateTime? FechaAdquisicionE { get; set; }
        public string BarCodeImg { get; set; }
        public string QRCodeImg { get; set; }
        [DisplayName("Fecha calibración")]
        [DataType(DataType.Date, ErrorMessage = "No tiene el formato correcto")]
        public DateTime? FechaCalibracion { get; set; }
        public int? PeriodoCalibracion { get; set; }
        public string UnidadPeriodo { get; set; }//Meses, horas, días
        public string OrigenDestino { get; set; }
        public string Prefijo { get; set; }
        public DateTime? FechaUltRemision { get; set; }
    }
}
