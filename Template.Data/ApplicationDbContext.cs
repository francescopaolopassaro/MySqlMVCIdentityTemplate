using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Repository.Pattern.Ef6;
using MySql.Data.Entity;
using Template.Data.Mapping;
using Template.Data.Models;

namespace AkiraSocial
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public partial class ApplicationDbContext : DataContext
    {
        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public ApplicationDbContext()
            : base("Name=DefaultConnection")
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new UserProfileMap());
        }
    }
}
