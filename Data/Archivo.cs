using System.ComponentModel.DataAnnotations;
using System;
using System.IO;

namespace GilgalInventar.Data
{
    public class Archivo
    {
        // CONSTRUCTOR
        public Archivo()
        {
            this.Id = Guid.NewGuid();
            this.Creado = DateTime.Now;
        }

        // PROPIEDADES PÚBLICAS
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public DateTime Creado { get; set; }
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        [StringLength(4)]
        [Required]
        public string Extension { get; set; }
        [Required]
        public int? Descargas { get; set; }
        public string PathViewer { get; set; }
        public string ExtensionViewer { get; set; }
        public string PathVista { get; set; }
        public byte[] Contenido { get; set; }
        public string Mime { get; set; }
        public string ContenidoBase64 { get; set; }

        // PROPIEDADES PRIVADAS
        public string PathRelativo
        {
            get
            {
                return @"doccargados\" +
                                            this.Id.ToString() + "." +
                                            this.Extension;
            }
        }

        public string PathCompleto
        {
            get
            {
                var _PathAplicacion = Directory.GetCurrentDirectory();
                return Path.Combine(_PathAplicacion, "wwwroot", this.PathRelativo);
            }
        }

        // MÉTODOS PÚBLICOS

        public void SubirArchivo(byte[] archivo)
        {
            File.WriteAllBytes(this.PathCompleto, archivo);
        }

        public byte[] DescargarArchivo()
        {
            //return File.ReadAllBytes(this.PathCompleto);
            return this.Contenido;
        }

        public void EliminarArchivo()
        {
            File.Delete(this.PathCompleto);
        }

        public void DescargarArchivoContenido()
        {
            byte[] imageBytes = this.Contenido;
            File.WriteAllBytes(this.PathViewer, imageBytes);
        }

        public void DescargarArchivoViewer()
        {
            byte[] imageBytes = Convert.FromBase64String(this.ContenidoBase64);
            File.WriteAllBytes(this.PathViewer, imageBytes);
        }

        public string RetornarNombreArchivo()
        {
            return string.Format("{0}.{1}", this.Nombre, this.Extension);
        }

        public string RetornarPathViewer()
        {
            return this.PathViewer;
        }
        public string RetornarPathVista()
        {
            return this.PathVista;
        }
        public string RetornarExtensionViewer()
        {
            return this.ExtensionViewer;
        }

        public string RetornarPathDescargas()
        {
            return @"/doccargados/" +
                                                        this.Id.ToString() + "." +
                                                        this.Extension;
        }
    }
}
