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

        public bool CreateSession(CreateSessionViewModel createdSession)
        {
            try {
                // Check If Trainer Exists
                if (!IsTrainerExists(createdSession.TrainerId)) return false;
                // Check If Category Exists
                if(!IsCategoryExists(createdSession.CategoryId)) return false;
                //Check If StartDate is before EndDate
                if(!IsDateTimeValid(createdSession.StartDate,createdSession.EndDate)) return false;
                //Check Capacity is limited to 1-25
                if(createdSession.Capacity>25 || createdSession.Capacity<1) return false;

                var SessionEntity = _mapper.Map<Session>(createdSession);
                _unitOfWork.GetRepository<Session>().Add(SessionEntity);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex) {
                Console.WriteLine($"Create Session Failed : {ex}");
                return false;
            }
        }
        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var session=_unitOfWork.SessionRepository.GetById(sessionId);
            if(!IsSessionAvailableForUpdating(session!)) return null;
            return _mapper.Map<UpdateSessionViewModel>(session);
        }
        public bool UpdateSession(UpdateSessionViewModel updatedSession, int sessionId)
        {
            try {
                var session = _unitOfWork.SessionRepository.GetById(sessionId);
                if(!IsSessionAvailableForUpdating(session!)) return false;
                if (!IsTrainerExists(updatedSession.TrainerId)) return false;
                if (!IsDateTimeValid(updatedSession.StartDate, updatedSession.EndDate)) return false;
                _mapper.Map(updatedSession, session);
                session!.UpdatedAt = DateTime.Now;
                _unitOfWork.SessionRepository.Update(session);
                return _unitOfWork.SaveChanges()>0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update Session Failed: {ex}");
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
        private bool IsSessionAvailableForUpdating(Session session)
        {
            //If Session Not Exists - No Update Allowed
            if (session is null) return false;
            //If Session Completed - No Update Allowed
            if(session.EndDate<DateTime.Now) return false;
            //If Session Started - No Update Allowed
            if (session.StartDate <= DateTime.Now) return false;
            //If Session Has Active Booking - No Update Allowed
            var hasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (hasActiveBooking) return false;
            return true;
        }
        #endregion
    }
}
