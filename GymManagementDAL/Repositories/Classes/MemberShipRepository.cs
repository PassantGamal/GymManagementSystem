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
    public class MemberShipRepository : IMemberShipRepository
    {
        private readonly GymDbContext _dbContext;
        public MemberShipRepository(GymDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public int Add(Membership membership)
        {
            _dbContext.Memberships.Add(membership);
            return _dbContext.SaveChanges();
        }

        public int Delete(Membership membership)
        {
            var memberShip = _dbContext.Memberships.Find(membership);
            if (memberShip is null)
                return 0;  
            _dbContext.Memberships.Remove(membership);
            return _dbContext.SaveChanges();

        }

        public IEnumerable<Membership> GetAll()
        {
            return _dbContext.Memberships.ToList();
        }

        public Membership? GetById(int id)
        {
            return _dbContext.Memberships.Find(id);
        }

        public int Update(Membership membership)
        {
            _dbContext.Memberships.Update(membership);
            return _dbContext.SaveChanges();
        }
    }
}
