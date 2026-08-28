using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var Plans=_unitOfWork.GetRepository<Plan>().GetAll();
            if (Plans is null || !Plans.Any()) return [];
            return Plans.Select(p => new PlanViewModel()
            {
                Id= p.Id,
                Name=p.Name,
                Description=p.Description,
                DurationDays=p.DurationDays,
                IsActive=p.IsActive,
                Price=p.Price,
            }

            );

        }

        public PlanViewModel? GetPlanById(int PlanId)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan is null) return null;
            return new PlanViewModel()
            {
                Id = Plan.Id,
                Name = Plan.Name,
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                IsActive = Plan.IsActive,
                Price = Plan.Price,
            };
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int PlanId)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan is null || Plan.IsActive == false || HasActiveMemberShip(PlanId)) return null;
            return new UpdatePlanViewModel()
            {
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                planName = Plan.Name,
                Price = Plan.Price
            };
        }
        public bool UpdatePlan(int PlanId, PlanViewModel updatedPlan)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan is null || HasActiveMemberShip(PlanId)) return false;
            (Plan.Description, Plan.Price, Plan.DurationDays, Plan.UpdatedAt) =
                (updatedPlan.Description,updatedPlan.Price,updatedPlan.DurationDays,DateTime.Now);
            _unitOfWork.GetRepository<Plan>().Update(Plan);
            return _unitOfWork.SaveChanges() > 0;
        }
        public bool ToggleStatus(int PlanId)
        {
            var repo = _unitOfWork.GetRepository<Plan>();
            var Plan = repo.GetById(PlanId);
            if (Plan is null || HasActiveMemberShip(PlanId)) return false;
            Plan.IsActive=Plan.IsActive==true ? false : true;
            Plan.UpdatedAt=DateTime.Now;
            try 
            {
               repo.Update(Plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch 
            {
                return false;

            }
        }

        #region Helper
        private bool HasActiveMemberShip(int PlanId)
        {
            var ActiveMemberShip = _unitOfWork.GetRepository<Membership>()
                .GetAll(x => x.PlanId == PlanId && x.Status == "Active");
            return ActiveMemberShip.Any();
        }
        #endregion

    }
}
