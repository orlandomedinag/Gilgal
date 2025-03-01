using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GilgalInventar.Data
{
    public class ArchivoEtiqueta
    {
        public ArchivoEtiqueta()
        {
            this.Estado = false;
        }
        [Key]
        public long ID { get; set; }
        public string Nombre { get; set; }
        public string Contenido { get; set; }
        public bool Estado { get; set; }
    }
}
