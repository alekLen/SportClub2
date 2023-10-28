using SportClub.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Interfaces
{
    public interface ITrainingIndRepository :ISetGetRepository<TrainingInd>
    {
         Task<IEnumerable<TrainingInd>> GetAllOfCoach(int id);
        Task<IEnumerable<TrainingInd>> GetAllOfClient(int id)
    }
}
