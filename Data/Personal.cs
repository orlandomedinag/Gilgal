using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GilgalInventar.Data
{
    public partial class Personal
    {
        public Personal()
        {
            this.ResponsableInventario = null;
            this.Activo = true;
        }

        [Key]
        public long IDPersonal { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NombreCompleto
        {
            get
            {
                return string.Format("{0} {1}", this.Nombres, this.Apellidos);
            }
        }
        public string TipoIdentificacion { get; set; }
        public string NoIdentificacion { get; set; }
        public string Email { get; set; }
        public string LugarExpedicionCC { get; set; }
        public string Cargo { get; set; }
        public string TelefonoMovil { get; set; }
        public string TipoPersonal { get; set; }
        public string Empresa { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string Usuario_Creacion { get; set; }
        public bool Activo { get; set; }
        public string ResponsableInventario { get; set; }
        public long IDAreaFuncional { get; set; }
    }
}
