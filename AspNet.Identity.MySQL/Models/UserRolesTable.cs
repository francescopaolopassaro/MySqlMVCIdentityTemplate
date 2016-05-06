using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Identity.MySQL
{
    public class UserRolesTable
    {
        [Column(Order=0),Key,ForeignKey("User")]
        public string UserId { get; set; }
        [Column(Order = 1), Key, ForeignKey("Role")]
        public string RoleId { get; set; }
        public virtual IdentityUser User { get; set; }
        public virtual IdentityRole Role { get; set; }
    }
}
