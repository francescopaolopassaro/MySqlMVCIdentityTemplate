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


namespace Template.Data.Service
{
    /// <summary>
    ///     Add any custom business logic (methods) here
    /// </summary>
    public interface ICategoryService : IService<Category>
    {
        IEnumerable<Category> CategoriesByName(string name);
    }

     /// <summary>
    ///     All methods that are exposed from Repository in Service are overridable to add business logic,
    ///     business logic should be in the Service layer and not in repository for separation of concerns.
    /// </summary>
     public class CategoryService : Service<Category>, ICategoryService
    {
        private readonly IRepositoryAsync<Category> _repository;

        public CategoryService(IRepositoryAsync<Category> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public IEnumerable<Category> CategoriesByName(string name)
        {
            // e.g. find category by name
                return _repository.CategoriesByName(name);
        }
        public override void Insert(Category entity)
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
