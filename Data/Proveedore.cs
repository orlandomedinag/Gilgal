using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GilgalInventar.Data
{
    public partial class Proveedore
    {
        [Key]
        public long Idproveedor { get; set; }
        public string Nombre { get; set; }
        public string Sigla { get; set; }
        public string Nit { get; set; }
        public int? DigitoVerificacion { get; set; }
        public string TipoSociedad { get; set; }
        public long? Iddepartamento { get; set; }
        public long? Idmunicipio { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string SitioWeb { get; set; }
        public string TelefonoFijo { get; set; }
        public string TelefonoMovil { get; set; }
        public string Contacto { get; set; }
        public string TelefonoContacto { get; set; }
        public string Servicios { get; set; }
    }
}
