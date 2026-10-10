using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToRoute("Trainers",new { action="GetTrainers" });
        }
        public ActionResult GetMember()
        {
            return View();
        }
        public ActionResult CreateMember()
        {
            return View();
        }

    }
}
