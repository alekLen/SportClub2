using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SportClub.Models
{
    public class CangePasswordModel
    {
        public int Id { get; set; }
       
        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                 ErrorMessageResourceName = "PassRequired")]
        [Display(Name = "password", ResourceType = typeof(Resources.Resource))]
        [Remote("CheckPassword", "Login", ErrorMessageResourceType = typeof(Resources.Resource),
                ErrorMessageResourceName = "LightPass")]      
        public string? NewPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                ErrorMessageResourceName = "PassConRequired")]
        [Display(Name = "passwordConf", ResourceType = typeof(Resources.Resource))]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.Resource),
                ErrorMessageResourceName = "passnoteq")]
        public string? PasswordConfirm { get; set; }
    }
}
