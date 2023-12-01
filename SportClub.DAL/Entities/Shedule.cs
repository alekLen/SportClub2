﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Entities
{
    public class Shedule
    {
        public int Id { get; set; }
        public List<Timetable> Week { get; set; } =new List<Timetable>();
       // public List<TrainingInd> trainingInds { get; set; }
       public int Monday { get; set; }
        public int Tuesday { get; set; }
        public int Wednesday { get; set; }
        public int Thursday { get; set; }
       public int Friday { get; set; }
       public int Saturday { get; set; }
        public int Sunday { get; set; }
    }
}
