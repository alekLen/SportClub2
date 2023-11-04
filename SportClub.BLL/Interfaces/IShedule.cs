using SportClub.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.Interfaces
{
    public interface IShedule
    {
        Task AddShedule(SheduleDTO pDto);
        Task<SheduleDTO> GetShedule(int id);
        Task<IEnumerable<SheduleDTO>> GetAllShedules();
        Task DeleteShedule(int id);
        Task UpdateShedule(SheduleDTO pDto);
    }
}
