using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Entities
{
    public class Shedule
    {
        public int Id { get; set; }
       //  public List<Timetable> Week { get; set; } =new List<Timetable>();
        // public List<TrainingInd> trainingInds { get; set; }
       public Timetable Monday { get; set; }
        public Timetable Tuesday { get; set; }
        public Timetable Wednesday { get; set; }
        public Timetable Thursday { get; set; }
        public Timetable Friday { get; set; }
        public Timetable Saturday { get; set; }
        public Timetable Sunday { get; set; }
    }
}
