using System;
using System.ComponentModel.DataAnnotations;


#nullable disable

namespace GilgalInventar.Data
{
    public partial class Usuario
    {
        [Key]
        public long Idusuario { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaUltMod { get; set; }
        public string UsuarioUltMod { get; set; }
        public long IdgrupoResponsable { get; set; }
        public int? GrupoResponsableIdgrupoResponsable { get; set; }

        public virtual GrupoResponsable GrupoResponsableIdgrupoResponsableNavigation { get; set; }
    }
}
