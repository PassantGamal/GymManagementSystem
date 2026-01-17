using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IMemberShipRepository
    {
        IEnumerable<Membership> GetAll();
        Membership? GetById(int id);
        int Update(Membership membership);
        int Delete(Membership membership);
        int Add(Membership membership);
    }
}
