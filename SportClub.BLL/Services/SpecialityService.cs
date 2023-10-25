using SportClub.BLL.Interfaces;
using SportClub.BLL.Infrastructure;
using SportClub.DAL.Interfaces;
using SportClub.BLL.DTO;
using SportClub.DAL.Entities;
using AutoMapper;

namespace SportClub.BLL.Services
{
    public class SpecialityService: ISpeciality
    {
        IUnitOfWork Database { get; set; }

        public SpecialityService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public async Task AddSpeciality(SpecialityDTO pDto)
        {
            var a = new Speciality()
            {
                Name = pDto.Name
            };
            await Database.Specialitys.AddItem(a);
            await Database.Save();
        }
        public async Task<SpecialityDTO> GetSpeciality(int id)
        {
            Speciality a = await Database.Specialitys.Get(id);
            if (a == null)
                throw new ValidationException("Wrong", "");
            return new SpecialityDTO
            {
                Id = a.Id,
                Name = a.Name,
            };
        }
        public async Task<IEnumerable<SpecialityDTO>> GetAllSpecialitys()
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Speciality, SpecialityDTO>());
                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<Speciality>, IEnumerable<SpecialityDTO>>(await Database.Specialitys.GetAll());
            }
            catch { return null; }
        }
        public async Task DeleteSpeciality(int id)
        {
            await Database.Specialitys.Delete(id);
            await Database.Save();
        }
        public async Task UpdateSpeciality(SpecialityDTO a)
        {
            Speciality s = await Database.Specialitys.Get(a.Id);
            s.Name = a.Name;
            await Database.Specialitys.Update(s);
            await Database.Save();
        }
    }
}
