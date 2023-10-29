using SportClub.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.Interfaces
{
    public interface ITime
    {
        Task AddTimeT(TimeTDTO pDto);
        Task<TimeTDTO> GetTimeT(int id);
        Task<IEnumerable<TimeTDTO>> GetAllTimeTs();
        Task DeleteTimeT(int id);
        Task UpdateTimeT(TimeTDTO a);
    }
}
