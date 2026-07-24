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
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly IGenericRepository<Membership> _membershipRepository;
        private readonly IGenericRepository<Plan> _planRepository;
        private readonly IGenericRepository<HealthRecord> _healthRecordRepository;
        public MemberService(IGenericRepository<Member> memberRepository,IGenericRepository<Membership> membershipRepository,IGenericRepository<Plan>planRepository,IGenericRepository<HealthRecord> healthRecordRepository)
        {
            _memberRepository = memberRepository;
            _membershipRepository = membershipRepository;
           _planRepository = planRepository;
            _healthRecordRepository = healthRecordRepository;
        }

        public bool CreateMember(CreateMemberViewModel createdmember)
        {
            try
            {
                var PhoneExists = _memberRepository.GetAll(x => x.Phone == createdmember.Phone).Any();
                var EmailExists = _memberRepository.GetAll(x => x.Email == createdmember.Email).Any();
                if (PhoneExists || EmailExists)
                    return false;

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
                return _memberRepository.Add(member) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _memberRepository.GetAll();

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
            var member = _memberRepository.GetById(memberId);
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
            var ActiveMemberShip=_membershipRepository.GetAll(x=>x.Id == memberId && x.Status=="Active")
                .FirstOrDefault();
            if(ActiveMemberShip is not null)
            {
                ViewModel.MemberShipStartDate=ActiveMemberShip.CreatedAt.ToShortDateString();
                ViewModel.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();
            }
            if (ActiveMemberShip is not null)
            {
                var plan = _planRepository.GetById(ActiveMemberShip.PlanId);
                ViewModel.PlanName = plan?.Name;
            }
            return ViewModel;
         }

        public HealthRecordViewModel? GetMemberHealthRecordDetails(int memberId)
        {
            var member = _healthRecordRepository.GetById(memberId);
           if(member is null) return null;
            return new HealthRecordViewModel()
            {
                Weight = member.Weight,
                Height = member.Height,
                BloodType = member.BloodType,
                Note = member.Note
            };
        }
    }
}

