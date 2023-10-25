using SportClub.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Interfaces
{
    public interface IAdminRepository:ISetGetRepository<Admin>
    {
        Task<Admin> GetAdminLogin(string login);
    }
}
