using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.DTO
{
    public class GroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;//название группы
        public int Number { get; set; }//количество человек
        public int CoachId { get; set; }//id тренера
        public int trainingId { get; set; }//id тренировки
        public List<int> usersId { get; set; } = new();//список человек
    }
}
