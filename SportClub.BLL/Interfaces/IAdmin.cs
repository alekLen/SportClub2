using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportClub.BLL.DTO;

namespace SportClub.BLL.Interfaces
{
    public interface IAdmin
    {
        Task AddAdmin(AdminDTO adminDto);
       /* Task<AdminDTO> GetAdmin(int id);
        Task<IEnumerable<AdminDTO>> GetAllAdmins();
        Task DeleteAdmin(int id);
        Task UpdateAdmin(int id, string n, string p);*/
    }
}
