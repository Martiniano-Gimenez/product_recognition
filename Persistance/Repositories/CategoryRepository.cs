using Model.Domain;
using Model.Repositories;
using Persistance.Context;

namespace Persistance.Repositories
{
    public class CategoryRepository : EntityFrameworkRepository<Category, long>, ICategoryRepository
    {
        public CategoryRepository(ShoppingCartContext context) : base(context) { }
    }
}
