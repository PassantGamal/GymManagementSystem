using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Trainer: GymUser
    {
        public Specialties Specialty { get; set; }
        //HireDate == CreatedAt from BasaEntity 

        #region Relationships
        #region Trainer - Sessions 1-M
        public ICollection<Session> Sessions { get; set; } = null!;
        #endregion

        #endregion
    }
}
