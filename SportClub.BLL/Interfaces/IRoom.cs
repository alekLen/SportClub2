using SportClub.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.Interfaces
{
    public interface IRoom   
    {
        Task AddRoom(RoomDTO pDto);
        Task AddSheduleToRoom(RoomDTO pDto, int id);
        Task AddDeleteSheduleFromRoom(RoomDTO pDto);
        Task<RoomDTO> GetRoom(int id);
        Task<IEnumerable<RoomDTO>> GetAllRooms();
        Task DeleteRoom(int id);
        Task UpdateRoom(RoomDTO pDto, int id);
        Task Update(RoomDTO pDto);

    }
}
