using Microsoft.AspNetCore.Mvc;
using SportClub.BLL.DTO;
using System.ComponentModel.DataAnnotations;

namespace SportClub.Models
{
    public class CreateGroupTrainingModel
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



        [Required(ErrorMessage = "Required")]
        [Display(Name = "Time")]
        public int TimeId { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Coach")]
        public int RoomId { get; set; }//id зала


        [Required(ErrorMessage = "Required")]
        [Display(Name = "Group")]
        public int GroupId { get; set; }//id группы

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Speciality")]
        public int? SpecialityId { get; set; }//id специальности тренировки
    }
}
