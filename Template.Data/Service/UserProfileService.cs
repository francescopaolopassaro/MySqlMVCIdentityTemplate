//Necessary to add repository extension methods
using Template.Data.Repositories;
//Necessary to add models
using Template.Data.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Necessary to references
using Repository.Pattern.Repositories;
using AspNet.Identity.MySQL;


namespace Template.Data.Service
{
    /// <summary>
    ///     Add any custom business logic (methods) here
    /// </summary>
    public interface IUserProfileService : IService<UserProfile>
    {
        void Insert(IdentityUser user, UserProfile data);
    }

     /// <summary>
    ///     All methods that are exposed from Repository in Service are overridable to add business logic,
    ///     business logic should be in the Service layer and not in repository for separation of concerns.
    /// </summary>
     public class UserProfileService  : Service<UserProfile>, IUserProfileService
    {
        private readonly IRepositoryAsync<UserProfile> _repository;

        public UserProfileService(IRepositoryAsync<UserProfile> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public void Insert(IdentityUser user, UserProfile data)
        {
            // e.g. find category by name
             _repository.AddUserProfile(user,data);
        }
        public override void Insert(UserProfile entity)
        {
            // e.g. add business logic here before inserting
            base.Insert(entity);
        }

        public override void Delete(object id)
        {
            // e.g. add business logic here before deleting
            base.Delete(id);
        }
    }
}
