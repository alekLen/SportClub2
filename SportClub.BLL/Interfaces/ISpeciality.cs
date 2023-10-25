using SportClub.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.Interfaces
{
    public interface ISpeciality
    {
        Task AddSpeciality(SpecialityDTO sDto);
        Task<SpecialityDTO> GetSpeciality(int id);
        Task<IEnumerable<SpecialityDTO>> GetAllSpecialitys();
        Task DeleteSpeciality(int id);
        Task UpdateSpeciality(SpecialityDTO a);
    }
}
