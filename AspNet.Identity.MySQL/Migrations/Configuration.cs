namespace AspNet.Identity.MySQL.Migrations
{
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AspNet.Identity.MySQL.MySQLDatabase>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AspNet.Identity.MySQL.MySQLDatabase context)
        {

            var userManager = new UserManager<IdentityUser>(new UserStoreRepository<IdentityUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStoreRepository<IdentityRole>(context));
            const string name = "admin@admin.it";
            const string password = "Pass@123";
            const string roleName = "Admin";
            const string name_User = "user@user.it";
            const string password_User = "Pass@456";
            const string roleName_User = "User";

            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                roleManager.Create(role);
            }
            context.SaveChanges();


            var role_u = roleManager.FindByName(roleName_User);
            if (role_u == null)
            {
                role_u = new IdentityRole(roleName_User);
                roleManager.Create(role_u);
            }
            context.SaveChanges();
            
            
            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new IdentityUser { UserName = name, Email = name };
                userManager.Create(user, password);
                userManager.SetLockoutEnabled(user.Id, false);
            }
            context.SaveChanges();


            var user_User = userManager.FindByName(name_User);
            if (user_User == null)
            {
                user_User = new IdentityUser { UserName = name_User, Email = name_User };
                userManager.Create(user_User, password_User);
                userManager.SetLockoutEnabled(user_User.Id, false);
            }
            context.SaveChanges();

            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                userManager.AddToRole(user.Id, role.Name);
            }
            context.SaveChanges();

            var rolesForUserUser = userManager.GetRoles(user_User.Id);
            if (!rolesForUserUser.Contains(role_u.Name))
            {
                userManager.AddToRole(user_User.Id, role_u.Name);
            }
            context.SaveChanges();
      
        }
    }
}
