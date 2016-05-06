using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Data.Entity;

namespace AspNet.Identity.MySQL
{
    /// <summary>
    /// Class that represents the UserClaims table in the MySQL Database
    /// </summary>
    public class UserClaimsTableRepository
    {
        private MySQLDatabase _database;
        private IDbSet<UserClaims> dbSet;
        public IDbSet<UserClaims> DbSet
        {
            get { return dbSet; }
            set { dbSet = value; }
        }
        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserClaimsTableRepository(MySQLDatabase database)
        {
            _database = database;
            this.dbSet = database.Set<UserClaims>();
        }

        /// <summary>
        /// Returns a ClaimsIdentity instance given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        /*public ClaimsIdentity FindByUserId(string userId)
        {
            ClaimsIdentity claims = new ClaimsIdentity();
            string commandText = "Select * from UserClaims where UserId = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UserId", userId } };

            var rows = _database.Query(commandText, parameters);
            foreach (var row in rows)
            {
                Claim claim = new Claim(row["ClaimType"], row["ClaimValue"]);
                claims.AddClaim(claim);
            }

            return claims;
        }*/
        public ClaimsIdentity FindByUserId(string userId)
        {
            ClaimsIdentity claims = new ClaimsIdentity();
            var rows = dbSet.Where(x => x.UserId == userId);
            foreach (var row in rows)
            {
                Claim claim = new Claim(row.ClaimType, row.ClaimValue);
                claims.AddClaim(claim);
            }
            return claims;
        }

        /// <summary>
        /// Deletes all claims from a user given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>

        /* public int Delete(string userId)
         {
             string commandText = "Delete from UserClaims where UserId = @userId";
             Dictionary<string, object> parameters = new Dictionary<string, object>();
             parameters.Add("userId", userId);

             return _database.Execute(commandText, parameters);
         }*/


        public int Delete(string userId)
        {
            var userClaim = dbSet.Where(x => x.UserId == userId).FirstOrDefault();
            return Delete(userClaim);
        }

        public int Delete(UserClaims userClaim)
        {
            if (_database.Entry(userClaim).State == System.Data.Entity.EntityState.Detached)
            {
                dbSet.Attach(userClaim);
            }
            dbSet.Remove(userClaim);
            return this.Save();
        }

        /// <summary>
        /// Inserts a new claim in UserClaims table
        /// </summary>
        /// <param name="userClaim">User's claim to be added</param>
        /// <param name="userId">User's id</param>
        /// <returns></returns>
        /*public int Insert(Claim userClaim, string userId)
        {
            string commandText = "Insert into UserClaims (ClaimValue, ClaimType, UserId) values (@value, @type, @userId)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("value", userClaim.Value);
            parameters.Add("type", userClaim.Type);
            parameters.Add("userId", userId);

            return _database.Execute(commandText, parameters);
        }*/


        public int Insert(Claim userClaim, string userId)
        {
            var userClaimObj = new UserClaims
                            {
                                UserId = userId,
                                ClaimValue = userClaim.Value,
                                ClaimType = userClaim.Type
                            };
            dbSet.Add(userClaimObj);
            return this.Save();
        }

        /// <summary>
        /// Deletes a claim from a user 
        /// </summary>
        /// <param name="user">The user to have a claim deleted</param>
        /// <param name="claim">A claim to be deleted from user</param>
        /// <returns></returns>
        /* public int Delete(IdentityUser user, Claim claim)
         {
             string commandText = "Delete from UserClaims where UserId = @userId and @ClaimValue = @value and ClaimType = @type";
             Dictionary<string, object> parameters = new Dictionary<string, object>();
             parameters.Add("userId", user.Id);
             parameters.Add("value", claim.Value);
             parameters.Add("type", claim.Type);

             return _database.Execute(commandText, parameters);
         }*/

        public int Delete(IdentityUser user, Claim claim)
        {
            var userClaim = dbSet.Where(x => x.UserId == user.Id && x.ClaimValue == claim.Value && x.ClaimType == claim.Type).FirstOrDefault();
            return Delete(userClaim);
        }

        public int Save()
        {
            return _database.SaveChanges();
        }

    }
}
