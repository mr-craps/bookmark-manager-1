using Entity;
using System.Collections.Generic;

namespace Services
{
    public interface ICategoryService
    {
        Category CreateCategory(Category category);
        List<Category> GetCategories(string userId);
        Category GetCategory(int Id);
        Category GetCategory(string Name, string UserId);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
        void DeleteCategory(int id);
    }
}
