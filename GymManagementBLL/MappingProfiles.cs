using AutoMapper;
using GymManagementBLL.ViewModels;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL
{
   public class MappingProfiles:Profile
    {
        public MappingProfiles() 
        {
            MapSession();
            MapMember();
            MapTrainer();
            MapPlan();
        }
        private void MapSession()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName, options => options.MapFrom(src => src.SessionCategory.CategoryName))
                .ForMember(dest => dest.TrainerName, options => options.MapFrom(src => src.TrainerSessions.Name))
                .ForMember(dest => dest.AvailableSlots, options => options.Ignore());
            CreateMap<SessionViewModel, Session>();
            CreateMap<UpdateSessionViewModel, Session>().ReverseMap();
        }
        private void MapMember()
        {
            //CreateMap<CreateMemberViewModel, Member>()
            //    .ForMember(dest => dest.Address, options =>
            //    options.MapFrom(src => new Address()
            //    {
            //        BuildingNumber = src.BuildingNumber,
            //        Street = src.Street,
            //        City = src.City
            //    }
            //        ));
            //OR
            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, options =>
                options.MapFrom(src => src))
            .ForMember(dest => dest.HealthRecord, options => options.
            MapFrom(src => src.HealthRecordViewModel));
            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();
            CreateMap<CreateMemberViewModel, Address>()
                .ForMember(dest => dest.BuildingNumber, options => options.MapFrom(src => src.BuildingNumber))
                .ForMember(dest => dest.Street, options => options.MapFrom(src => src.Street))
                .ForMember(dest => dest.City, options => options.MapFrom(src => src.City));
            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Gender, options => options
                .MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.DateOfBirth, options => options
                .MapFrom(src => src.DateOfBirth.ToShortDateString()))
                .ForMember(dest => dest.Address, options => options
                .MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest=>dest.Street,options=>options
                .MapFrom(src=>src.Address.Street))
                .ForMember(dest => dest.City, options => options
                .MapFrom(src => src.Address.City))
                .ForMember(dest => dest.BuildingNumber, options => options
                .MapFrom(src => src.Address.BuildingNumber));
            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, options => options.Ignore())
                .ForMember(dest => dest.Photo, options => options.Ignore())
                .AfterMap((src, dest) =>
                {
                     dest.Address.Street= src.Street;
                     dest.Address.City= src.City;
                     dest.Address.BuildingNumber= src.BuildingNumber;
                    dest.UpdatedAt = DateTime.Now; //not accurate
                });





        }
        private void MapTrainer()
        {
            CreateMap<Trainer, TrainerViewModel>();
            CreateMap<CreateTrainerViewModel, Trainer>()
               .ForMember(dest => dest.Address, options => options
               .MapFrom(src => new Address()
               {
                   BuildingNumber= src.BuildingNumber,
                   Street= src.Street,
                   City= src.City
               }));
            CreateMap<Trainer, TrainerDetailsViewModel>()
                .ForMember(dest => dest.HireDate, options => options.Ignore())
                .ForMember(dest => dest.Address, options => options.
                MapFrom(src => $"{src.Address.BuildingNumber}-{src.Address.Street}-{src.Address.City}"));
            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dest => dest.Street, options => options.
                MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, options => options.
                MapFrom(src => src.Address.City))
                .ForMember(dest => dest.BuildingNumber, options => options.
                MapFrom(src => src.Address.BuildingNumber));

            CreateMap<UpdateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Name, options => options.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildingNumber= src.BuildingNumber;
                    dest.Address.City= src.City;
                    dest.Address.Street= src.Street;
                    dest.UpdatedAt = DateTime.Now;
                });
        }
        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, UpdatePlanViewModel>()
                .ForMember(dest => dest.planName, options => options.
                MapFrom(src=>src.Name));
            CreateMap<UpdatePlanViewModel, Plan>()
                .ForMember(dest => dest.Name, options => options.Ignore())
                .ForMember(dest=>dest.UpdatedAt,options=>options.
                MapFrom(src=>DateTime.Now));
        }

    }
}
