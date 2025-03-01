using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GilgalInventar.Data
{
    public class sp_ConsultaKardexInventario
    {
        [Key]
        public Guid ID { get; set; }
        public int? Orden { get; set; }
        [DisplayName("Tipo Elemento")]
        public string TipoELemento { get; set; }
        [DisplayName("Código")]
        public string Barcode { get; set; }
        [DisplayName("Origen")]
        public string Origen { get; set; }
        [DisplayName("Destino")]
        public string Destino { get; set; }
        [DisplayName("Descripción")]
        public string NombreElemento { get; set; }
        [DisplayName("Existencias")]
        public int? Stock { get; set; }
        public DateTime? Fecha { get; set; }
        [DisplayName("Tipo Movimiento")]
        public string TipoMovimiento { get; set; }
        [DisplayName("Prefijo")]
        public string Prefijo { get; set; }
        [DisplayName("Cantidad Entrada")]
        public int? EntradaCantidad { get; set; }
        public int? SalidaCantidad { get; set; }
        [DisplayName("Saldo")]
        public int? Saldo { get; set; }
        [DisplayName("Fecha Recibido")]
        public DateTime? FechaEntrega { get; set; }
        [DisplayName("Recibido Cant.")]
        public int? RecibidoCantidad { get; set; }
        [DisplayName("Nombre")]
        public string NombreEmpleado { get; set; }
    }
}
