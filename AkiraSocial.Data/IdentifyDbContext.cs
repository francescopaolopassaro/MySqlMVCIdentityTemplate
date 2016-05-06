using AspNet.Identity.MySQL;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Data
{
    public class IdentityDbContext : MySQLDatabase
    {
        public IdentityDbContext(string connectionName)
            : base(connectionName)
        {
        }

        public static IdentityDbContext Create()
        {
            return new IdentityDbContext("DefaultConnection");
        }

        public IdentityDbContext()
        {
        }
    }
}
