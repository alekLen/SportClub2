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
        public List<TrainingInd> trainingInds { get; set; }
    }
}
