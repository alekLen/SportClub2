using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Entities
{
    public class Timetable
    {
        public int Id { get; set; }
        public List<TimeT> Times{ get; set; }
    }
}
