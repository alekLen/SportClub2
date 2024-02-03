using SportClub.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.Interfaces
{
    public interface ICoach
    {
        Task AddCoach(CoachDTO coachDto);
        Task<CoachDTO> GetCoach(int id);
        Task<IEnumerable<CoachDTO>> GetAllCoaches();
        Task DeleteCoach(int id);
        Task UpdateCoach(CoachDTO a);
        Task<bool> CheckPasswordC(CoachDTO u, string p);
        Task<CoachDTO> GetCoachByLogin(string login);
        Task<CoachDTO> GetCoachByEmail(string email);

        Task ChangeCoachPassword(CoachDTO uDto, string pass);
    }
}
