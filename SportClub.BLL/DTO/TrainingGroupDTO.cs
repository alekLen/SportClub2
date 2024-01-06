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
        //public int Number { get; set; }
        public string Name { get; set; } = string.Empty;//название тренировки

        public int TimeId { get; set; }
        public int RoomId { get; set; }//id зала

        public string CoachName { get; set; } = string.Empty;//имя тренера
        public int? CoachId { get; set; }//id тренера

        public string GroupName { get; set; } = string.Empty;//имя группы
        public int? GroupId { get; set; }//id группы

        public string SpecialityName { get; set; } = string.Empty;//имя специальности тренировки
        public int? SpecialityId { get; set; }//id специальности тренировки
    }
}
