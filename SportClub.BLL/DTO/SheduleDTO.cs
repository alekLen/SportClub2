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
        //public List<int> trainingId { get; set; } = new();
        public int MondayId { get; set; }
        public int TuesdayId { get; set; }
        public int WednesdayId { get; set; }
        public int ThursdayId { get; set; }
        public int FridayId { get; set; }
        public int SaturdayId { get; set; }
        public int SundayId { get; set; }
    }
}
