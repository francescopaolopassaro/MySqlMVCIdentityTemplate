using Template.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.Repositories;
using AspNet.Identity.MySQL;

namespace Template.Data.Repositories
{
    // Exmaple: How to add custom methods to a repository.
    public static class UserProfileRepository
    {
        public static void AddUserProfile(this IRepositoryAsync<UserProfile> repository, IdentityUser user, UserProfile data)
        {
           
            data.UserId = user.Id;
            data.ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added;
            repository.Insert(data);
        }

    }
}
