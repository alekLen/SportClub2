using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Entities
{
    public class SkipShedule
    {
        public int Id { get; set; }
        public TrainingGroup trainingGroup { get; set; }

        //public string? UserName { get; set; }
        public User User { get; set; }

        public bool SkipMonday { get; set; } = false;
        public bool SkipTuesday { get; set; } = false;
        public bool SkipWednesday { get; set; } = false;
        public bool SkipThursday { get; set; } = false;
        public bool SkipFriday { get; set; } = false;
        public bool SkipSaturday { get; set; } = false;
        public bool SkipSunday { get; set; } = false;
    }
}
