using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GilgalInventar.Data
{
    public class Empresa
    {
        [Key]
        public long IDEmpresa { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = "Nombre es requerido.")]
        [DisplayName("Nombre")]
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        [StringLength(10)]
        [Required(ErrorMessage = "Nit es requerido.")]
        [DisplayName("Nit")]
        public string Nit { get; set; }
        [Required(ErrorMessage = "DV es requerido.")]
        [DisplayName("DV")]
        public int DigitoVerificacion { get; set; }
        [StringLength(250)]
        [DisplayName("Tipo de sociedad")]
        public string TipoSociedad { get; set; }
        [DisplayName("Departamento")]
        public string IDDepartamento { get; set; }
        [DisplayName("Municipio")]
        public string IDMunicipio { get; set; }
        [DisplayName("Municipio")]
        public string Pais { get; set; }
        [Required(ErrorMessage = "Dirección es requerido.")]
        [DisplayName("Dirección")]
        public string Direccion { get; set; }
        [DataType(DataType.EmailAddress)]
        [DisplayName("E-mail")]
        public string Email { get; set; }
        [DataType(DataType.Url)]
        [DisplayName("Sitio web")]
        public string SitioWeb { get; set; }
        [StringLength(250)]
        [DisplayName("Teléfono fijo")]
        public string TelefonoFijo { get; set; }
        [StringLength(250)]
        [DisplayName("Teléfono móvil")]
        public string TelefonoMovil { get; set; }
        [StringLength(250)]
        [DisplayName("Contacto")]
        public string Contacto { get; set; }
        [StringLength(100)]
        [DisplayName("Teléfono Contacto")]
        public string TelefonoContacto { get; set; }
        [StringLength(50)]
        [DisplayName("Código de Habilitación")]
        public string CodigoPostal { get; set; }
        public string Logo { get; set; }
        public int AlertaStock { get; set; }
        public string PrnEncabezado { get; set; }
        public string PrnBarCode { get; set; }
        public string PrnInicioCode1 { get; set; }
        public string PrnQRCode { get; set; }
        public string PrnSeparador1 { get; set; }
        public string PrnSeparador2 { get; set; }
        public string PrnInicioCode2 { get; set; }
        public string PrnNombreCode1L1 { get; set; }
        public string PrnNombreCode1L2 { get; set; }
        public string PrnNombreCode2L1 { get; set; }
        public string PrnNombreCode2L2 { get; set; }
        public string PrnPiePagina { get; set; }
        public string PrnComando { get; set; }
        public string Impresora { get; set; }
    }
}
