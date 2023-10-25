using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.DTO
{
    public class TrainingDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TimeId { get; set; }
        public int RoomId { get; set; }
        public int typeOfTrainingId { get; set; }
    }
}
