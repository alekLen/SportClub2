using SportClub.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.Interfaces
{
    public interface IUser
    {
        Task AddUser(UserDTO userDto);
        Task<UserDTO> GetUser(int id);
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task DeleteUser(int id);
        Task UpdateUser(UserDTO a);
        Task<bool> CheckPasswordU(UserDTO u, string p);
        Task<UserDTO> GetUserByLogin(string login);
        Task<UserDTO> GetUserByEmail(string email);
    }
}
