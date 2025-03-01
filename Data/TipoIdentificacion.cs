using System.ComponentModel.DataAnnotations;


#nullable disable

namespace GilgalInventar.Data
{
    public partial class TipoIdentificacion
    {
        [Key]
        public string IDtipoIdentificacion { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Abreviatura { get; set; }
    }
}
