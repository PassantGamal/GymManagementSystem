using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlans();
        PlanViewModel? GetPlanById(int PlanId);
        UpdatePlanViewModel? GetPlanToUpdate(int  PlanId);
        bool UpdatePlan(int  PlanId, PlanViewModel updatedPlan);
        bool ToggleStatus(int  PlanId);
    }
}
