using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GilgalInventar.Data
{
    public class sp_AspNetUserRoles
    {
        [Key]
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}
