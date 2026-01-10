using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    internal class Trainer: GymUser
    {
        public Specialties Specialty { get; set; }
        //HireDate == CreatedAt from BasaEntity 

    }
}
