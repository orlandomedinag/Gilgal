using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace GilgalInventar.Data
{
    public class RequisisionDetalle
    {
        [Key]
        [Editable(false)]
        public long IDRequisisionDetalle { get; set; }
        public long IDRequisision { get; set; }
        public int ItemNo { get; set; }
        [Required(ErrorMessage = "Descripción es requerido.")]
        public string BarCode { get; set; }
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Unidad es requerido.")]
        public long IDUnidad { get; set; }
        [Required(ErrorMessage = "Cantidad Solicitada es requerido.")]
        public int CantidadSolicitada { get; set; }
        public string EspecificacionesTecnicas { get; set; }
        [Required(ErrorMessage = "Cantidad en Almacén es requerido.")]
        public int CantidadAlmacen { get; set; }
        public int CantidadAutorizada { get; set; }
        public bool Activo { get; set; }
    }
}
