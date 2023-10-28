using SportClub.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.Interfaces
{
    public interface ITrainingInd
    {
        Task AddTrainingInd(TrainingIndDTO postDto);
        Task<TrainingIndDTO> GetTrainingInd(int id);
        Task<IEnumerable<TrainingIndDTO>> GetAllTrainingInds();
        Task DeleteTrainingInd(int id);
        Task UpdateTrainingInd(TrainingIndDTO a);
    }
}
