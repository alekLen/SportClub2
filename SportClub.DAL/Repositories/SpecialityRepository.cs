using Microsoft.EntityFrameworkCore;
using SportClub.DAL.Interfaces;
using SportClub.DAL.EF;
using SportClub.DAL.Entities;

namespace SportClub.DAL.Repositories
{
    public class SpecialityRepository : ISetGetRepository<Speciality>
    {
        private SportClubContext db;

        public SpecialityRepository(SportClubContext context)
        {
            this.db = context;
        }
        public async Task<IEnumerable<Speciality>> GetAll()
        {
            return await db.Specialitys.ToListAsync();
        }
        public async Task<Speciality> Get(int id)
        {
            return await db.Specialitys.FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task AddItem(Speciality c)
        {
            await db.AddAsync(c);
        }
        public async Task Update(Speciality c)
        {
            var f = await db.Specialitys.FirstOrDefaultAsync(m => m.Id == c.Id);
            if (f != null)
            {
                db.Specialitys.Update(c);

            }
        }
        public async Task Delete(int id)
        {
            var c = await db.Specialitys.FindAsync(id);
            if (c != null)
            {
                db.Specialitys.Remove(c);

            }
        }
    }
}
