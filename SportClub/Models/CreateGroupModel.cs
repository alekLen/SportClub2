using Microsoft.AspNetCore.Mvc;
using SportClub.BLL.DTO;
using System.ComponentModel.DataAnnotations;

namespace SportClub.Models
{
    public class CreateGroupModel
    {
        [Required(ErrorMessage="Required")]
        [Display(Name = "Group name")] 
        public string Name { get; set; } = string.Empty;//название группы


        [Required(ErrorMessage = "Required")]
        [Display(Name = "Count users")]
        [Range(0, 35)]
        public int Number { get; set; }//количество человек


        [Required(ErrorMessage = "Required")]
        [Display(Name = "Coach")] 
        public int CoachId { get; set; }//id тренера

        //[Required(ErrorMessage = "Required")]
        //[Display(Name = "Training")]
        //public int TrainingId { get; set; }//id тренировки

        [Required(ErrorMessage = "Required")]
        [Display(Name = "users")]
        public List<UserDTO> UsersId { get; set; } = new();//список человек
    }
}
