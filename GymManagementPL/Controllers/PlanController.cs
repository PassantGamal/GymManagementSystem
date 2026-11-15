using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;
        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }
        public ActionResult Index()
        {
            var plans=_planService.GetAllPlans();
            return View(plans);
        }
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of plan can not be negative number or 0 ";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanById(id);
            if(plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of plan can not be negative number or 0 ";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanToUpdate(id);
            if(plan is null)
                {
                    TempData["ErrorMessage"] = "Plan Can Not Be Updated";
                    return RedirectToAction(nameof(Index));
                }
            return View(plan);

            
        }
        [HttpPost]
        public ActionResult Edit([FromRoute]int id,UpdatePlanViewModel updatedPlan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Validation");
                return View(updatedPlan);
            }
            var result = _planService.UpdatePlan(id, updatedPlan);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan Updated Successfully";
            }
            else
            {
                TempData["ErorrMessage"] = "Plan Failed To Update";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
