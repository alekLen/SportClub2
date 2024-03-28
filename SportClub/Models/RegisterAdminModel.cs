using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SportClub.Models
{
    public class RegisterAdminModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "LogRequired")]
        [Display(Name = "loginN", ResourceType = typeof(Resources.Resource))]
        [Remote("IsAdminLoginInUse", "Login", ErrorMessageResourceType = typeof(Resources.Resource),
           ErrorMessageResourceName = "loginused")]
        [StringLength(15, MinimumLength = 3, ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "LoginLength")]
        public string? Login { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                  ErrorMessageResourceName = "PassRequired")]
        [Display(Name = "password", ResourceType = typeof(Resources.Resource))]
        [Remote("CheckPassword", "Login", ErrorMessageResourceType = typeof(Resources.Resource),
                 ErrorMessageResourceName = "LightPass")]
        [StringLength(15, MinimumLength = 3, ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "PassLength")]
        /*  [Required(ErrorMessageResourceName = "Обязательное поле")]
          [Display(Name = "Password")]
          [Remote("CheckPassword", "Login", ErrorMessageResourceName = "Минимум 8 символов, одна заглавная, одна цифра,один спец.символ")]
          [DataType(DataType.Password)]*/
        public string? Password { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                ErrorMessageResourceName = "PassConRequired")]
        [Display(Name = "passwordConf", ResourceType = typeof(Resources.Resource))]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.Resource),
                ErrorMessageResourceName = "passnoteq")]
        /* [Required(ErrorMessageResourceName = "Обязательное поле")]
         [Display(Name = "Confirm password")]
         [Compare("Password", ErrorMessageResourceName = "Пароли не совпадают")]
         [DataType(DataType.Password)]*/
        public string? PasswordConfirm { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Resource),
                ErrorMessageResourceName = "unemail")]
        /*  [Required(ErrorMessageResourceName = "Обязательное поле")]
          [Display(Name = "email ")]
          [EmailAddress(ErrorMessageResourceName = "не корректный ввод")]*/
        [Remote("IsEmailInUse", "Login", ErrorMessageResourceType = typeof(Resources.Resource),
                ErrorMessageResourceName = "emailused")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "EmailRequired")]
        // [Remote("IsEmailInUse", "Login", ErrorMessageResourceName = "email уже зарегестрирован")]
        public string? Email { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "DateB", ResourceType = typeof(Resources.Resource))]
        //[RegularExpression(@"^(0[1-9]|[12][0-9]|3[01]).(0[1-9]|1[012]).\d{4}$", ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "DateFormatError")]
        public string? DateOfBirth { get; set; }//^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/([19 | 20]\d\d)$
      
         

        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Phone", ResourceType = typeof(Resources.Resource))]
        //[RegularExpression(@"\(\d{3}\)-\d{3} \d{2} \d{2}", ErrorMessage = "Введите номер в формате (097)-111 11 11")]
        [RegularExpression(@"^(\+\d{1,3}\s?)?(\(\d{3}\)|\d{3}[-\.\s]?)(\d{3}[-\.\s]?)(\d{4})$", ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "PhoneFormatError")]
        public string? Phone { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "gender", ResourceType = typeof(Resources.Resource))]
        public string Gender { get; set; }


        /* [Required(ErrorMessageResourceName = "Обязательное поле")]*/
        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Required")]
        [RegularExpression("^[a-zA-Z-а-яА-Я' ']+$", ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "NameFormatError")]
        public string Name { get; set; }
    }
}
