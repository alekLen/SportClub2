using SportClub.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Interfaces
{
    public interface ITrainingGroupRepository : ISetGetRepository<TrainingGroup>
    {
        Task<IEnumerable<TrainingGroup>> GetAllOfCoach(int id);
        Task<IEnumerable<TrainingGroup>> GetAllOfClient(User u);
    }
}
