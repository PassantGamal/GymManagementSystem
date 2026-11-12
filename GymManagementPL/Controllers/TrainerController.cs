using GymManagementBLL.Services.Classes;
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
        public ActionResult TrainerEdit(int id)
        {
            if(id <= 0)
            {
                TempData["ErorrMessage"] = "Id of trainer can not be negative number or 0 ";
                return RedirectToAction(nameof(Index));
            }
            var trainer=_trainerService.GetTrainerToUpdate(id);
            if (trainer is null) {
                TempData["ErorrMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
            
        }
        [HttpPost]
        public ActionResult TrainerEdit([FromRoute] int id,TrainerToUpdateViewModel trainerToUpdate)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("InvalidData", "Chack Data And Missing Fields");
                return View(trainerToUpdate);
            }
               
            var result = _trainerService.UpdateTrainer(id, trainerToUpdate);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
            }
            else
            {
                TempData["ErorrMessage"] = "Trainer Failed To Update";
            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErorrMessage"] = "Id of trainer can not be negative number or 0 ";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainerById(id);
            if(trainer is null)
            {
                TempData["ErorrMessage"] = "Trainer Not Found";
            }
            ViewBag.TrainerId = id;
            ViewBag.TrainerName = trainer?.Name;
            return View();
        }
        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            var Result = _trainerService.RemoveTrainer(id);
            if (Result)
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            else
                TempData["ErorrMessage"] = "Trainer Failed To Delete";
            return RedirectToAction(nameof(Index));
        }

    }
}
