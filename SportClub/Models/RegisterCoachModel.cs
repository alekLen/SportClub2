using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SportClub.Models
{
    public class RegisterCoachModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "LogRequired")]
        [Display(Name = "loginN", ResourceType = typeof(Resources.Resource))]
        [Remote("IsCoachLoginInUse", "Login", ErrorMessageResourceType = typeof(Resources.Resource),
         ErrorMessageResourceName = "loginused")]
        public string? Login { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                  ErrorMessageResourceName = "PassRequired")]
        [Display(Name = "password", ResourceType = typeof(Resources.Resource))]
        [Remote("CheckPassword", "Login", ErrorMessageResourceType = typeof(Resources.Resource),
                 ErrorMessageResourceName = "LightPass")]
        public string? Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                ErrorMessageResourceName = "PassConRequired")]
        [Display(Name = "passwordConf", ResourceType = typeof(Resources.Resource))]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.Resource),
                ErrorMessageResourceName = "passnoteq")]
        public string? PasswordConfirm { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Resource),
                ErrorMessageResourceName = "unemail")]
        [Remote("IsEmailInUse", "Login", ErrorMessageResourceType = typeof(Resources.Resource),
                ErrorMessageResourceName = "emailused")]
        public string? Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "DateB", ResourceType = typeof(Resources.Resource))]
        public string? DateOfBirth { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Phone", ResourceType = typeof(Resources.Resource))]
        [RegularExpression(@"\(\d{3}\)-\d{3} \d{2} \d{2}", ErrorMessage = "Введите номер в формате (097)-111 11 11")]
        public string? Phone { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "gender", ResourceType = typeof(Resources.Resource))]
        public string Gender { get; set; }
        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Required")]
        [RegularExpression("^[a-zA-Z-а-яА-Я,' ']+$", ErrorMessage = "Поле 'Имя' должно содержать только буквы.")]
        public string Name { get; set; }
       /* [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Surname", ResourceType = typeof(Resources.Resource))]
        [RegularExpression("^[a-zA-Z-а-яА-Я]+$", ErrorMessage = "Поле 'Имя' должно содержать только буквы.")]
        public string Surname { get; set; }
        [RegularExpression("^[a-zA-Z-а-яА-Я]+$", ErrorMessage = "Поле 'Имя' должно содержать только буквы.")]
        [Display(Name = "Dopname", ResourceType = typeof(Resources.Resource))]
        public string Dopname { get; set; }*/
        
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Post", ResourceType = typeof(Resources.Resource))]
        public int PostId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Speciality", ResourceType = typeof(Resources.Resource))]
        public int SpecialityId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Description", ResourceType = typeof(Resources.Resource))]
        public string Description { get; set; }
    }
}
