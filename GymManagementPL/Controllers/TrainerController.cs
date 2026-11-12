using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;
        public TrainerController(ITrainerService trainerService) 
        {
            _trainerService = trainerService;
        }
        public ActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateTrainer(CreateTrainerViewModel createdTrainer)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("InvalidData", "Chack Data And Missing Fields");
                return View(nameof(Create), createdTrainer);
            }
            var Result = _trainerService.CreateTrainer(createdTrainer);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            }
            else
            {
                TempData["ErorrMessage"] = "Meember Failed To Create , Check Phone And Email";
            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErorrMessage"] = "Id of trainer can not be negative number or 0 ";
                return RedirectToAction(nameof(Index));
            }
            var trainer=_trainerService.GetTrainerById(id);
            if(trainer is null)
            {
                TempData["ErorrMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
                return View(trainer);
            
        }
    }
}
