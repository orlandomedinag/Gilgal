using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GilgalInventar.Data
{
    public class EntregaFirma
    {
        [Key]
        public long IDEntrega { get; set; }
        public string DespachadoFirma { get; set; }
        public string UploadFile { get; set; }
    }
}
