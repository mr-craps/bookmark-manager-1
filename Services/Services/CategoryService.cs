using Data;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private ReadLaterDataContext _ReadLaterDataContext;
        public CategoryService(ReadLaterDataContext readLaterDataContext) 
        {
            _ReadLaterDataContext = readLaterDataContext;            
        }

        public Category CreateCategory(Category category)
        {
            Category existingCategory =
                    _ReadLaterDataContext.Categories.Where(c =>
                                                        c.Name.ToLower() == category.Name.ToLower() &&
                                                        c.UserId.ToLower() == category.UserId.ToLower()                                                        
                                                     ).FirstOrDefault();

            if (existingCategory != null)
            {
                return existingCategory;
            }
            else
            {
                _ReadLaterDataContext.Add(category);
                _ReadLaterDataContext.SaveChanges();
                return category;
            }            
        }

        public void UpdateCategory(Category category)
        {
            _ReadLaterDataContext.Update(category);
            _ReadLaterDataContext.SaveChanges();
        }

        public List<Category> GetCategories(string userId)
        {
            return _ReadLaterDataContext.Categories.Where(b => b.UserId == userId).ToList();
        }

        public Category GetCategory(int Id)
        {
            return _ReadLaterDataContext.Categories.Where(c => c.ID == Id).FirstOrDefault();
        }

        public Category GetCategory(string Name, string UserId)
        {
            return _ReadLaterDataContext.Categories.Where(c => 
                                                        c.Name.ToLower() == Name.ToLower() &&
                                                        c.UserId.ToLower() == UserId.ToLower()
                                                    ).FirstOrDefault();
        }

        public void DeleteCategory(Category category)
        {
            _ReadLaterDataContext.Categories.Remove(category);
            _ReadLaterDataContext.SaveChanges();
        }

        public void DeleteCategory(int Id)
        {
            Category categoryToRemove = _ReadLaterDataContext.Categories.Where(c => c.ID == Id).FirstOrDefault();
            _ReadLaterDataContext.Remove(categoryToRemove);
            _ReadLaterDataContext.SaveChanges();
        }
    }
}
