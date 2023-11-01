using SportClub.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Interfaces
{
    public interface ITimeTRepository: ISetGetRepository<TimeT>
    {
        Task<TimeT> Find(string s, string e);
        Task DeleteAll();
    }
}
