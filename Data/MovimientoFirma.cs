using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GilgalInventar.Data
{
    public class MovimientoFirma
    {
        [Key]
        public long IDMovimiento { get; set; }
        public string DespachadoFirma { get; set; }
        public string RecibidoFirma { get; set; }
        public string UploadFile { get; set; }
    }
}
