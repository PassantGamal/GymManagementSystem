using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        public ActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }
        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErorrMessage"] = "Id of member can not be negative number or 0 ";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErorrMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        public ActionResult MemberHealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErorrMessage"] = "Id of MemberHealthRecord can not be negative number or 0 ";
                return RedirectToAction(nameof(Index));
            }
            var MemberHealthRecord = _memberService.GetMemberHealthRecordDetails(id);
            if (MemberHealthRecord is null)
            {
                TempData["ErorrMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(MemberHealthRecord);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel createdMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Chack Data And Missing Fields");
                return View(nameof(Create), createdMember);
            }
            bool Result = _memberService.CreateMember(createdMember);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErorrMessage"] = "Meember Failed To Create , Check Phone And Email";
            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult MemberEdit(int Id)
        {
            if (Id <= 0)
            {
                TempData["ErorrMessage"] = "Id of MemberHealthRecord can not be negative number or 0 ";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberService.GetMemberToUpdate(Id);
            if (Member is null)
            {
                TempData["ErorrMessage"] = "Member Not Found ";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }
        [HttpPost]
        public ActionResult MemberEdit([FromRoute]int Id,MemberToUpdateViewModel memberToEdit)
        {
            if(!ModelState.IsValid)
            return View(memberToEdit);

            var Result = _memberService.UpdateMemberDetails(Id, memberToEdit);
            if(Result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";
            }
            else
            {
                TempData["ErorrMessage"] = "Meember Failed To Update";
            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErorrMessage"]= "Id of Member can not be negative number or 0 ";
                return RedirectToAction(nameof(Index));
            }
            var member=_memberService.GetMemberDetails(id);
            if(member is null)
            {
                TempData["ErorrMessage"] = "Member Not Found ";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            ViewBag.MemberName=member.Name;
            return View();
        }
        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            var Result = _memberService.RemoveMember(id);
            if(Result)
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            else
                TempData["ErorrMessage"] = "Meember Failed To Delete";
            return RedirectToAction(nameof(Index));
        }
    }
}
