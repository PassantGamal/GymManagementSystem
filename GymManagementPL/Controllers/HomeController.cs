using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            //var Result = new ViewResult();
            //return Result;
            //OR
            return View();
        }
        public JsonResult Trainer()
        {
            var Trainers = new List<Trainer>()
            {
                new Trainer(){Name="Mohammed" ,Phone="01265478592"}
                ,
                new Trainer(){Name="Amr" ,Phone="01523654785"}
            };
            return Json(Trainers);
        }
        public RedirectResult Redirect()
        {
            return Redirect("https://google.com");
        }
        public ContentResult Content()
        {
            //return Content("Hello From Gym Management System");
            return Content("<h1>Hello From Gym Management System</h1>","text/html");

        }
        public FileResult DownloadFile()
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "css", "site.css");
            var FileBytes=System.IO.File.ReadAllBytes(FilePath);
            return File(FileBytes, "text/css", "DownloadableSites.css");
        }
        public EmptyResult EmptyAction()
        {
            return new EmptyResult();
        }
    }
}
