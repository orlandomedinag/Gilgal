using System.ComponentModel.DataAnnotations;


#nullable disable

namespace GilgalInventar.Data
{
    public partial class Cargo
    {
        [Key]
        public long Idcargo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Origen { get; set; }
        public long? IdcargoJefe { get; set; }
        public string Objetivos { get; set; }
        public string Funciones { get; set; }
        public string Responsabilidad { get; set; }
        public string Autoridad { get; set; }
        public int? Activo { get; set; }
        public bool? EsSupervisor { get; set; }
        public bool? EsSupervisado { get; set; }
        public bool? ElaboraDocumentos { get; set; }
        public bool? RevisaApruebaDocumentos { get; set; }
    }
}
