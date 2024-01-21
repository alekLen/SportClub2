using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.DTO
{
    public class GroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;//название группы
        [Display(Name = "Count users")]
        public int Number { get; set; }//количество человек

        //public string CoachName { get; set; } = string.Empty;//имя тренера
        //public int CoachId { get; set; }//id тренера
        //public int trainingId { get; set; }//id тренировки

        public List<UserDTO> UsersId { get; set; } = new();//список человек
    }
}
