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

        public string Name { get; set; } = string.Empty;//название тренировки

        public string? TimeName { get; set; }

        public int Day { get; set; }
        public string? DayName { get; set; }

        public string RoomName { get; set; } = string.Empty;
        public int RoomId { get; set; }//id зала

        public string CoachName { get; set; } = string.Empty;//имя тренера//
        public int? CoachId { get; set; }//id тренера
        public string? CoachPhoto { get; set; }
        public List<UserDTO> UsersId { get; set; } = new();//список человек

        //public string typeflag { get => "TrainingGroup"; }
    }
}
