using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly GymDbContext _dbContext;
        public CategoryRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(Category category)
        {
            _dbContext.Categories.Add(category);
            return _dbContext.SaveChanges();
        }


        public int Delete(int Id)
        {
            var Cat = _dbContext.Categories.Find(Id);
            if(Cat==null)
                return 0;
            _dbContext.Remove(Cat);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Category> GetAll() => _dbContext.Categories.ToList();
      

        public Category? GetById(int Id) => _dbContext.Categories.Find(Id);
        

        public int Update(Category category)
        {
            _dbContext.Categories.Update(category);
            return _dbContext.SaveChanges();
        }
    }
}
