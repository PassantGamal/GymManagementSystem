using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Membership:BaseEntity
    {
        //StartDate = CreatedAt from BasaEntity
        public Member Member { get; set; } = null!;
        public Plan Plan { get; set; } = null!;
        public int MemberId { get; set; }
        public int PlanId { get; set; }
        public DateTime EndDate { get; set; }

        public string Status
        {
            get
            {
                if (EndDate >= DateTime.Now)
                    return "Expired";
                else
                    return "Active";
            }
        }

    }
}
