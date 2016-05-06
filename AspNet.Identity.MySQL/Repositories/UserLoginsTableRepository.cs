using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AspNet.Identity.MySQL
{
    /// <summary>
    /// Class that represents the UserLogins table in the MySQL Database
    /// </summary>
    public class UserLoginsTableRepository
    {
        private MySQLDatabase _database;
        private IDbSet<UserLoginTable> dbSet;
        public IDbSet<UserLoginTable> DbSet
        {
            get { return dbSet; }
            set { dbSet = value; }
        }
        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserLoginsTableRepository(MySQLDatabase database)
        {
            _database = database;
            this.dbSet = database.Set<UserLoginTable>();
        }

        /// <summary>
        /// Deletes a login from a user in the UserLogins table
        /// </summary>
        /// <param name="user">User to have login deleted</param>
        /// <param name="login">Login to be deleted from user</param>
        /// <returns></returns>
        /*public int Delete(IdentityUser user, UserLoginInfo login)
        {
            string commandText = "Delete from UserLogins where UserId = @userId and LoginProvider = @loginProvider and ProviderKey = @providerKey";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("UserId", user.Id);
            parameters.Add("loginProvider", login.LoginProvider);
            parameters.Add("providerKey", login.ProviderKey);

            return _database.Execute(commandText, parameters);
        }*/


        public int Delete(IdentityUser user, UserLoginInfo login)
        {
            var userLogin = dbSet.Where(x => x.UserId == user.Id && x.LoginProvider == login.LoginProvider && x.ProviderKey == login.LoginProvider).FirstOrDefault();
            return Delete(userLogin);
        }

        public int Delete(UserLoginTable userLogin)
        {
            if (_database.Entry(userLogin).State == System.Data.Entity.EntityState.Detached)
            {
                dbSet.Attach(userLogin);
            }
            dbSet.Remove(userLogin);
            return this.Save();
        }

        /// <summary>
        /// Deletes all Logins from a user in the UserLogins table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        /*public int Delete(string userId)
        {
            string commandText = "Delete from UserLogins where UserId = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("UserId", userId);

            return _database.Execute(commandText, parameters);
        }*/

        public int Delete(string userId)
        {
            var userLogin = dbSet.Where(x => x.UserId == userId).FirstOrDefault();
            return Delete(userLogin);
        }

        /// <summary>
        /// Inserts a new login in the UserLogins table
        /// </summary>
        /// <param name="user">User to have new login added</param>
        /// <param name="login">Login to be added</param>
        /// <returns></returns>
        /*public int Insert(IdentityUser user, UserLoginInfo login)
        {
            string commandText = "Insert into UserLogins (LoginProvider, ProviderKey, UserId) values (@loginProvider, @providerKey, @userId)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("loginProvider", login.LoginProvider);
            parameters.Add("providerKey", login.ProviderKey);
            parameters.Add("userId", user.Id);

            return _database.Execute(commandText, parameters);
        }*/

        public int Insert(IdentityUser user, UserLoginInfo login)
        {
            var userLogin = new UserLoginTable
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                UserId = user.Id
            };
            dbSet.Add(userLogin);
            return this.Save();
        }

        /// <summary>
        /// Return a userId given a user's login
        /// </summary>
        /// <param name="userLogin">The user's login info</param>
        /// <returns></returns>
        /*public string FindUserIdByLogin(UserLoginInfo userLogin)
        {
            string commandText = "Select UserId from UserLogins where LoginProvider = @loginProvider and ProviderKey = @providerKey";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("loginProvider", userLogin.LoginProvider);
            parameters.Add("providerKey", userLogin.ProviderKey);

            return _database.GetStrValue(commandText, parameters);
        }*/
        public string FindUserIdByLogin(UserLoginInfo userLogin)
        {
            var user = dbSet.Where(x => x.LoginProvider == userLogin.LoginProvider && x.ProviderKey == userLogin.LoginProvider).FirstOrDefault();
            if (user != null)
            {
                return user.UserId;
            }

            return null;
        }


        /// <summary>
        /// Returns a list of user's logins
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        /*public List<UserLoginInfo> FindByUserId(string userId)
        {
            List<UserLoginInfo> logins = new List<UserLoginInfo>();
            string commandText = "Select * from UserLogins where UserId = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@userId", userId } };

            var rows = _database.Query(commandText, parameters);
            foreach (var row in rows)
            {
                var login = new UserLoginInfo(row["LoginProvider"], row["ProviderKey"]);
                logins.Add(login);
            }

            return logins;
        }*/

        public List<UserLoginInfo> FindByUserId(string userId)
        {
            List<UserLoginInfo> logins = new List<UserLoginInfo>();
            var rows = dbSet.Where(x => x.UserId == userId).ToList();
            foreach (var row in rows)
            {
                var login = new UserLoginInfo(row.LoginProvider, row.ProviderKey);
                logins.Add(login);
            }
            return logins;
        }

        public int Save()
        {
            return _database.SaveChanges();
        }
    }
}
