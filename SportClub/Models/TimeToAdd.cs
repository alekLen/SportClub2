using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SportClub.Models
{
    public class TimeToAdd
    {
      
        [RegularExpression(@"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Введите время в формате 00:00")]

        public string StartTime { get; set; }

        [Remote("CheckTime", "Time", ErrorMessage = "время окончаня должно быть позже, чем время начала")]
        [RegularExpression(@"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Введите время в формате 00:00")]
        public string EndTime { get; set; }
    }
}
