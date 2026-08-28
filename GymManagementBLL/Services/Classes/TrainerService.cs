using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var Trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (Trainers is null || !Trainers.Any()) return [];
            return Trainers.Select(T => new TrainerViewModel()
            {
                Id = T.Id,
                Name = T.Name,
                Email = T.Email,
                Phone=T.Phone,
                Specialty = T.Specialty.ToString(),
            }
            );
        }
        public bool CreateTrainer(CreateTrainerViewModel createdTrainer)
        {
            try {
                var repo= _unitOfWork.GetRepository<Trainer>();
                if (IsEmailExist(createdTrainer.Email) || IsPhoneExist(createdTrainer.Phone)) return false;
                var Trainer=new Trainer(){
                    Name = createdTrainer.Name,
                    Email=createdTrainer.Email,
                    Phone = createdTrainer.Phone,
                    DateOfBirth=createdTrainer.DateOfBirth,
                    Specialty=createdTrainer.Specialty,
                    Gender=createdTrainer.Gender,
                    Address=new Address()
                    {
                        BuildingNumber=createdTrainer.BuildingNumber,
                        Street=createdTrainer.Street,
                        City=createdTrainer.City
                    }
                };
                repo.Add( Trainer );
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception) { return false; }
        }

       

        public TrainerDetailsViewModel? GetTrainerById(int trainerId)
        {
            var Trainer=_unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (Trainer is null) return null;
            return new TrainerDetailsViewModel()
            {
                Id = Trainer.Id,
                Name = Trainer.Name,
                Email = Trainer.Email,
                Phone = Trainer.Phone,
                DateOfBirth = Trainer.DateOfBirth,
                Specialty = Trainer.Specialty,
                Address = $"{Trainer.Address.BuildingNumber}-{Trainer.Address.Street}-{Trainer.Address.City}"
            };
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (Trainer is null) return null;
            return new TrainerToUpdateViewModel()
            {
                Name = Trainer.Name, //For Display
                Email = Trainer.Email,
                Phone = Trainer.Phone,
                Street = Trainer.Address.Street,
                BuildingNumber = Trainer.Address.BuildingNumber,
                City = Trainer.Address.City,
                Specialty = Trainer.Specialty,
            };
        }
        public bool UpdateTrainer(int trainerId, UpdateTrainerViewModel updatedTrainer)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToUpdate = repo.GetById(trainerId);
            if(TrainerToUpdate is null || IsEmailExist(updatedTrainer.Email) || IsPhoneExist(updatedTrainer.Phone)) return false;
            TrainerToUpdate.Email = updatedTrainer.Email;
            TrainerToUpdate.Phone = updatedTrainer.Phone;
            TrainerToUpdate.Address.BuildingNumber = updatedTrainer.BuildingNumber;
            TrainerToUpdate.Address.Street = updatedTrainer.Street;
            TrainerToUpdate.Address.City= updatedTrainer.City;
            TrainerToUpdate.Specialty = updatedTrainer.Specialty;
            TrainerToUpdate.UpdatedAt=DateTime.Now;
            repo.Update(TrainerToUpdate);
            return _unitOfWork.SaveChanges()>0;

        }
        public bool RemoveTrainer(int trainerId)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToRemove = repo.GetById(trainerId);
            if(TrainerToRemove is null || HasActiveSessions(trainerId)) return false;
            repo.Delete(TrainerToRemove);
            return _unitOfWork.SaveChanges() > 0;
        }

        #region Helper
        private bool IsEmailExist(string email)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(T => T.Email == email).Any();
        }
        private bool IsPhoneExist(string phone) 
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(T => T.Phone == phone).Any();
        }
        private bool HasActiveSessions(int id)
        {
            var activeSessions=_unitOfWork.GetRepository<Session>().GetAll
                (s=>s.TrainerId == id && s.StartDate>DateTime.Now).Any();
            return activeSessions;
        }
        #endregion
    }
}
