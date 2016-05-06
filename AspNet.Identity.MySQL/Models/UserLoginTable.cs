using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AspNet.Identity.MySQL
{
    public class UserLoginTable
    {
        [Column(Order = 0),Key]
        public string LoginProvider { get; set; }
        [Column(Order = 1), Key]
        public string ProviderKey { get; set; }
        [Column(Order = 2), Key]
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}
