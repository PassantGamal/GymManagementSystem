using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels
{
    public class TrainerDetailsViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; } = null!; 
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Specialties Specialty { get; set; }
        public string? Address { get; set; }
        public DateTime HireDate { get; set; }
    }
}
