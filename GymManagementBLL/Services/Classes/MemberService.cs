using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementPL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    internal class MemberService : IMemberService
    {
        //private readonly IGenericRepository<Member> _memberRepository;
        //private readonly IGenericRepository<Membership> _membershipRepository;
        //private readonly IGenericRepository<Plan> _planRepository;
        //private readonly IGenericRepository<HealthRecord> _healthRecordRepository;
        //private readonly IGenericRepository<MemberSession> _memberSessionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public MemberService(/*IGenericRepository<Member> memberRepository,IGenericRepository<Membership> membershipRepository,IGenericRepository<Plan>planRepository,IGenericRepository<HealthRecord> healthRecordRepository, IGenericRepository<MemberSession> memberSessionRepository*/ IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            // _memberRepository = memberRepository;
            // _membershipRepository = membershipRepository;
            //_planRepository = planRepository;
            // _healthRecordRepository = healthRecordRepository;
            // _memberSessionRepository = memberSessionRepository;
        }

        public bool CreateMember(CreateMemberViewModel createdmember)
        {
            try
            {
                if (IsEmailExist(createdmember.Email) || IsPhoneExist(createdmember.Phone)) return false;

                var member = new Member()
                {
                    Name = createdmember.Name,
                    Email = createdmember.Email,
                    Phone = createdmember.Phone,
                    Gender = createdmember.Gender,
                    DateOfBirth = createdmember.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                    Address = new Address()
                    {
                        BuildingNumber = createdmember.BuildingNumber,
                        City = createdmember.City,
                        Street = createdmember.Street
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = createdmember.HealthRecordViewModel.Height,
                        Weight = createdmember.HealthRecordViewModel.Weight,
                        BloodType = createdmember.HealthRecordViewModel.BloodType,
                        Note = createdmember.HealthRecordViewModel.Note,
                    }

                };
                 _unitOfWork.GetRepository<Member>().Add(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();

            var MemberViewModels = new List<MemberViewModel>();
            foreach (var member in members)
            {
                var memberViewModel = new MemberViewModel()
                {
                    Id = member.Id,
                    Name = member.Name,
                    Email = member.Email,
                    Phone = member.Phone,
                    Photo = member.Photo,
                    Gender = member.Gender.ToString()
                };
                MemberViewModels.Add(memberViewModel);
            }
            ;
            return MemberViewModels;
        }

        public MemberViewModel? GetMemberDetails(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null) return null;
            var ViewModel = new MemberViewModel()
            {
              Name=member.Name,
              Email=member.Email,
              Phone=member.Phone,
              Gender=member.Gender.ToString(),
              DateOfBirth=member.DateOfBirth.ToShortDateString(),
              Address=$"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}",
              Photo=member.Photo
            };
            var ActiveMemberShip=_unitOfWork.GetRepository<Membership>().GetAll(x=>x.Id == memberId && x.Status=="Active")
                .FirstOrDefault();
            if(ActiveMemberShip is not null)
            {
                ViewModel.MemberShipStartDate=ActiveMemberShip.CreatedAt.ToShortDateString();
                ViewModel.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();
            }
            if (ActiveMemberShip is not null)
            {
                var plan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMemberShip.PlanId);
                ViewModel.PlanName = plan?.Name;
            }
            return ViewModel;
         }

        public HealthRecordViewModel? GetMemberHealthRecordDetails(int memberId)
        {
            var member = _unitOfWork.GetRepository<HealthRecord>().GetById(memberId);
           if(member is null) return null;
            return new HealthRecordViewModel()
            {
                Weight = member.Weight,
                Height = member.Height,
                BloodType = member.BloodType,
                Note = member.Note
            };
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int memberId)
        {
            var member= _unitOfWork.GetRepository<Member>().GetById(memberId);
            if(member is null) return null;
            return new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Photo = member.Photo,
                Email = member.Email,
                Phone = member.Phone,
                Street = member.Address.Street,
                City = member.Address.City,
                BuildingNumber = member.Address.BuildingNumber
            };
        }
        public bool RemoveMember(int memberId)
        {
            var MemberRepo= _unitOfWork.GetRepository<Member>();
            var MembershipRepo= _unitOfWork.GetRepository<Membership>();
            var Member = MemberRepo.GetById(memberId);
            if(Member is null) return false;
            var HasActiveMemberSession = _unitOfWork.GetRepository<MemberSession>()
                .GetAll(x=>x.MemberId == memberId && x.Session.StartDate>DateTime.Now).Any();
            if (HasActiveMemberSession) return false;
            var MemberShips= MembershipRepo.GetAll(x=>x.Id == memberId);
            try
            {
                if(MemberShips.Any( ))
                {
                    foreach (var membership in MemberShips)
                    {
                        MembershipRepo.Delete(membership);
                    }
                }
                MemberRepo.Delete(Member) ;
                return _unitOfWork.SaveChanges() > 0;
            }
            catch { return false; }
        }

        public bool UpdateMemberDetails(int Id, MemberToUpdateViewModel UpdatedMember)
        {
            var MemberRepo = _unitOfWork.GetRepository<Member>();
            try
            {
                if (IsEmailExist(UpdatedMember.Email) || IsPhoneExist(UpdatedMember.Phone)) return false;
                var Member = MemberRepo.GetById(Id);
                if (Member is null) return false;
                Member.Phone = UpdatedMember.Phone;
                Member.Email = UpdatedMember.Email;
                Member.Address.Street = UpdatedMember.Street;
                Member.Address.City = UpdatedMember.City;
                Member.Address.BuildingNumber = UpdatedMember.BuildingNumber;
                Member.UpdatedAt = DateTime.Now;

                 MemberRepo.Update(Member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
        #region Add Helper Methods
        private bool IsEmailExist(string email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(x=>x.Email == email).Any();
        }
        private bool IsPhoneExist(string phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(x => x.Phone == phone).Any();
        }

        #endregion
    }
}

