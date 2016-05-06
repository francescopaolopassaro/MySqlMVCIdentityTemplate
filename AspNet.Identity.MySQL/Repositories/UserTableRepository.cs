using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AspNet.Identity.MySQL
{
    /// <summary>
    /// Class that represents the Users table in the MySQL Database
    /// </summary>
    public class UserTableRepository<TUser>
        where TUser : IdentityUser
    {
        private MySQLDatabase _database;
        private IDbSet<IdentityUser> dbSet;
        public IDbSet<IdentityUser> DbSet
        {
            get { return dbSet; }
            set { dbSet = value; }
        }

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserTableRepository(MySQLDatabase database)
        {
            _database = database;
            //_database.Set(typeof(IdentityUser));
            this.dbSet = database.Set<IdentityUser>();
        }

        /// <summary>
        /// Returns the user's name given a user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public string GetUserName(string userId)
        //{
        //    string commandText = "Select Name from Users where Id = @id";
        //    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };

        //    return _database.GetStrValue(commandText, parameters);
        //}

        public string GetUserName(string userId)
        {

            var user = dbSet.Where(o => o.Id == userId).FirstOrDefault();

            if (user != null)
            {
                return user.UserName;
            }

            return null;
        }

        /// <summary>
        /// Returns a User ID given a user name
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        /*   public string GetUserId(string userName)
           {
               string commandText = "Select Id from Users where UserName = @name";
               Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", userName } };

               return _database.GetStrValue(commandText, parameters);
           }*/

        public string GetUserId(string userName)
        {

            var user = dbSet.Where(x => x.UserName == userName).FirstOrDefault();

            if (user != null)
            {
                return user.Id;
            }

            return null;
        }
        /// <summary>
        /// Returns an TUser given the user's id
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        //public TUser GetUserById(string userId)
        //{
        //    TUser user = null;
        //    string commandText = "Select * from Users where Id = @id";
        //    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };

        //    var rows = _database.Query(commandText, parameters);
        //    if (rows != null && rows.Count == 1)
        //    {
        //        var row = rows[0];
        //        user = (TUser)Activator.CreateInstance(typeof(TUser));
        //        user.Id = row["Id"];
        //        user.UserName = row["UserName"];
        //        user.PasswordHash = string.IsNullOrEmpty(row["PasswordHash"]) ? null : row["PasswordHash"];
        //        user.SecurityStamp = string.IsNullOrEmpty(row["SecurityStamp"]) ? null : row["SecurityStamp"];
        //        user.Email = string.IsNullOrEmpty(row["Email"]) ? null : row["Email"];
        //        user.EmailConfirmed = row["EmailConfirmed"] == "1" ? true:false;
        //        user.PhoneNumber = string.IsNullOrEmpty(row["PhoneNumber"]) ? null : row["PhoneNumber"];
        //        user.PhoneNumberConfirmed = row["PhoneNumberConfirmed"] == "1" ? true : false;
        //        user.LockoutEnabled = row["LockoutEnabled"] == "1" ? true : false;
        //        user.LockoutEndDateUtc = string.IsNullOrEmpty(row["LockoutEndDateUtc"]) ? DateTime.Now : DateTime.Parse(row["LockoutEndDateUtc"]);
        //        user.AccessFailedCount = string.IsNullOrEmpty(row["AccessFailedCount"]) ? 0 : int.Parse(row["AccessFailedCount"]);
        //    }

        //    return user;
        //}

        public TUser GetUserById(string userId)
        {
            return (TUser)dbSet.Where(o => o.Id == userId).FirstOrDefault();
        }
        /// <summary>
        /// Returns a list of TUser instances given a user name
        /// </summary>
        /// <param name="userName">User's name</param>
        /// <returns></returns>
        //public List<TUser> GetUserByName(string userName)
        //{
        //    List<TUser> users = new List<TUser>();
        //    string commandText = "Select * from Users where UserName = @name";
        //    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", userName } };

        //    var rows = _database.Query(commandText, parameters);
        //    foreach(var row in rows)
        //    {
        //        TUser user = (TUser)Activator.CreateInstance(typeof(TUser));
        //        user.Id = row["Id"];
        //        user.UserName = row["UserName"];
        //        user.PasswordHash = string.IsNullOrEmpty(row["PasswordHash"]) ? null : row["PasswordHash"];
        //        user.SecurityStamp = string.IsNullOrEmpty(row["SecurityStamp"]) ? null : row["SecurityStamp"];
        //        user.Email = string.IsNullOrEmpty(row["Email"]) ? null : row["Email"];
        //        user.EmailConfirmed = row["EmailConfirmed"] == "1" ? true : false;
        //        user.PhoneNumber = string.IsNullOrEmpty(row["PhoneNumber"]) ? null : row["PhoneNumber"];
        //        user.PhoneNumberConfirmed = row["PhoneNumberConfirmed"] == "1" ? true : false;
        //        user.LockoutEnabled = row["LockoutEnabled"] == "1" ? true : false;
        //        user.TwoFactorEnabled = row["TwoFactorEnabled"] == "1" ? true : false;
        //        user.LockoutEndDateUtc = string.IsNullOrEmpty(row["LockoutEndDateUtc"]) ? DateTime.Now : DateTime.Parse(row["LockoutEndDateUtc"]);
        //        user.AccessFailedCount = string.IsNullOrEmpty(row["AccessFailedCount"]) ? 0 : int.Parse(row["AccessFailedCount"]);
        //        users.Add(user);
        //    }

        //    return users;
        //}

        public List<TUser> GetUserByName(string userName)
        {

            List<TUser> userslst = new List<TUser>();

            var usersList = dbSet.Where(x => x.UserName == userName);

            foreach (var item in usersList)
            {
                userslst.Add((TUser)item);
            }

            //var users = new List<TUser>(usersList.Cast<TUser>());
            return userslst;


        }
        public List<TUser> GetUserByEmail(string email)
        {
            return null;
        }

        /// <summary>
        /// Return the user's password hash
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public string GetPasswordHash(string userId)
        {
            string passHash = dbSet.Where(x => x.Id == userId).FirstOrDefault().PasswordHash;
            if (string.IsNullOrEmpty(passHash))
            {
                return null;
            }
            return passHash;
        }

        /// <summary>
        /// Sets the user's password hash
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        /*public int SetPasswordHash(string userId, string passwordHash)
        {
            string commandText = "Update Users set PasswordHash = @pwdHash where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@pwdHash", passwordHash);
            parameters.Add("@id", userId);

            return _database.Execute(commandText, parameters);
        }*/

        public int SetPasswordHash(string userId, string passwordHash)
        {
            var userObj = dbSet.Where(x => x.Id == userId).FirstOrDefault();
            userObj.PasswordHash = passwordHash;
            return Update(userObj);
        }

        /// <summary>
        /// Returns the user's security stamp
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /*public string GetSecurityStamp(string userId)
        {
            string commandText = "Select SecurityStamp from Users where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };
            var result = _database.GetStrValue(commandText, parameters);

            return result;
        }*/

        public string GetSecurityStamp(string userId)
        {
            var user = dbSet.Where(x => x.Id == userId).FirstOrDefault();

            if (user != null)
            {
                return user.SecurityStamp;
            }
            return null;
        }

        /// <summary>
        /// Inserts a new user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /*public int Insert(TUser user)
        {
            string commandText = @"Insert into Users (UserName, Id, PasswordHash, SecurityStamp,Email,EmailConfirmed,PhoneNumber,PhoneNumberConfirmed, AccessFailedCount,LockoutEnabled,LockoutEndDateUtc,TwoFactorEnabled)
                values (@name, @id, @pwdHash, @SecStamp,@email,@emailconfirmed,@phonenumber,@phonenumberconfirmed,@accesscount,@lockoutenabled,@lockoutenddate,@twofactorenabled)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@name", user.UserName);
            parameters.Add("@id", user.Id);
            parameters.Add("@pwdHash", user.PasswordHash);
            parameters.Add("@SecStamp", user.SecurityStamp);
            parameters.Add("@email", user.Email);
            parameters.Add("@emailconfirmed", user.EmailConfirmed);
            parameters.Add("@phonenumber", user.PhoneNumber);
            parameters.Add("@phonenumberconfirmed", user.PhoneNumberConfirmed);
            parameters.Add("@accesscount", user.AccessFailedCount);
            parameters.Add("@lockoutenabled", user.LockoutEnabled);
            parameters.Add("@lockoutenddate", user.LockoutEndDateUtc);
            parameters.Add("@twofactorenabled", user.TwoFactorEnabled);

            return _database.Execute(commandText, parameters);
        }*/

        public int Insert(TUser user)
        {
            
            IdentityUser identityUser = user as IdentityUser;
            dbSet.Add(identityUser);
            return this.Save();
        }


        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        /* private int Delete(string userId)
         {
             string commandText = "Delete from Users where Id = @userId";
             Dictionary<string, object> parameters = new Dictionary<string, object>();
             parameters.Add("@userId", userId);

             return _database.Execute(commandText, parameters);
         }*/
        private int Delete(string userId)
        {
            var userObj = dbSet.Find(userId);
            return Delete(userObj);
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Delete(TUser user)
        {
            return Delete(user.Id);
        }

        /// <summary>
        /// Updates a user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /*public int Update(TUser user)
        {
            string commandText = @"Update Users set UserName = @userName, PasswordHash = @pswHash, SecurityStamp = @secStamp, 
                Email=@email, EmailConfirmed=@emailconfirmed, PhoneNumber=@phonenumber, PhoneNumberConfirmed=@phonenumberconfirmed,
                AccessFailedCount=@accesscount, LockoutEnabled=@lockoutenabled, LockoutEndDateUtc=@lockoutenddate, TwoFactorEnabled=@twofactorenabled  
                WHERE Id = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userName", user.UserName);
            parameters.Add("@pswHash", user.PasswordHash);
            parameters.Add("@secStamp", user.SecurityStamp);
            parameters.Add("@userId", user.Id);
            parameters.Add("@email", user.Email);
            parameters.Add("@emailconfirmed", user.EmailConfirmed);
            parameters.Add("@phonenumber", user.PhoneNumber);
            parameters.Add("@phonenumberconfirmed", user.PhoneNumberConfirmed);
            parameters.Add("@accesscount", user.AccessFailedCount);
            parameters.Add("@lockoutenabled", user.LockoutEnabled);
            parameters.Add("@lockoutenddate", user.LockoutEndDateUtc);
            parameters.Add("@twofactorenabled", user.TwoFactorEnabled);

            return _database.Execute(commandText, parameters);
        }*/

        public int Update(TUser user)
        {
            var userObj = dbSet.Find(user.Id);
            if (userObj == null)
                return 0;
            userObj = user;
            return Update(userObj);
        }

        public int Update(IdentityUser user)
        {
            dbSet.Attach(user);
            _database.Entry(user).State = System.Data.Entity.EntityState.Modified;
            return this.Save();
        }

        public int Delete(IdentityUser entityToDelete)
        {
            if (_database.Entry(entityToDelete).State == System.Data.Entity.EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
            return this.Save();
        }

        public int Save()
        {
            return _database.SaveChanges();
        }
    }
}
