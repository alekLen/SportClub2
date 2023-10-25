using SportClub.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Interfaces
{
    public interface ISaltRepository:ISetGetRepository<Salt>
    {
        Task<Salt> GetAdminSalt(Admin a);
        Task<Salt> GetCoachSalt(Coach a);
        Task<Salt> GetUserSalt(User a);

    }
}
