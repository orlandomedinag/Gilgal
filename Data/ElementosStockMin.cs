using System.ComponentModel.DataAnnotations;


namespace GilgalInventar.Data
{
    public class ElementosStockMin
    {
        [Key]
        public string BarCode { get; set; }
        public string Tipo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Unidad { get; set; }
        public string Talla { get; set; }
        public int? StockMin { get; set; }
        public int? Stock { get; set; }
        public int? CantPedir { get; set; }
    }
}
