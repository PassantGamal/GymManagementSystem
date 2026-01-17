using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
  public interface IHealthRecordRepository
    {
        IEnumerable<HealthRecord> GetAll();
        HealthRecord? GetById(int id);
        int Update(HealthRecord record);
        int Delete(int id);
        int Add(HealthRecord record);
    }
}
