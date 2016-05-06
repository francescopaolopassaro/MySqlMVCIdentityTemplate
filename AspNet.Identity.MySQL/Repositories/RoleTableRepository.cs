using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AspNet.Identity.MySQL
{
    /// <summary>
    /// Class that represents the Role table in the MySQL Database
    /// </summary>
    public class RoleTableRepository
    {
        private MySQLDatabase _database;
        private IDbSet<IdentityRole> dbSet;
        public IDbSet<IdentityRole> DbSet
        {
            get { return dbSet; }
            set { dbSet = value; }
        }

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public RoleTableRepository(MySQLDatabase database)
        {
            _database = database;
            //_database.Set(typeof(IdentityRole));
            this.dbSet = database.Set<IdentityRole>();
        }

        /// <summary>
        /// Deltes a role from the Roles table
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns></returns>
        /*public int Delete(string roleId)
        {
            string commandText = "Delete from Roles where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", roleId);
            _database.Execute(commandText, parameters);
        }*/



        public int Delete(string roleId)
        {
            var role = dbSet.Find(roleId);
            return Delete(role);
        }

        public int Delete(IdentityRole role)
        {
            if (_database.Entry(role).State == System.Data.Entity.EntityState.Detached)
            {
                _database.Roles.Attach(role);
            }
            dbSet.Remove(role);
            return this.Save();
        }

        /// <summary>
        /// Inserts a new Role in the Roles table
        /// </summary>
        /// <param name="roleName">The role's name</param>
        /// <returns></returns>
        /*public int Insert(IdentityRole role)
        {
            string commandText = "Insert into Roles (Id, Name) values (@id, @name)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@name", role.Name);
            parameters.Add("@id", role.Id);

            return _database.Execute(commandText, parameters);
        }*/

        public int Insert(IdentityRole role)
        {
            dbSet.Add(role);
            return this.Save();
        }

        /// <summary>
        /// Returns a role name given the roleId
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns>Role name</returns>
        /*public string GetRoleName(string roleId)
        {
            string commandText = "Select Name from Roles where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", roleId);

            return _database.GetStrValue(commandText, parameters);
        }*/

        public string GetRoleName(string roleId)
        {
            var role = dbSet.Find(roleId);

            if (role != null)
            {
                return role.Name;
            }

            return null;
        }

        /// <summary>
        /// Returns the role Id given a role name
        /// </summary>
        /// <param name="roleName">Role's name</param>
        /// <returns>Role's Id</returns>
        /* public string GetRoleId(string roleName)
         {
             string roleId = null;
             string commandText = "Select Id from Roles where Name = @name";
             Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", roleName } };

             var result = _database.QueryValue(commandText, parameters);
             if (result != null)
             {
                 return Convert.ToString(result);
             }

             return roleId;
         }*/

        public string GetRoleId(string roleName)
        {
           // return dbSet.Where(x => x.Name == roleName).FirstOrDefault().Id;

            var result = dbSet.Where(x => x.Name == roleName).FirstOrDefault();

            if (result != null)
            {
                return result.Id;
            }

            return null;
        }

        /// <summary>
        /// Gets the IdentityRole given the role Id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        /* public IdentityRole GetRoleById(string roleId)
        {
            var roleName = GetRoleName(roleId);
            IdentityRole role = null;

            if (roleName != null)
            {
                role = new IdentityRole(roleName, roleId);
            }

            return role;

        }*/


        public IdentityRole GetRoleById(string roleId)
        {
            var roleName = GetRoleName(roleId);
            IdentityRole role = null;

            if (roleName != null)
            {
                role = new IdentityRole(roleName, roleId);
            }

            return role;

        }

        /// <summary>
        /// Gets the IdentityRole given the role name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        /*public IdentityRole GetRoleByName(string roleName)
        {
            var roleId = GetRoleId(roleName);
            IdentityRole role = null;

            if (roleId != null)
            {
                role = new IdentityRole(roleName, roleId);
            }

            return role;
        }*/

        public IdentityRole GetRoleByName(string roleName)
        {
            var roleId = GetRoleId(roleName);
            IdentityRole role = null;

            if (roleId != null)
            {
                role = new IdentityRole(roleName, roleId);
            }

            return role;
        }

        /*public int Update(IdentityRole role)
        {
            string commandText = "Update Roles set Name = @name where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", role.Id);

            return _database.Execute(commandText, parameters);
        }*/

        public int Update(IdentityRole role)
        {
            dbSet.Attach(role);
            _database.Entry(role).State = System.Data.Entity.EntityState.Modified;
            return this.Save();
        }

        public int Save()
        {
            return _database.SaveChanges();
        }
    }
}
