using Microsoft.EntityFrameworkCore;
using SportClub.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportClub.DAL.EF;
using SportClub.DAL.Entities;

namespace SportClub.DAL.Repositories
{
    public class CoachRepository : ICoachRepository
    {
        private SportClubContext db;

        public CoachRepository(SportClubContext context)
        {
            this.db = context;
        }
        public async Task<IEnumerable<Coach>> GetAll()
        {
            return await db.Coaches.ToListAsync();
        }
        public async Task<Coach> Get(int id)
        {
            return await db.Coaches.FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task AddItem(Coach c)
        {
            await db.AddAsync(c);
        }
        public async Task Update(Coach c)
        {
            var f = await db.Coaches.FirstOrDefaultAsync(m => m.Id == c.Id);
            if (f != null)
            {
                db.Coaches.Update(c);

            }
        }
        public async Task Delete(int id)
        {
            var c = await db.Coaches.FindAsync(id);
            if (c != null)
            {
                db.Coaches.Remove(c);

            }
        }
        public async Task<Coach> GetCoachLogin(string login)
        {
            return await db.Coaches.FirstOrDefaultAsync(m => m.Login == login);
        }
        public async Task<Coach> GetCoachEmail(string email)
        {
            return await db.Coaches.FirstOrDefaultAsync(m => m.Email == email);
        }
    }
}
