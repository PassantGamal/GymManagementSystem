using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork,IMapper mapper)
        {
          _unitOfWork = unitOfWork;
          _mapper = mapper;
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory();
            if (!Sessions.Any()) return [];
            //return Sessions.Select(S => new SessionViewModel
            //{
            //    Id = S.Id,
            //    Description = S.Description,
            //    StartDate = S.StartDate,
            //    EndDate = S.EndDate,
            //    Capacity = S.Capacity,
            //    TrainerName = S.TrainerSessions.Name,
            //    CategoryName = S.SessionCategory.CategoryName,
            //    AvailableSlots = S.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(S.Id)

            //}
            //);
            var MappedSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(Sessions);
            foreach (var Session in MappedSessions)
                Session.AvailableSlots = Session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(Session.Id);
            return MappedSessions;
        }

        public SessionViewModel? GetSessionById(int sessionId)
        {
            var session=_unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId);
            if (session is null) return null;
            var MappedSession = _mapper.Map<Session, SessionViewModel>(session);
            MappedSession.AvailableSlots = MappedSession.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(MappedSession.Id);
            return MappedSession;
        }

        public bool CreateSession(CreateSessionViewModel CreatedSession)
        {
            try {
                // Check If Trainer Exists
                if (!IsTrainerExists(CreatedSession.TrainerId)) return false;
                // Check If Category Exists
                if(!IsCategoryExists(CreatedSession.CategoryId)) return false;
                //Check If StartDate is before EndDate
                if(!IsDateTimeValid(CreatedSession.StartDate,CreatedSession.EndDate)) return false;
                //Check Capacity is limited to 1-25
                if(CreatedSession.Capacity>25 || CreatedSession.Capacity<1) return false;

                var SessionEntity = _mapper.Map<Session>(CreatedSession);
                _unitOfWork.GetRepository<Session>().Add(SessionEntity);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex) {
                Console.WriteLine($"Created Session Failed : {ex}");
                return false;
            }
        }

        #region Helper Methods
        // Check If Trainer Exists
        private bool IsTrainerExists(int TrainerId)
        {
            return _unitOfWork.GetRepository<Session>().GetById(TrainerId) is not null;
        }
        // Check If Category Exists
        private bool IsCategoryExists(int CategoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;
        }
        //Check If StartDate is before EndDate
        private bool IsDateTimeValid(DateTime StartDate, DateTime EndDate)
        {
            return StartDate < EndDate;
        }
        #endregion
    }
}
