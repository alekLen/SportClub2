using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.DTO
{
    public class PersonDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Surname { get; set; } = string.Empty;
        public string? Dopname { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public int? Age { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
    }
}
