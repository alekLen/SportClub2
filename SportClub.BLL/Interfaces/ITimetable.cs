using SportClub.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.Interfaces
{
    public interface ITimetable
    {
        Task AddTimetable(TimetableDTO pDto);
        Task AddTimeToTimetable(string start, string end, TimetableDTO time);
        Task<TimetableDTO> GetTimetable(int id);
        Task<IEnumerable<TimetableDTO>> GetAllTimetables();
        Task DeleteTimetable(int id);
        Task UpdateTimetable(TimetableDTO a);

    }
}
