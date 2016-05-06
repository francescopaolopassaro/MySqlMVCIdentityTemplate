using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AspNet.Identity.MySQL
{
    /// <summary>
    /// Class that represents the UserRoles table in the MySQL Database
    /// </summary>
    public class UserRolesTableRepository
    {
        private MySQLDatabase _database;
        private IDbSet<UserRolesTable> dbSet;
        public IDbSet<UserRolesTable> DbSet
        {
            get { return dbSet; }
            set { dbSet = value; }
        }

        private IDbSet<IdentityRole> dbSetRoles;
        public IDbSet<IdentityRole> DbSetRoles
        {
            get { return dbSetRoles; }
            set { dbSetRoles = value; }
        }


        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserRolesTableRepository(MySQLDatabase database)
        {
            _database = database;
            //_database.Set(typeof(UserRolesTable));
            this.dbSet = database.Set<UserRolesTable>();
            this.dbSetRoles = database.Set<IdentityRole>();
            
        }

        /// <summary>
        /// Returns a list of user's roles
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        /*public List<string> FindByUserId(string userId)
        {
            List<string> roles = new List<string>();
            string commandText = "Select Roles.Name from UserRoles, Roles where UserRoles.UserId = @userId and UserRoles.RoleId = Roles.Id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userId", userId);

            var rows = _database.Query(commandText, parameters);
            foreach(var row in rows)
            {
                roles.Add(row["Name"]);
            }

            return roles;
        }*/

        public List<string> FindByUserId(string userId)
        {

            var myRoles = from userRoles in dbSet
                          from roles in dbSetRoles
                          where userRoles.UserId == userId
                          && userRoles.RoleId == roles.Id
                          select roles.Name;
            List<string> rolesList = myRoles.ToList();
            return rolesList;
        }


        /// <summary>
        /// Deletes all roles from a user in the UserRoles table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        /*public int Delete(string userId)
        {
            string commandText = "Delete from UserRoles where UserId = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("UserId", userId);

            return _database.Execute(commandText, parameters);
        }*/

        public int Delete(string userId)
        {
            var userRoles = dbSet.Where(x => x.UserId == userId).ToList();
            int deletionCount = 0;
            foreach (var userRole in userRoles)
            {
                deletionCount += Delete(userRole);
            }
            return deletionCount;
        }

        public int Delete(UserRolesTable userRole)
        {
            if (_database.Entry(userRole).State == System.Data.Entity.EntityState.Detached)
            {
                dbSet.Attach(userRole);
            }
            dbSet.Remove(userRole);
            return this.Save();
        }

        /// <summary>
        /// Inserts a new role for a user in the UserRoles table
        /// </summary>
        /// <param name="user">The User</param>
        /// <param name="roleId">The Role's id</param>
        /// <returns></returns>
        /*public int Insert(IdentityUser user, string roleId)
        {
            string commandText = "Insert into UserRoles (UserId, RoleId) values (@userId, @roleId)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("userId", user.Id);
            parameters.Add("roleId", roleId);
            return _database.Execute(commandText, parameters);
        }*/

        public int Insert(IdentityUser user, string roleId)
        {
            var userRole = new UserRolesTable
            {
                UserId = user.Id,
                RoleId = roleId
            };
            dbSet.Add(userRole);
            return this.Save();

        }

        public int Save()
        {
            return _database.SaveChanges();
        }
    }
}
