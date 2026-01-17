using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
   public interface IMemberSessionRepository
    {
        IEnumerable<MemberSession> GetAll();
        int Delete(MemberSession memberSession);
        int Update(MemberSession memberSession);
        int Add(MemberSession memberSession);
        MemberSession? GetById(int id);
    }
}
