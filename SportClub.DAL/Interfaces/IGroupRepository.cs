using SportClub.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Interfaces
{
    public interface IGroupRepository : ISetGetRepository<Group>
    {
        Task<Group> GetGroupName(string name);
        //Task<Coach> GetCoachEmail(string email);
    }
  
}
