using AutoMapper;
using GymManagementBLL.ViewModels;
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
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName, options => options.MapFrom(src => src.SessionCategory.CategoryName))
                .ForMember(dest => dest.TrainerName, options => options.MapFrom(src => src.TrainerSessions.Name))
                .ForMember(dest => dest.AvailableSlots, options => options.Ignore());
                
        }
    }
}
