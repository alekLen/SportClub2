using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportClub.BLL.DTO;

namespace SportClub.BLL.Interfaces
{
    public interface IGroup
    {
        Task AddGroup(GroupDTO groupDto);
        Task<GroupDTO> GetGroup(int id);
        Task<IEnumerable<GroupDTO>> GetAllGroups();
        Task DeleteGroup(int id);
        Task UpdateGroup(GroupDTO a);
        //Task<bool> CheckPasswordA(AdminDTO u, string p);
        Task<GroupDTO> GetGroupByName(string name);
        //Task<AdminDTO> GetAdminByEmail(string email);
        //Task ChangeAdminPassword(AdminDTO uDto, string pass);

        Task<IEnumerable<UserDTO>> GetGroupUsers(int id);
    }
}
