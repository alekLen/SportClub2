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
        Task<AdminDTO> GetAdmin(int id);
        Task<IEnumerable<AdminDTO>> GetAllAdmins();
        Task DeleteAdmin(int id);
        Task UpdateAdmin(AdminDTO a);
        Task<bool> CheckPasswordA(AdminDTO u, string p);
        Task<AdminDTO> GetAdminByLogin(string login);
        Task<AdminDTO> GetAdminByEmail(string email);
        Task ChangeAdminPassword(AdminDTO uDto, string pass);
    }
}
