using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface ITrainerRepository
    {
        //GetAll
        IEnumerable<Trainer> GetAll();
        //GetById
        Trainer? GetById(int Id);
        //Add
        int Add(Trainer trainer);
        //Update
        int Update(Trainer trainer);
        //Delete
        int Delete(Trainer trainer);

    }
}
