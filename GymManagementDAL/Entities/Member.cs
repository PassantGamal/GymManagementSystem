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


    }
}
