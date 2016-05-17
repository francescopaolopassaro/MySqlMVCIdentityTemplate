using Template.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.Repositories;

namespace Template.Data.Repositories
{
    // Exmaple: How to add custom methods to a repository.
    public static class CategoryRepository
    {
        public static IEnumerable<Category> CategoriesByName(this IRepositoryAsync<Category> repository, string name)
        {
            return repository
                .Queryable()
                .Where(x => x.CategoryName.Contains(name))
                .AsEnumerable();
        }

    }
}
