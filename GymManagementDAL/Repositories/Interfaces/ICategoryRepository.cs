using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        //Add
       int Add(Category category);
        //GetAll
        IEnumerable<Category> GetAll();
        //update
        int Update(Category category);
        //Delete
        int Delete(int Id);
        //GetById
        Category? GetById(int Id);
       
    }
}
