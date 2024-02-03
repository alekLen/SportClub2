using SportClub.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.Interfaces
{
    public interface ITrainingGroup
    {
        Task AddTrainingGroup(TrainingGroupDTO tDto);
        Task<TrainingGroupDTO> GetTrainingGroup(int id);
        Task<IEnumerable<TrainingGroupDTO>> GetAllTrainingGroups();
        Task<IEnumerable<TrainingGroupDTO>> GetAllOfCoachTrainingGroups(int id);
        Task<IEnumerable<TrainingGroupDTO>> GetAllOfClientTrainingGroups(int id);
        Task DeleteTrainingGroup(int id);
        Task UpdateTrainingGroup(TrainingGroupDTO a);

        Task<IEnumerable<UserDTO>> GetTrainingGroupUsers(int id);
    }
}
