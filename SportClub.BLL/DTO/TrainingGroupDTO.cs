using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.DTO
{
    public class TrainingGroupDTO
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public int TimeId { get; set; }
        public int RoomId { get; set; }
        public string CoachName { get; set; }
        public int? CoachId { get; set; }
        public string GroupName { get; set; }
        public int? GroupId { get; set; }
        public string SpecialityName { get; set; }
        public int? SpecialityId { get; set; }
    }
}
