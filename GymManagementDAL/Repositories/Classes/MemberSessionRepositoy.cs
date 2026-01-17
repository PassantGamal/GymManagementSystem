using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    internal class MemberSessionRepositoy : IMemberSessionRepository
    {
        private readonly GymDbContext _dbContext;
        public MemberSessionRepositoy(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Add(MemberSession memberSession)
        {
            _dbContext.MemberSessions.Add(memberSession);
            return _dbContext.SaveChanges();
        }

        public int Delete(MemberSession memberSession)
        {
            var membersession = _dbContext.MemberSessions.Find(memberSession);
            if (membersession is null)
                return 0;
            _dbContext.MemberSessions.Remove(membersession);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<MemberSession> GetAll()
        {
            return _dbContext.MemberSessions.ToList();
        }

        public MemberSession? GetById(int id)
        {
            return _dbContext.MemberSessions.Find(id);
        }

        public int Update(MemberSession memberSession)
        {
            _dbContext.MemberSessions.Update(memberSession);
            return _dbContext.SaveChanges();
        }
    }
}
