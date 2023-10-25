using SportClub.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Interfaces
{
    public interface ICoachRepository : ISetGetRepository<Coach>
    {
        Task<Coach> GetCoachLogin(string login);
        Task<Coach> GetCoachEmail(string email);
    }
  
}
