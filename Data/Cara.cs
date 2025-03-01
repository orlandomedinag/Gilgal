using System.ComponentModel.DataAnnotations;


#nullable disable

namespace GilgalInventar.Data
{
    public partial class Cara
    {
        [Key]
        public long IDCara { get; set; }
        public string Nombre { get; set; }
    }
}
