using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    internal class Member: GymUser
    {
        //JoinDate == CreatedAt from BasaEntity
        public string? Photo { get; set; }

        #region Relationships
        #region  Member - HealthRecord (1-1) Mandatory
        public HealthRecord HealthRecord { get; set; } = null!;

        #endregion
        #region Member - Plan (M-M)
        public ICollection<Membership> Memberships { get; set; } = null!;
        #endregion
        #region Member - Session (M-M) 
        public ICollection<MemberSession> memberSessions { get; set; } = null!;
        #endregion
        #endregion

    }
}
