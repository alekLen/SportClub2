using SportClub.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.Interfaces
{
    public interface ISkipShedule
    {
        Task AddSkipShedule(SkipSheduleDTO skDTO);//
        Task<SkipSheduleDTO> GetSkipShedule(int id);
        //Task<IEnumerable<UserDTO>> GetAllUsers();
        Task DeleteSkipShedule(int id);
        Task UpdateSkipShedule(SkipSheduleDTO sk);
        //Task<bool> CheckPasswordU(UserDTO u, string p);
        //Task<UserDTO> GetUserByLogin(string login);
        //Task<UserDTO> GetUserByEmail(string email);
        //Task ChangeUserPassword(UserDTO uDto, string pass);
    }
}
