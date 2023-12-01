using SportClub.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.DTO
{
    public class SheduleDTO
    {
        public int Id { get; set; }
        public List<TimetableDTO> timetables { get; set; } = new();
        public List<TrainingGroupDTO> trainingGroup { get; set; } = new();
        public List<TrainingIndDTO> trainingInd { get; set; } = new();
       
    }
}
