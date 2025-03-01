using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

#nullable disable

namespace GilgalInventar.Data
{
    public partial class GrupoResponsable
    {
        public GrupoResponsable()
        {
            Usuarios = new HashSet<Usuario>();
        }

        [Key]
        public int IdgrupoResponsable { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Activo { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
