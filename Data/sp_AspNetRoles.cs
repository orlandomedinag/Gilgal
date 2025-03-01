using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GilgalInventar.Data
{
    public class sp_AspNetRoles
    {
        [Key]
        [Editable(false)]
        public string RoleId { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}
