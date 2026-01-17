using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IPlanRepository
    {
        //GetAll
        IEnumerable<Plan> GetAll();
        //Add
        int Add(Plan plan);
        //update
        int Update(Plan plan);
        //Delete
        int Delete(Plan plan);
        //GetById
        Plan? GetById(int id);

    }
}
