using MySql.Data.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading;

namespace AspNet.Identity.MySQL
{
    /// <summary>
    /// Class that encapsulates a MySQL database connections 
    /// and CRUD operations
    /// </summary>
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class MySQLDatabase : DbContext, IDisposable
    {
        private MySqlConnection _connection;
        public DbSet<IdentityRole> Roles { get; set; }
        public IDbSet<IdentityUser> User { get; set; }
        public IDbSet<UserClaims> UserClaims { get; set; }
        public IDbSet<UserLoginTable> UserLogins { get; set; }

        public IDbSet<UserRolesTable> UserRoles { get; set; }
        /// Default constructor which uses the "DefaultConnection" connectionString
        /// </summary>
        public MySQLDatabase()
            : base("DefaultConnection")
        {
           /* this.Set(typeof(IdentityRole));
            this.Set(typeof(IdentityUser));
            this.Set(typeof(UserClaims));
            this.Set(typeof(UserLoginTable));
            this.Set(typeof(UserRolesTable));
            //Roles = new DbSet<IdentityRole>();*/
            //UserClaims = new DbSet<UserClaims>();
        }

        /// <summary>
        /// Constructor which takes the connection string name
        /// </summary>
        /// <param name="connectionStringName"></param>
        public MySQLDatabase(string connectionStringName) :base(connectionStringName)
        {
           // string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            //_connection = new MySqlConnection(connectionString);
        }
       /* public MySQLDatabase MySqlDbContext
        {
            get { return _MySqlDbContext;}
            set
            {
                value = _MySqlDbContext;
            }
        }*/
       // private static MySQLDatabase _MySqlDbContext;
        static MySQLDatabase()
        {

            //_MySqlDbContext = new MySQLDatabase();
            // static constructors are guaranteed to only fire once per application.
            // I do this here instead of App_Start so I can avoid including EF
            // in my MVC project (I use UnitOfWork/Repository pattern instead)
            DbConfiguration.SetConfiguration(new MySql.Data.Entity.MySqlEFConfiguration());
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {

            
            modelBuilder.Entity<UserClaims>().ToTable("UserClaims").HasRequired(u => u.User).WithMany(x => x.Claims).HasForeignKey(t => t.UserId).WillCascadeOnDelete(false);
            modelBuilder.Entity<UserLoginTable>().ToTable("UserLogins").HasRequired(u => u.User).WithMany(x => x.Login).HasForeignKey(t => t.UserId).WillCascadeOnDelete(false);

            /*modelBuilder.Entity<IdentityUser>().HasMany(t => t.Roles).WithMany(c => c.Users)
                                .Map(t => t.ToTable("UserRoles")
                                    .MapLeftKey("UserId")
                                    .MapRightKey("RoleId"));*/


            modelBuilder.Entity<UserRolesTable>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUser>().ToTable("users");
            base.OnModelCreating(modelBuilder);


            //modelBuilder.Entity<ORCHA.Domain.Model.User>().Ignore(p => p.UserName);
            //modelBuilder.Entity<Domain.Model.Product>().HasRequired(e => e.Category)
            //.WithMany(e => e.Products).HasForeignKey(c=>c.CategoryId);
        }

        /// <summary>
        /// Executes a non-query MySQL statement
        /// </summary>
        /// <param name="commandText">The MySQL query to execute</param>
        /// <param name="parameters">Optional parameters to pass to the query</param>
        /// <returns>The count of records affected by the MySQL statement</returns>
        public int Execute(string commandText, Dictionary<string, object> parameters)
        {
            int result = 0;

            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                EnsureConnectionOpen();
                var command = CreateCommand(commandText, parameters);
                result = command.ExecuteNonQuery();
            }
            finally
            {
                _connection.Close();
            }

            return result;
        }

        /// <summary>
        /// Executes a MySQL query that returns a single scalar value as the result.
        /// </summary>
        /// <param name="commandText">The MySQL query to execute</param>
        /// <param name="parameters">Optional parameters to pass to the query</param>
        /// <returns></returns>
        public object QueryValue(string commandText, Dictionary<string, object> parameters)
        {
            object result = null;

            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                EnsureConnectionOpen();
                var command = CreateCommand(commandText, parameters);
                result = command.ExecuteScalar();
            }
            finally
            {
                EnsureConnectionClosed();
            }

            return result;
        }

        /// <summary>
        /// Executes a SQL query that returns a list of rows as the result.
        /// </summary>
        /// <param name="commandText">The MySQL query to execute</param>
        /// <param name="parameters">Parameters to pass to the MySQL query</param>
        /// <returns>A list of a Dictionary of Key, values pairs representing the 
        /// ColumnName and corresponding value</returns>
        public List<Dictionary<string, string>> Query(string commandText, Dictionary<string, object> parameters)
        {
            List<Dictionary<string, string>> rows = null;
            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                EnsureConnectionOpen();
                var command = CreateCommand(commandText, parameters);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    rows = new List<Dictionary<string, string>>();
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, string>();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            var columnValue = reader.IsDBNull(i) ? null : reader.GetString(i);
                            row.Add(columnName, columnValue);
                        }
                        rows.Add(row);
                    }
                }
            }
            finally
            {
                EnsureConnectionClosed();
            }

            return rows;
        }

        /// <summary>
        /// Opens a connection if not open
        /// </summary>
        private void EnsureConnectionOpen()
        {
            var retries = 3;
            if (_connection.State == ConnectionState.Open)
            {
                return;
            }
            else
            {
                while (retries >= 0 && _connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                    retries--;
                    Thread.Sleep(30);
                }
            }
        }

        /// <summary>
        /// Closes a connection if open
        /// </summary>
        public void EnsureConnectionClosed()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        /// <summary>
        /// Creates a MySQLCommand with the given parameters
        /// </summary>
        /// <param name="commandText">The MySQL query to execute</param>
        /// <param name="parameters">Parameters to pass to the MySQL query</param>
        /// <returns></returns>
        private MySqlCommand CreateCommand(string commandText, Dictionary<string, object> parameters)
        {
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = commandText;
            AddParameters(command, parameters);

            return command;
        }

        /// <summary>
        /// Adds the parameters to a MySQL command
        /// </summary>
        /// <param name="commandText">The MySQL query to execute</param>
        /// <param name="parameters">Parameters to pass to the MySQL query</param>
        private static void AddParameters(MySqlCommand command, Dictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                return;
            }

            foreach (var param in parameters)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = param.Key;
                parameter.Value = param.Value ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }
        }

        /// <summary>
        /// Helper method to return query a string value 
        /// </summary>
        /// <param name="commandText">The MySQL query to execute</param>
        /// <param name="parameters">Parameters to pass to the MySQL query</param>
        /// <returns>The string value resulting from the query</returns>
        public string GetStrValue(string commandText, Dictionary<string, object> parameters)
        {
            string value = QueryValue(commandText, parameters) as string;
            return value;
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }



        
    }
}
